namespace GiveAID.ModelView
{
    public class MV_Donate_Pro_Topic
    {
        public int donation_id { get; set; }
        public int money { get; set; }
        public DateTime donation_date { get; set; }
        public int member_id { get; set; }
        public int program_id { get; set; }
        public DateTime created_at { get; set; }
        public string? o_program_name { get; set; }
        public string? topic_name { get; set; }
        public int target { get; set; }
        public int current { get; set; }
    }
}
