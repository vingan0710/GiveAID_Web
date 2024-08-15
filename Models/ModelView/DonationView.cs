namespace GiveAID.Models.ModelView
{
    public class DonationView
    {
        public int id { get; set; }
        public int money { get; set; }
        public DateTime donation_date { get; set; }
        public string? mem_name { get; set; }
        public string? program_name { get; set; }
    }
}
