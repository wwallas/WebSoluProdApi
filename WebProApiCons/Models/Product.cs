namespace WebProApiCons.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        
        // Constructor
        //public Product()
        //{
        //    CreatedAt = DateTime.Now;
        //    UpdatedAt = DateTime.Now;
        //}
    }
}
