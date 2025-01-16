using Kaalcharakk.Dtos.OrderDtos;
using Razorpay.Api;

namespace Kaalcharakk.Helpers.RazorPayHelper
{
    public class RazorpayHelper : IRazorpayHelper
    {
        private readonly IConfiguration _configuration;
        public RazorpayHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateRazorpayOrder(long price)
        {

            Dictionary<string, object> razorInp = new Dictionary<string, object>();

            string transactionId = Guid.NewGuid().ToString();

            razorInp.Add("amount", Convert.ToDecimal(price) * 100);
            razorInp.Add("currency", "INR");
            razorInp.Add("receipt", transactionId);

            string key = _configuration["Razorpay:KeyId"];
            string secret = _configuration["Razorpay:KeySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(razorInp);
            var orderId = order["id"].ToString();
            return orderId;
        }

        public async Task<bool> VerifyRazorpayPayment(RazorpayPaymentDto payment)
        {

            try
            {
                if (payment == null || string.IsNullOrEmpty(payment.razorpay_payment_id)
                    || string.IsNullOrEmpty(payment.razorpay_orderId) || string.IsNullOrEmpty(payment.razorpay_signature))
                {
                    return false;
                }

                RazorpayClient client = new RazorpayClient(_configuration["Razorpay:KeyId"], _configuration["Razorpay:KeySecret"]);

                Dictionary<string, string> paymentVerificationDetails = new Dictionary<string, string>
                {
                    {"razorpay_payment_id", payment.razorpay_payment_id},
                    {"razorpay_orderId" ,payment.razorpay_orderId },
                    {"razorpay_signature",payment.razorpay_signature }


                };
                Utils.verifyPaymentSignature(paymentVerificationDetails);
                return true;
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }



        }
    }
}
