namespace Kaalcharakk.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public int AddressId { get; set; }
        public decimal Totalamount { get; set; }
        public string TransactionId { get; set; }
    }
}
