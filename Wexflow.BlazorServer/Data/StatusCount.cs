namespace Wexflow.BlazorServer.Data
{
    public class StatusCount
    {
        public int Pending { get; set; }
        public int Running { get; set; }
        public int Done { get; set; }
        public int Failed { get; set; }
        public int Warning { get; set; }
        public int Disabled { get; set; }
        public int Rejected { get; set; }
        public int Stopped { get; set; }
    }
}
