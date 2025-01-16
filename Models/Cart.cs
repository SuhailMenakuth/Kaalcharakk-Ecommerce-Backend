namespace Kaalcharakk.Models
{
    public class Cart
    {

        public int CartId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }


    }
}
