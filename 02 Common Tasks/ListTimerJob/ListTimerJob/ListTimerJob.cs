using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace ListTimerJob
{
    class ListTimerJob : SPJobDefinition
    {
        public ListTimerJob() : base()
        {

        }

        public ListTimerJob(string jobName, SPService service, SPServer server, SPJobLockType targetType) : base(jobName, service, server, targetType)
        {

        }

        public ListTimerJob(string jobName, SPWebApplication webApplication) : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "List Timer Job";
        }

        public override void Execute(Guid contentDbId)
        {
            SPWebApplication webApplication = Parent as SPWebApplication;
            SPContentDatabase contentDb = webApplication.ContentDatabases[contentDbId];

            SPWeb web = contentDb.Sites[0].RootWeb;
            SPList list = EnsureList(web);

            SPListItem item = list.Items.Add();
            item["Title"] = string.Format("Timer Job was executed at : {0}", DateTime.Now.ToString());
            item.Update();

        }

        protected static SPList EnsureList(SPWeb web)
        {
            SPList list;
            try
            {
                list = web.Lists["TimerJobExecution"];
            }
            catch (Exception)
            {
                Guid id = web.Lists.Add("TimerJobExecution", "A list to record Timer Job Execution",
                                        SPListTemplateType.GenericList);
                list = web.Lists[id];
            }
            return list;
        }
    }
}
