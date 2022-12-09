using CreditClearance.Domain.Entities;
using CreditClearance.Domain.Enums;

namespace CreditClearance.Domain.Interfaces.Repository;

public interface ICreditRepository
{
	#region Methods
	List<Credit> GetAll();
	Credit? GetById(CreditType id);
	#endregion
}
