using Domain.Utils;

namespace Domain.Enumerators.User.Company;

public enum BusinessSegment
{
    [EnumDescription("DROPSHIPPING_BR")]
    DROPSHIPPING_BR = 1,

    [EnumDescription("DROPSHIPPING_GLOBAL")]
    DROPSHIPPING_GLOBAL = 2,

    [EnumDescription("E_COMMERCE")]
    E_COMMERCE = 3,

    [EnumDescription("INFOPRODUCTS")]
    INFOPRODUCTS = 4,

    [EnumDescription("NUTRACEUTICALS")]
    NUTRACEUTICALS = 5,
}
