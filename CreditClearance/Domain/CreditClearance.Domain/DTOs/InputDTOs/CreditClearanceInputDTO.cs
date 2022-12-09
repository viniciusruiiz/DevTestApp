using CreditClearance.Domain.Enums;

namespace CreditClearance.Domain.DTOs.InputDTOs;

public record CreditClearanceInputDTO
{
    #region Properties
    public decimal Amount { get; init; }
    public CreditType CreditType { get; init; }
    public int NumberOfInstallments { get; init; }
    public DateTime FirstDueDate { get; init; } 
    #endregion
}