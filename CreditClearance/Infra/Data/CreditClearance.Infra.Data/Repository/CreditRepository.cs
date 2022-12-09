using CreditClearance.Domain.Entities;
using CreditClearance.Domain.Enums;
using CreditClearance.Domain.Interfaces.Repository;

namespace CreditClearance.Infra.Data.Repository;

public class CreditRepository : ICreditRepository
{
    #region Properties
    public static List<Credit> Credits = 
        new()
        {
            new Credit(CreditType.Direct, "Direto", 0.02M),
            new Credit(CreditType.PayrollLoans, "Consignado", 0.01M),
            new Credit(CreditType.LegalPerson, "Pessoa Jurídica", 0.05M),
            new Credit(CreditType.PhysicalPerson, "Pessoa Física", 0.04M),
            new Credit(CreditType.RealState, "Imobiliário", 0.09M)
        };
    #endregion

    #region Methods
    public List<Credit> GetAll() => Credits;

    public Credit? GetById(CreditType id) => Credits.FirstOrDefault(x => x.Id == id);
    #endregion
}
