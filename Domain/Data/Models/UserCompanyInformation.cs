using Domain.Enumerators.User.Company;

namespace Domain.Data.Models;

public class UserCompanyInformation
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyDocument { get; set; } = string.Empty;
    public MonthlyRevenue MonthlyRevenue { get; set; }
    public string CompanyDomain { get; set; } = string.Empty;
    public BusinessSegment BusinessSegment { get; set; }
    public string? BusinessDescription { get; set; }
}
