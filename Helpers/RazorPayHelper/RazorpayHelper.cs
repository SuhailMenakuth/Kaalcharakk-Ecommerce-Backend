using Razorpay.Api;
using Microsoft.Extensions.Configuration;

namespace Kaalcharakk.Helpers.RazorPayHelper
{
    public class RazorpayHelper
    {
        private readonly string _razorpayKeyId;
        private readonly string _razorpayKeySecret;

        public RazorpayHelper(IConfiguration configuration)
        {
            _razorpayKeyId = configuration["Razorpay:KeyId"];
            _razorpayKeySecret = configuration["Razorpay:KeySecret"];
        }

        // Method to create a Razorpay order
        public async Task<Order> CreateOrderAsync(decimal amount, string currency = "INR")
        {
            var client = new RazorpayClient(_razorpayKeyId, _razorpayKeySecret);
            var orderOptions = new Dictionary<string, object>
            {
                { "amount", amount * 100 }, // Amount is in paise (cents)
                { "currency", currency },
                { "receipt", Guid.NewGuid().ToString() }
            };

            var order = client.Order.Create(orderOptions);
            return order;
        }

        // Method to verify Razorpay payment signature
        public bool VerifyPaymentSignature(string orderId, string paymentId, string signature)
        {
            var data = orderId + "|" + paymentId;
            // Use Razorpay's method directly for signature verification
            var paymentVerificationDetails = new Dictionary<string, string>
            {
                { "razorpay_payment_id", paymentId },
                { "razorpay_orderId", orderId },
                { "razorpay_signature", signature }
            };

            try
            {
                // Verify payment signature using Razorpay SDK's built-in method
                Utils.verifyPaymentSignature(paymentVerificationDetails);
                return true;
            }
            catch (Exception ex)
            {
                // Handle signature verification failure
                throw new Exception("Payment signature verification failed: " + ex.Message);
            }
        }

        // Method to capture payment after successful payment
        public async Task CapturePaymentAsync(string paymentId, decimal amount)
        {
            var client = new RazorpayClient(_razorpayKeyId, _razorpayKeySecret);
            var payment = client.Payment.Fetch(paymentId); // No need to await here
            var captureOptions = new Dictionary<string, object>
            {
                { "amount", amount * 100 } // Amount in paise
            };

            payment.Capture(captureOptions); // No await here either
        }
    }
}