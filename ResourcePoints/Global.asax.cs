using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using ImplicitREST;

namespace ResourcePoints
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            var entityRoutePopulator = new EntityRouteRegistrar<IWantRESTExposure>
            {
                Routes = RouteTable.Routes,
                TypeMap = new VerbToTypeMapper()
            };

            entityRoutePopulator.RegisterRoutes();
        }

#if DEBUG
        /// <summary>
        /// Lists routes if compiled in debug mode, and sent magic request URL.  Helpful for debugging routes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Application_BeginRequest(object sender, EventArgs e)
        {
            var request = ((Global)sender).Request;
            var rawUrl = request.RawUrl;
            if (!rawUrl.Contains("/whats/my/routes/foo!")) return;
            RouteTable.Routes
                .ToList()
                .ForEach(WriteRouteLine);
            Response.End();
        }

        private void WriteRouteLine(RouteBase route)
        {
            Response.Write(((Route) route).Url + Environment.NewLine);
        }
#endif
    }
}
