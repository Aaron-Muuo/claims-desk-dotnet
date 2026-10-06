namespace ClaimsDesk.API.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g. ClaimsOfficer, Underwriter, FinanceDirector, Admin
        public string Description { get; set; } = string.Empty;

        //Navigation Prop
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
