using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models.RazorPay
{
    public class PaymentRequest
    {
        public int? mem_id { get; set; }
        public int? pro_id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
