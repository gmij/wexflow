using Microsoft.AspNetCore.Mvc;
using Wexflow.BlazorServer.Data;
using Wexflow.BlazorServer.ViewModel;

namespace Wexflow.BlazorServer.Controllers
{

    

    [ApiController]
    [Route("[controller]")]
    public class DashboardController : WexflowControllerBase
    {

        public DashboardController(WexflowService service) : base(service) {
        
        }


        [HttpGet("statusCount")]
        public StatusCount GetStatusCount()
        {
            var statusCount = _service.Engine.GetStatusCount();
            return new StatusCount()
            {
                Pending = statusCount.PendingCount,
                Running = statusCount.RunningCount,
                Done = statusCount.DoneCount,
                Failed = statusCount.FailedCount,
                Warning = statusCount.WarningCount,
                Disabled = statusCount.DisabledCount,
                Rejected = statusCount.RejectedCount,
                Stopped = statusCount.StoppedCount
            };
        }

        [HttpGet("entriesCountByDate")]
        public long GetEntriesCountByDate()
        {
            var context = this.HttpContext;
            string keyword = context.Request.Query["s"].ToString();
            double from = double.Parse(context.Request.Query["from"].ToString());
            double to = double.Parse(context.Request.Query["to"].ToString());

            DateTime baseDate = new DateTime(1970, 1, 1);
            DateTime fromDate = baseDate.AddMilliseconds(from);
            DateTime toDate = baseDate.AddMilliseconds(to);
            long count = _service.Engine.GetEntriesCount(keyword, fromDate, toDate);
            return count;
        }

        [HttpGet("searchEntriesByPageOrderBy")]
        public IEnumerable<Entry> SearchEntriesByPageOrderBy(EntryQueryModel query)
        {
            //var context = this.HttpContext;
            //string keyword = context.Request.Query["s"].ToString();
            //double from = double.Parse(context.Request.Query["from"].ToString());
            //double to = double.Parse(context.Request.Query["to"].ToString());
            //int page = int.Parse(context.Request.Query["page"].ToString());
            //int entriesCount = int.Parse(context.Request.Query["entriesCount"].ToString());
            //int heo = int.Parse(context.Request.Query["heo"].ToString());

            //DateTime baseDate = new DateTime(1970, 1, 1);
            //DateTime fromDate = baseDate.AddMilliseconds(from);
            //DateTime toDate = baseDate.AddMilliseconds(to);

            //var entries = _service.Engine.GetEntries(keyword, fromDate, toDate, page, entriesCount, (Wexflow.Core.Db.EntryOrderBy)heo);
            var entries = _service.Engine.GetEntries(query.Keyword, query.From, query.To, query.Page, query.PageSize, Core.Db.EntryOrderBy.StatusDateAscending);

            return entries.Select(e =>
                new Entry
                {
                    Id = e.GetDbId(),
                    WorkflowId = e.WorkflowId,
                    Name = e.Name,
                    LaunchType = (LaunchType)(int)e.LaunchType,
                    Description = e.Description,
                    Status = (Status)(int)e.Status,
                    //StatusDate = (e.StatusDate - baseDate).TotalMilliseconds
                    StatusDate = e.StatusDate.ToString(_service.Config["DateTimeFormat"])
                });
        }

        [HttpGet("entryStatusDateMin")]
        public double GetEntryStatusDateMin()
        {
            var date = _service.Engine.GetEntryStatusDateMin();
            DateTime baseDate = new DateTime(1970, 1, 1);
            return (date - baseDate).TotalMilliseconds;
        }

        [HttpGet("entryStatusDateMax")]
        public double GetEntryStatusDateMax()
        {
            var date = _service.Engine.GetEntryStatusDateMax();
            DateTime baseDate = new DateTime(1970, 1, 1);
            return (date - baseDate).TotalMilliseconds;
        }

        [HttpGet("entryLogs")]

        public string GetEntryLogs(string entryId)
        {
            return _service.Engine.GetEntryLogs(entryId);
        }
    }
}
