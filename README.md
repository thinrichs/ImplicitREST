ImplicitREST -- A .NET WCF based REST engine
====================================
## LICENSE

ImplicitREST is published under the [WTFPL](http://en.wikipedia.org/wiki/WTFPL)
```
           DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
                   Version 2, December 2004

Copyright (C) 2013 Profound Creative Studio, LLC <mail@weareprofound.com>
Everyone is permitted to copy and distribute verbatim or modified
copies of this license document, and changing it is allowed as long
as the name is changed.

           DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
  TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION

 0. You just DO WHAT THE FUCK YOU WANT TO.
```

## DESCRIPTION

ImplicitREST is a .NET 4 WCF based REST engine that supports both JSON and XML.

ImplicitREST has a few requirements:

* .NET 4.0
* AspNetCompatibilityRequirementsMode has to be set to allowed.  
	* This can be done in configuration with this node:
	
			<serviceHostingEnvironment aspNetCompatibilityEnabled="true"/>
			
* Automatic format selection and automatic help require the following configuration:
	
			<webHttpEndpoint>
				<standardEndpoint name="" helpEnabled="true" automaticFormatSelectionEnabled="true"/>
			</webHttpEndpoint>
	
ImplicitREST is built on the shoulders of giants, such as:

* [Glenn Block for increasing WCF innovation by an order of magnitiude](http://blogs.msdn.com/b/gblock/)
* [Steve Michelotti for this blog entry that showed the way](http://geekswithblogs.net/michelotti/archive/2010/08/21/restful-wcf-services-with-no-svc-file-and-no-config.aspx)
* [StackOverflow in General](http://www.stackoverflow.com)
* [Andras Zoltan](http://stackoverflow.com/users/157701/andras-zoltan) specifically for his answer to [this stackoverflow question](http://stackoverflow.com/questions/3021613/how-to-pre-load-all-deployed-assemblies-for-an-appdomain)

There is a sample project called HowToConsumeImplicitREST that shows... wait for it... How to Consume ImplicitREST.

## SAMPLE IMPLEMENTATION

The sample implementation is the simplest thing that will work.  It does not follow the best practices.  Best Practices are laid out below this section.
This and subsequent sections will assume the base URL for the sample is http://localhost/HowToConsumeImplicitREST
To test the sample implementation perform the following:

		POST to http://localhost/HowToConsumeImplicitREST/RESTable/Create with an empty JSON payload like {} 
		
This should result a response with a JSON body similar to:

		{"APIkey":"1K4q7f9HAEO3cXs6idYYgg=="}
		
The RESTable type is exposed for access according to this interface:

		[ServiceContract]
		public interface IService<T> where T : IRESTable
		{
			[OperationContract]
			T Create(T payload);

			[OperationContract]
			T Read(string id);

			[OperationContract]
			T Update(T payload);

			[OperationContract]
			void Delete(string id);
		}
		
WCF gives you a help page for each type also.  The URL looks like this http://localhost/HowToConsumeImplicitREST/Restable/help
		
		Operations at http://localhost/HowToConsumeImplicitREST/Restable
		This page describes the service operations at this endpoint.

		Uri		Method	Description
		{id}	GET		Service at http://localhost/HowToConsumeImplicitREST/Restable/{ID}
				DELETE	Service at http://localhost/HowToConsumeImplicitREST/Restable/{ID}
		Create	POST	Service at http://localhost/HowToConsumeImplicitREST/Restable/Create
		Update	PUT		Service at http://localhost/HowToConsumeImplicitREST/Restable/Update
				
## IMPLEMENTATION STEPS

It is as simple as this:

* Add a reference to the ImplicitREST project
* Implement IRESTable on the items that you want to expose via REST, such as below.  Tag the class with DataContract, data payload elements with DataMember

		[DataContract(Namespace = "")]
		public class Restable : IRESTable
		{       
			[DataMember]
			public string APIkey { get; set; }       
		}

* Implement IRepositoryTypeMap.  The class that implements IRepositoryTypeMap 
  is responsible for mapping REST requests to the code that will process the request
	* Note that IRepositoryTypeMap may soon be renamed to ITypeToUnitOfWorkMap, 
	  as UnitOfWork is more generic than Repository.  
* Add the following code to your Global application class (similar but slightly different for MVC projects):

		protected void Application_Start()
        {
            var entityRoutePopulator = new EntityRouteRegistrar
            {
                Routes = RouteTable.Routes,
                TypeMap = new RepositoryTypeMapper() // implements IRepositoryTypeMap
            };

            entityRoutePopulator.RegisterRoutes();
        }
* Add the following to your web.config:
		
		<system.serviceModel>
			<behaviors>
				<serviceBehaviors>
					<behavior>
						<serviceMetadata httpGetEnabled="true"/>
						<!-- you may want includeExceptionDetailInFaults="false" depending on environment -->
						<serviceDebug includeExceptionDetailInFaults="true"/> 
					</behavior>
				</serviceBehaviors>
			</behaviors>
			<serviceHostingEnvironment aspNetCompatibilityEnabled="true"/>
			<standardEndpoints>
				<webHttpEndpoint>
					<standardEndpoint name="" helpEnabled="true" automaticFormatSelectionEnabled="true"/>
				</webHttpEndpoint>
			</standardEndpoints>
		</system.serviceModel>
		
## BEST PRACTICES	

1. Hide private information.  The objects exposed via your REST API should not be analogous 
   to your internal object model, persistence model, or any other internal model.
	* This is why the Restable class in the sample website lives in Model.Domain.  
	  Persistence models would live in Model.Persistence.  
	  Do what makes sense for your project, but make sure you only expose information that you want to expose.
	* Having no dependency between your REST API and the rest of your system allows you to change your system but leave your REST API intact.
2. Layer Appropriately.  It is appropriate for the sample to have all components in one project, 
   as the sample is meant to be as simple as possible.  If, for example, your project has Models 
   that implement IRESTable in a seperate dll, you could update your REST API by just copying the new dll into the bin and resetting IIS
   
## TODO / PENDING

* Decide if it is worth updating routes on each request.  Microsoft says this is not a best practice.  I can't find the link right this minute.
* Due to current WCF implementation, resource identifiers have to strings, ala, IService<T>.Read(string id)
	* The [latest WCF contrib bits](http://wcf.codeplex.com/releases/view/64449) now support primitive types instead of just strings.  Should I add a dependency and support this?
