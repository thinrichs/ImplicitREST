using System;
using System.Collections.Generic;

namespace AutoREST
{
    public class EntityFuncMap<TResult> : Dictionary<Type, Func<TResult, IRESTable>> { }
}
