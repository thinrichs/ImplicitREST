using System;
using System.Collections.Generic;

namespace ImplicitREST
{
    public class EntityFuncMap<TInput> : Dictionary<Type, Func<TInput, IWantRESTExposure>> where TInput : class 
    { }
}