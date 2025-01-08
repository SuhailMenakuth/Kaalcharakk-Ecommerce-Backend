namespace Kaalcharakk.Dtos.OrderDtos
{
    public class RazorpayPaymentDto
    {
        public string? razorpay_payment_id { get; set; }
        public string? razorpay_orderId { get; set; }
        public string? razorpay_signature { get; set; }
    }
}
