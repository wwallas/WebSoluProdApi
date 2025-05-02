using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebProdApi.Data;
using WebProdApi.Models;

namespace WebProdApi.Controllers
{
    [Route("api/[controller]")]

    public class ProductsController : Controller
    {
        private readonly AdoNetContext _context;

        public ProductsController(AdoNetContext context)
        { 
            _context = context;
        }


        //Get : api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts() 
        {
            var dataTable = _context.ExecuteStoredProcedure("sp_GetAllProducts");
            var Products = new List<Product>();
            foreach (DataRow row in dataTable.Rows) 
            {
                Products.Add(new Product
                {
                    Id = Convert.ToInt32(row["ProId"]),
                    Name = row["ProNam"].ToString(),
                    Price = Convert.ToDecimal(row["ProPri"]),
                    Description = row["ProDes"].ToString()
                });
            }

            return Ok(Products);
        }

        // GET: api/Products/5
        //[HttpGet("{id}")]
        //public ActionResult<Product> GetProduct(int id)
        //{
        //    var product = Products.FirstOrDefault(p => p.Id == id);

        //    if (product==null) 
        //    {
        //        return NotFound();
        //    }
        //    return product;
        //}

        // POST: api/Products
        [HttpPost]
        public ActionResult Create([FromBody]Product product) 
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Name",product.Name),
                new SqlParameter("@Price",product.Price),
                new SqlParameter("@Description",product.Description)
            };

            var newId = _context.ExecuteScalarStoredProcedure("sp_InsertProduct", parameters);

            return Ok(new { Id = newId, Message = "Product creted succefully!" });
        }

        // PUT: api/Products/5
        //[HttpPut("{id}")]
        //public IActionResult PutProduct(int Id,Product product) 
        //{ 
        //    if(Id != product.Id) { return BadRequest(); }

        //    var existingProduct= products.FirstOrDefault(p => p.Id == Id);
        //    if (existingProduct == null) { return NotFound(); }

        //    existingProduct.Name = product.Name;
        //    existingProduct.Price = product.Price;
        //    existingProduct.Description = product.Description;

        //    return NoContent();
        //}


        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
