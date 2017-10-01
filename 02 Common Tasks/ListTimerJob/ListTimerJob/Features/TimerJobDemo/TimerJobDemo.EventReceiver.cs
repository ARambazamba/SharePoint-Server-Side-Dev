using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace ListTimerJob.Features.Feature1
{

    [Guid("f84debe1-28cb-4bc4-86b9-0426a3110cab")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        const string List_JOB_NAME = "List Timer Job";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {                      
            DeleteExistingJob(properties);

            // install the job
            ListTimerJob listLoggerJob = new ListTimerJob(List_JOB_NAME, (properties.Feature.Parent as SPSite).WebApplication);
            SPMinuteSchedule schedule = new SPMinuteSchedule { BeginSecond = 0, EndSecond = 59, Interval = 5 };
            listLoggerJob.Schedule = schedule;
            listLoggerJob.Update();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            DeleteExistingJob(properties);
        }

        private static void DeleteExistingJob(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == List_JOB_NAME)
                {
                    job.Delete();
                }
            }
        }
    }
}
