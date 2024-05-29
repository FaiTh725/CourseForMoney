namespace StudentPlacement.Backend.Models.Doc
{
    public class ReportStudentAllocation
    {
        public string FullName { get; set; }

        public double AverageScore {  get; set; }

        public AllocationData AllocationData { get; set; }
    }

    public class AllocationData 
    { 
        public string? NameOrganixation {  get; set; }

        public string? Contacts { get; set; }

        public string? AdressRequest {  get; set; }
    }

}
