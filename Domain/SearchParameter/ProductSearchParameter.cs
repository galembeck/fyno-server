using Domain.SearchParameter._Base;

namespace Domain.SearchParameter;

public class ProductSearchParameter : BaseSearchParameter
{
    public string Name { get; set; }

    public ProductSearchParameter(BaseSearchParameter searchParameter = null) : base(searchParameter)
    {

    }
}
