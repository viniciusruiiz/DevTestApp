namespace CreditClearance.Domain.DTOs.ResponseDTOs;

public record CreditListResponseDTO
{
	#region Properties
	public string Name { get; init; } = "";
	public int Id { get; init; }
	#endregion
}
