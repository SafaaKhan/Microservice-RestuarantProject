namespace Mango.Services.Email.Models
{
    public class EmailLog
    {
        public int Id { get; set; }
        public string Email { get; set; } = String.Empty;
        public string Log { get; set; }=String.Empty;
        public DateTime EmailSent { get; set; }
    }
}
