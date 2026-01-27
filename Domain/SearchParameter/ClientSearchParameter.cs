using Domain.SearchParameter._Base;

namespace Domain.SearchParameter;

public class ClientSearchParameter : BaseSearchParameter
{
    public string Name { get; set; }

    public ClientSearchParameter(BaseSearchParameter searchParameter = null) : base(searchParameter) 
    { 
    
    }
}
