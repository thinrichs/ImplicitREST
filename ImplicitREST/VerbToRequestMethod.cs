using System.Collections.Generic;

namespace ImplicitREST
{
    public static class VerbToRequestMethod
    {
        public static readonly Dictionary<RESTVerb, string> Mapping = new Dictionary<RESTVerb, string>
                                                        {
                                                            { RESTVerb.Create,  HttpVerb.Post},
                                                            { RESTVerb.Update,  HttpVerb.Put},
                                                            { RESTVerb.Retrieve,HttpVerb.Get},
                                                            { RESTVerb.Delete,  HttpVerb.Delete}
                                                        };
    }
}
