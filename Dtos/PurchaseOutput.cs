namespace A2.Dtos
{
    public class PurchaseOutput
    {
        public PurchaseOutput(string userName, int productID)
        {
            UserName = userName;
            ProductID = productID;
        }

        public string UserName { get; set; }
        public int ProductID { get; set; }
    }
}