using System;
using System.Web.Mvc;
using AutoRESTRepository;
using RESTModels;
using SampleUI.Properties;

namespace SampleUI.Controllers
{
    public class SampleController : Controller
    {
         // repositories shouldn't need context, because consumers shouldn't know / care that the repository needs a base REST url.
        // but I also don't want the generic repository class dependent on on settings / properties.
        // is DI (Dependency Injection) the solution here?
        private static readonly Repository<Sample> Repository = new Repository<Sample>(Settings.Default.BaseRestURI);

        //
        // GET: /Sample/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Sample/Details/5

        public ActionResult Details(int id)
        {
            var x = Repository.GetById(id);
            return View(x);
        }

        //
        // GET: /Sample/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Sample/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var sample = Repository.Create(new Sample
                                      {
                                          ApiKey = collection["ApiKey"]
                                      });
                var api = sample.ApiKey;
                return RedirectToAction("Edit",api);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        
        //
        // GET: /Sample/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Sample/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
