namespace Wexflow.BlazorServer.Data
{
    public class HistoryEntry
    {
        public string Id { get; internal set; }
        public int WorkflowId { get; internal set; }
        public string Name { get; internal set; }
        public LaunchType LaunchType { get; internal set; }
        public string Description { get; internal set; }
        public string StatusDate { get; internal set; }
        public Status Status { get; internal set; }
    }
}