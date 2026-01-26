using Domain.SearchParameter._Base;

namespace Domain.SearchParameter;

public class UserSearchParameter : BaseSearchParameter
{
    public string Name { get; set; }

    public UserSearchParameter(BaseSearchParameter searchParameter = null) : base(searchParameter)
    {

    }
}
