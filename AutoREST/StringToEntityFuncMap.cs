using System;
using System.Collections.Generic;

namespace AutoREST
{
    public class StringToEntityFuncMap : Dictionary<Type, Func<String, IRESTable>> { }
}
