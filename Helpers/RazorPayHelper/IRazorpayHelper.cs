using Kaalcharakk.Dtos.OrderDtos;

namespace Kaalcharakk.Helpers.RazorPayHelper
{
    public interface IRazorpayHelper
    {
        Task<string> CreateRazorpayOrder(long price);
        Task<bool> VerifyRazorpayPayment(RazorpayPaymentDto payment);
    }
}
