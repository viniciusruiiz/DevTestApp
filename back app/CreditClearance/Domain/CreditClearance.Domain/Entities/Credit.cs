using CreditClearance.Domain.Enums;

namespace CreditClearance.Domain.Entities;

public class Credit
{
    #region Properties
    public CreditType Id { get; private set; }
    public string Name { get; private set; }
    public decimal InterestRate { get; private set; } 
    #endregion

    #region Constructors
    protected Credit()
    {

    }

    public Credit(CreditType id, string name, decimal interestRate) : this()
    {
        Id = id;
        Name = name;
        InterestRate = interestRate;
    } 
    #endregion
}
