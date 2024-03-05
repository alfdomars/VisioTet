namespace VisioTet.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string CustomerType { get; set; }
        public int PointReward { get; set; }
        public decimal TotalBelanja { get; set; }
        public decimal Discounts { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
