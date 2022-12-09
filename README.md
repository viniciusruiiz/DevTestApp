# Aplicação para processo seletivo - liberação de crédito

1 (Aplicação Angular + .Net Core) - pastas "back app" e "front app"

2 (Queries e Comandos em SQL Server) - pasta "queries"

3 (Diagrama conceitual de microsserviços) - pasta "microsservice diagram"

### Instalação (requisitos e passo a passo)

Necessário para rodar o backend

```
Visual Studio 2022
ou
.Net Core CLI 6.0
ou
Docker (dockerfile configurado para .net core 6)
```

Necessário para rodar o frontend
```
Node.Js (utilizado a versão 18, mas versões mais antigas devem funcionar)
Angular CLI
```

Comandos para rodar o backend

.Net CLI:
```
   dotnet build
   dotnet run
```

Visual studio 2022:
```
   Escolher tipo de inicialização ("Docker" ou localmente (opção "CreditClearance.api"))
   Executar aplicação (botão de iniciar no menu superior, ou F5)
```

Comandos para rodar o frontend
```
npm i
ng s -o
```