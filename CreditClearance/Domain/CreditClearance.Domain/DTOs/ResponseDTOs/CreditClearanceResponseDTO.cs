namespace CreditClearance.Domain.DTOs.ResponseDTOs;

public record CreditClearanceResponseDTO
{
	#region Properties
	public bool Approved { get; init; }
	public decimal TotalAmount { get; init; }
	public decimal InterestAmount { get; init; }
	#endregion
}
