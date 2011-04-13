using System;
using System.Collections.Generic;

namespace AutoREST
{
    public class EntityToEntityFuncMap : Dictionary<Type, Func<IRESTable, IRESTable>> { }
}
