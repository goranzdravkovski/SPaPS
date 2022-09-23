namespace SPaPS.Models.CustomModels
{
    public class vm_Service
    {
        public long ServiceId { get; set; }
        public string? Description { get; set; }
        public List<int> ActivityIds { get; set; } = new List<int>();
        public string? Activities { get; set; }
    }
}
