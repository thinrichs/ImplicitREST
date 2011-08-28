using System;
using System.Collections.Generic;

namespace AutoREST
{
<<<<<<< HEAD
    public class EntityFuncMap<TResult> : Dictionary<Type, Func<TResult, IRESTable>> { }
=======
    public class EntityFuncMap<TInput> : Dictionary<Type, Func<TInput, IWantRESTExposure>> where TInput : class 
    { }
>>>>>>> d350cb0e0704861f24b5daeadfd2aec3a415c572
}
