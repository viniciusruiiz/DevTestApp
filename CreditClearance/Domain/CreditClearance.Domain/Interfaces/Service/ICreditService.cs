using CreditClearance.Domain.DTOs.InputDTOs;
using CreditClearance.Domain.DTOs.ResponseDTOs;

namespace CreditClearance.Domain.Interfaces.Service;

public interface ICreditService
{
    #region Methods
    CreditClearanceResponseDTO CreditClearanceAnalysis(CreditClearanceInputDTO request);
    IEnumerable<CreditListResponseDTO> GetAllCreditTypes();
    #endregion
}
