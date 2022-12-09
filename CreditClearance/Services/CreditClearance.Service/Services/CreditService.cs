using CreditClearance.Domain.DTOs.InputDTOs;
using CreditClearance.Domain.DTOs.ResponseDTOs;
using CreditClearance.Domain.Enums;
using CreditClearance.Domain.Interfaces.Repository;
using CreditClearance.Domain.Interfaces.Service;

namespace CreditClearance.Service.Services;

public class CreditService : ICreditService
{
    #region Properties
    private readonly ICreditRepository _creditRepository;

    private const int DAYS_IN_MONTH = 30;
    private const int MAX_INSTALLMENTS = 72;
    private const int MIN_INSTALLMENTS = 5;
    private const int MAX_CREDIT = 1_000_000;
    private const int MIN_CREDIT_LEGAL_PERSON = 15_000;
    private const int MAX_DAY_RANGE_FIRST_PAYMENT = 40;
    private const int MIN_DAY_RANGE_FIRST_PAYMENT = 15;
    #endregion

    #region Constructors
    public CreditService(ICreditRepository creditRepository)
    {
        _creditRepository = creditRepository;
    }
    #endregion

    #region Methods
    public CreditClearanceResponseDTO CreditClearanceAnalysis(CreditClearanceInputDTO request)
    {
        var credit = _creditRepository.GetById(request.CreditType);
        if (credit is null) throw new Exception("Invalid Credit Type");

        var approved = ValidateCreditClearanceAnalysisInput(request);

        var dailyTaxeRate = credit.InterestRate / DAYS_IN_MONTH;
        var dailyTaxeAmount = request.Amount * dailyTaxeRate;
        var daysUntilFirstPayment = (request.FirstDueDate - GetBrazilianTime()).Days;
        var totalDays = (DAYS_IN_MONTH * request.NumberOfInstallments) + daysUntilFirstPayment;

        var interestAmount = totalDays * dailyTaxeAmount;
        var totalAmount = interestAmount + request.Amount;

        return new CreditClearanceResponseDTO()
        {
            Approved = approved,
            InterestAmount = interestAmount,
            TotalAmount = totalAmount
        };
    }

    public IEnumerable<CreditListResponseDTO> GetAllCreditTypes()
    {
        var credits = _creditRepository.GetAll();
        return credits.Select(credit => 
            new CreditListResponseDTO { Name = credit.Name, Id = (int)credit.Id }
        );
    }

    private static bool ValidateCreditClearanceAnalysisInput(CreditClearanceInputDTO request)
    {
        var daysDiffBetweenFirstDueDate = (request.FirstDueDate - GetBrazilianTime()).Days;

        if (request.Amount > MAX_CREDIT) return false;
        if (request.NumberOfInstallments < MIN_INSTALLMENTS || request.NumberOfInstallments > MAX_INSTALLMENTS) return false;
        if (request.CreditType == CreditType.LegalPerson && request.Amount < MIN_CREDIT_LEGAL_PERSON) return false;
        if (daysDiffBetweenFirstDueDate < MIN_DAY_RANGE_FIRST_PAYMENT || daysDiffBetweenFirstDueDate > MAX_DAY_RANGE_FIRST_PAYMENT) return false;
        return true;
    }

    private static DateTime GetBrazilianTime()
    {
        return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).Date;
    } 
    #endregion
}
