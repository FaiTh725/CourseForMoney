using StudentPlacement.Backend.Domain.Enums;

namespace StudentPlacement.Backend.Models.Allocation
{
    public class AddRequestToUserResponse
    {
        public int IdStudent { get; set; } 

        public StatusAllocationRequest StatusRequest { get; set; }  

        public AllocationRequestView AllocationRequest { get; set; }
    }
}
