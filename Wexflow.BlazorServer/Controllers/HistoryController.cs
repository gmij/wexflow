﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Contracts;
using Wexflow.BlazorServer.ViewModel;
using Wexflow.BlazorServer.Data;

namespace Wexflow.BlazorServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HistoryController : WexflowControllerBase
    {
        public HistoryController(WexflowService service) : base(service)
        {
        }

        protected long _total;


        [HttpGet("searchHistoryEntriesByPageOrderBy")]
        public IEnumerable<HistoryEntry> SearchHistoryEntriesByPageOrderBy(EntryQueryModel queryModel)
        {

            var entries = _service.Engine.GetHistoryEntries(queryModel.Keyword, queryModel.From, queryModel.To, queryModel.Page,
                        queryModel.PageSize, queryModel.OrderField);

            return entries.Select(e =>
               new HistoryEntry
               {
                   Id = e.GetDbId(),
                   WorkflowId = e.WorkflowId,
                   Name = e.Name,
                   LaunchType = (LaunchType)((int)e.LaunchType),
                   Description = e.Description,
                   Status = (Status)((int)e.Status),
                   //StatusDate = (e.StatusDate - baseDate).TotalMilliseconds
                   StatusDate = e.StatusDate.ToString(_service.Config["DateTimeFormat"])
               });
        }

        [HttpGet("historyEntriesCountByDate")]

        public long GetHistoryEntriesCountByDate(EntryQueryModel queryModel)
        {
            return _service.Engine.GetHistoryEntriesCount(queryModel.Keyword, queryModel.From, queryModel.To);
        }

        [HttpGet("historyEntryStatusDateMin")]
        public DateTime GetHistoryEntryStatusDateMin()
        {
            return _service.Engine.GetHistoryEntryStatusDateMin();
        }

        [HttpGet("historyEntryStatusDateMax")]
        public DateTime GetHistoryEntryStatusDateMax()
        {
            return _service.Engine.GetHistoryEntryStatusDateMax();
        }


        [HttpGet("historyEntryLogs")]

        public string GetHistoryEntryLogs(string entryId)
        {
            return _service.Engine.GetHistoryEntryLogs(entryId);
        }
    }
}
