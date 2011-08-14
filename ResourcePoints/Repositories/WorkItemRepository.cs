using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Client;
using Models;
using TFS.Properties;
using TFSClient = Microsoft.TeamFoundation.WorkItemTracking.Client;
using WorkItem = Models.WorkItem;


namespace TFS.Repositories
{
    public class WorkItemRepository : IDisposable
    {
        private static readonly Uri CollectionUri = new Uri(Settings.Default.TFSServerUri);
        private static WorkItemStore _workItemStore;
        private static TfsTeamProjectCollection _projectCollection;

        public static void ConnectToTFS(ICredentials credential = null)
        {
            try
            {
                _projectCollection = credential == null 
                    ? new TfsTeamProjectCollection(CollectionUri) 
                    : new TfsTeamProjectCollection(CollectionUri, credential);

                _workItemStore = GetWorkItemStore();
            }
            catch (TeamFoundationServerUnauthorizedException ex)
            {
                throw ex;
                // handle access denied
            }
            catch (TeamFoundationServiceUnavailableException ex)
            {
                throw ex;
                // handle service unavailable
            }
            catch (WebException ex)
            {
                throw ex;
                // handle other web exception
            }

        }

        private static WorkItemStore GetWorkItemStore()
        {
            _projectCollection.EnsureAuthenticated();
            return _projectCollection.GetService<WorkItemStore>();
        }

        public WorkItem Create(WorkItem workItem)
        {
            var tfsItem = AsTFSWorkItem(workItem);
            var result = ValidateAndSaveWorkItem(tfsItem);
            return result;
        }

        private WorkItem ValidateAndSaveWorkItem(TFSClient.WorkItem workItem)
        {
            var invalidFields = workItem.Validate();

            if (invalidFields.Count > 0)
            {
                throw ValidationResultException(invalidFields);
            }

            // save item to TFS, populates it's Id
            workItem.Save();
            // get the Uri to the project collection to use
            var result = GetWorkItem(workItem.Id);
            return result;
        }

        private static TFSClient.WorkItem AsTFSWorkItem(WorkItem workItem)
        {
            var existingProject = _workItemStore.Projects[workItem.Project];
            if (existingProject == null)
                throw new ArgumentOutOfRangeException(String.Format("Can not find project {0} in {1} ", workItem.Project, CollectionUri));
            var workItemTypes = existingProject.WorkItemTypes;
            var result = new TFSClient.WorkItem(workItemTypes[workItem.Type])
                            { 
                                Description = workItem.Description,
                                Title = workItem.Title
                            };
            result.Fields[WorkItemFieldNames.StepsToReproduce].Value = workItem.StepsToReproduce;
            result.Fields[WorkItemFieldNames.AssignedTo].Value = workItem.AssignedTo;
            return result;
        }

        private static InvalidOperationException ValidationResultException(ArrayList invalidFields)
        {
            var sb = new StringBuilder();
            foreach (var field in invalidFields.OfType<Field>())
            {
                sb.AppendLine(field.Name + " has an invalid value (" + field.Value + ")");
            }
            return new InvalidOperationException(sb.ToString());
        }

        public WorkItem GetWorkItem(int id)
        {
            var tfsItem = _workItemStore.GetWorkItem(id);
            var workItem = AsWorkItem(tfsItem);
            return workItem;
        }

        private static WorkItem AsWorkItem(TFSClient.WorkItem tfsItem)
        {
            return new WorkItem
                       {
                           Id = tfsItem.Id,
                           Type = tfsItem.Type.Name,
                           StepsToReproduce = tfsItem.Fields[WorkItemFieldNames.StepsToReproduce].Value.ToString(),
                           Project = tfsItem.Project.Name,
                           Description = tfsItem.Description,
                           AssignedTo = tfsItem.Fields[WorkItemFieldNames.AssignedTo].Value.ToString(),
                           Title = tfsItem.Title
                       };
        }

        public WorkItem Update(WorkItem workItem)
        {
            var tfsItem = _workItemStore.GetWorkItem(workItem.Id);
            UpdateTFSItemFromWorkItem(ref tfsItem, workItem);
            return workItem;
        }

        private static void UpdateTFSItemFromWorkItem(ref TFSClient.WorkItem tfsItem, WorkItem workItem)
        {
            tfsItem.Title = workItem.Title;
            tfsItem.Description = workItem.Description;
            UpdateTFSItemField(ref tfsItem, WorkItemFieldNames.StepsToReproduce, workItem.StepsToReproduce);
            UpdateTFSItemField(ref tfsItem, WorkItemFieldNames.AssignedTo, workItem.AssignedTo);
            tfsItem.Save();
        }

        private static void UpdateTFSItemField(ref TFSClient.WorkItem tfsItem, string fieldName, string value)
        {
            var field = tfsItem.Fields[fieldName];
            if (field == null || String.IsNullOrEmpty(value)) return;
            field.Value = value;
        }

        public void Dispose()
        {
            if (_projectCollection == null) return;

            _projectCollection.Dispose();
        }
    }
}