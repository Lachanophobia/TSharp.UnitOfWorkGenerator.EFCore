using TSharp.UnitOfWorkGenerator.EFCore.Utils;

namespace TSharp.UnitOfWorkGenerator.API.Entities
{
    [UoWGenerateRepository]
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }
}
