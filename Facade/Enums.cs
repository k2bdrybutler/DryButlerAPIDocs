using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Facade
{
    public enum FilterOperators : int
    {
        Equal = 1,
        NotEqual = 2,
        LessThan = 3,
        LessThanOrEqual = 4,
        GreaterThan = 5,
        GreaterThanOrEqual = 6,
        Like = 7,
        NotLike = 8,
        In = 9,
        NotIn = 10,
        Between = 11,
        NotBetween = 12,
        IsNull = 13,
        IsNotNull = 14
    }

    public enum OrderTypes : int
    {
        Ascending = 1,
        Descending = 2
    }

    /******* Project Enums *******/

    public enum PropertyTypes : int
    {
        String = 1,
        Int32 = 2,
        Datetime = 3,
        Boolean = 4,
        JSONObject = 5

    }

    /******* Project Enums *******/

}
