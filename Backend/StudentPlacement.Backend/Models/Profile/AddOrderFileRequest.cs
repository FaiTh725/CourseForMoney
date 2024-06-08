namespace StudentPlacement.Backend.Models.Profile
{
    public class AddOrderFileRequest
    {
        public int IdAllocationRequest {  get; set; }

        public IFormFile? OrderRequest { get; set; }
    }
}
