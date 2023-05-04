namespace MyCompanion.Models
{
    public class ViewUserJobViewModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Aadhar { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid JobId { get; set; }
        public string Jobrole { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public Guid Posterid { get; set; }
        public string Pname { get; set; }
        public string Pemail { get; set; }
        public string Pphone { get; set; }
        public string Pcity { get; set; }
        public string Ppin { get; set; }
        public Guid Accepterid { get; set; }
        public string Aname { get; set; }
        public string Aemail { get; set; }
        public string Aphone { get; set; }
        public string Acity { get; set; }
        public string Apin { get; set; }
        public int Potp { get; set; }
        public int Aotp { get; set; }
    }
}
