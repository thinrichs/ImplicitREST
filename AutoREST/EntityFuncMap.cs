using System;
using System.Collections.Generic;

namespace AutoREST
{
    public class EntityFuncMap<TInput> : Dictionary<Type, Func<TInput, IWantRESTExposure>> where TInput : class 
    { }
}