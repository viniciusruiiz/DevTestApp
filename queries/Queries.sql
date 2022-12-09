CREATE DATABASE CreditClearance
GO 

USE CreditClearance
GO

CREATE TABLE Cliente (
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Nome NVARCHAR(255)  NOT NULL,
	CPF CHAR(11)  NOT NULL,
	UF CHAR(2)  NOT NULL,
	Celular VARCHAR(20)  NOT NULL
)
GO

CREATE TABLE TipoFinanciamento (
	Id INT NOT NULL PRIMARY KEY,
	Nome NVARCHAR(255)  NOT NULL
)
GO

CREATE TABLE Financiamento (
	Id INT IDENTITY(1,1) NOT NULL  PRIMARY KEY,
	IdCliente INT NOT NULL FOREIGN KEY REFERENCES Cliente(Id),
	IdTipoFinanciamento INT NOT NULL FOREIGN KEY REFERENCES TipoFinanciamento(Id), 
	CPF CHAR(11) NOT NULL,
	ValorTotal MONEY NOT NULL,
	DataUltimoVencimento DATE NOT NULL
)
GO

CREATE TABLE Parcela (
	Id INT IDENTITY(1,1) NOT NULL  PRIMARY KEY,
	IdFinanciamento INT NOT NULL FOREIGN KEY REFERENCES Financiamento(Id), 
	NumeroParcela INT NOT NULL,
	ValorParcela MONEY NOT NULL,
	DataVencimento DATE NOT NULL,
	DataPagamento DATE
)
GO

INSERT INTO [TipoFinanciamento] VALUES (1,'Consignado')
,(2,'Imobiliário')
GO

INSERT INTO [Cliente] (Nome, CPF, UF, Celular) VALUES ('Roger', '61071373005', 'SP', '+5511912345678')
,('Matheus', '04931772056', 'RJ', '+5511914488569')
,('Sarah', '79903802005', 'SP', '+5511988525698')
,('Paula', '12364784018', 'SP', '+551195815592')
,('Kaike', '53369092034', 'SP', '+551195815592')
,('Renan', '24743372020', 'RJ', '+551195123654')
,('Lorenzo', '50114493006', 'RS', '+551195654345')
,('Jansen', '18278162018', 'RS', '+551195743134')
,('Silvana', '04982742090', 'RJ', '+551195543231')
,('Neide', '15497437087', 'SP', '+551195132144')
GO

INSERT INTO [Financiamento] (IdCliente, IdTipoFinanciamento, CPF, ValorTotal, DataUltimoVencimento) VALUES (1, 1, '04931772056', 10000, '2025-10-19')
,(1, 2, '04931772056', 30000, '2024-12-15')
,(2, 1, '79903802005', 5000, '2023-09-09')
,(2, 2, '79903802005', 12000, '2028-12-11')
,(3, 1, '61071373005', 50000, '2030-06-01')
,(3, 2, '61071373005', 800, '2029-12-31')
,(4, 2, '12364784018', 5000, '2033-12-31')
,(5, 1, '53369092034', 20000, '2028-12-31')
,(6, 1, '24743372020', 100, '2025-12-31')
,(7, 2, '50114493006', 680, '2023-12-31')
,(8, 2, '18278162018', 8900, '2023-12-31')
,(9, 1, '04982742090', 5600, '2023-12-31')
,(10, 2, '15497437087', 658, '2023-12-31')
GO

--RECURSIVIDADE PARA ADICIONAR PARCELAS FUTURAS
DECLARE @IdFinanciamento INT, @ValorTotal MONEY, @DataUltimoVencimento DATE

DECLARE CURSOR_PARCELAS CURSOR FOR
SELECT Id AS IdFinanciamento, ValorTotal, DataUltimoVencimento from [Financiamento]

OPEN CURSOR_PARCELAS
	FETCH NEXT FROM CURSOR_PARCELAS INTO @IdFinanciamento, @ValorTotal, @DataUltimoVencimento
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @parcelas INT = DATEDIFF(MONTH, GETDATE(), @DataUltimoVencimento)
		DECLARE @numeroParcela INT = 1
		WHILE @numeroParcela <= @parcelas
		BEGIN
			--COLOCANDO VALORES ALEATORIOS NAS PARCELAS, PARA ALGUMAS ESTAREM PAGAS E OUTRAS NÃO
			DECLARE @DataPagamento DATE
			IF RAND() > 0.4 
				SET @DataPagamento = DATEADD(MONTH, @numeroParcela, GETDATE()) 
			ELSE
				SET @DataPagamento = NULL

			INSERT INTO [Parcela] (IdFinanciamento, NumeroParcela, ValorParcela, DataVencimento, DataPagamento) VALUES (@IdFinanciamento, @numeroParcela, @ValorTotal/@parcelas, DATEADD(MONTH, @numeroParcela - 3, GETDATE()), @DataPagamento)
			SET @numeroParcela = @numeroParcela + 1
		END
 
		FETCH NEXT FROM CURSOR_PARCELAS INTO @IdFinanciamento, @ValorTotal, @DataUltimoVencimento
	END
CLOSE CURSOR_PARCELAS
DEALLOCATE CURSOR_PARCELAS
GO

--SELECT PESSOAS COM MAIS QUE 60% PAGO
WITH CTE_PARCELAS AS (
SELECT 
	C.Id
	,C.Nome
	,COUNT(P.Id) AS PARCELAS
	,SUM(CASE WHEN P.DataPagamento IS NOT NULL THEN 1 ELSE 0 END) AS PARCELASPAGAS
FROM Cliente C
INNER JOIN Financiamento F ON F.IdCliente = C.Id
INNER JOIN Parcela P ON P.IdFinanciamento = F.Id
GROUP BY C.Id, C.Nome
) SELECT 
	Id
	,Nome
	,CAST((PARCELASPAGAS * 100.0 / PARCELAS) AS DECIMAL(5,2)) AS PorcentagemPago 
FROM CTE_PARCELAS
WHERE CAST((PARCELASPAGAS * 100.0 / PARCELAS) AS DECIMAL(5,2)) > 60

--TOP 4 COM PARCELAS EM ATRASO
SELECT TOP 4 C.Id, C.Nome FROM Cliente C
INNER JOIN Financiamento F ON F.IdCliente = C.Id
INNER JOIN Parcela P ON P.IdFinanciamento = F.Id 
WHERE DATEDIFF(DAY, DATEADD(DAY, 5, P.DataVencimento), GETDATE()) > 5
AND P.DataPagamento IS NULL
GROUP BY C.Id, C.Nome