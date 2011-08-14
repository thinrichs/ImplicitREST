namespace TFS.Repositories
{
    public class RepositoryFactory
    {
        private static WorkItemRepository _workItemRepository;

        public static WorkItemRepository WorkItemRepository
        {
            get
            {
                return _workItemRepository ?? (_workItemRepository = InstantiateWorkItemRepository());
            }
        }

        private static WorkItemRepository InstantiateWorkItemRepository()
        {
            // to connect to pauls TFS server.  Is there a better way to secure the password?
            //var credential =  new NetworkCredential("Publishers", "nosoup4u");
            var credentials = Global.Credentials;
            if (credentials == null)
            {
                WorkItemRepository.ConnectToTFS();
            }
            else
            {
                WorkItemRepository.ConnectToTFS(credentials);
            }

            return new WorkItemRepository();
        }
    }
}