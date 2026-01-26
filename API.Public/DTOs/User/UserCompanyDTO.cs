using API.Public.DTOs._Base;
using Domain.Enumerators.User.Company;

namespace API.Public.DTOs.User;

public class UserCompanyDTO
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyDocument { get; set; } = string.Empty;
    public MonthlyRevenue MonthlyRevenue { get; set; }
    public string CompanyDomain { get; set; } = string.Empty;
    public BusinessSegment BusinessSegment { get; set; }
    public string? BusinessDescription { get; set; }
}
