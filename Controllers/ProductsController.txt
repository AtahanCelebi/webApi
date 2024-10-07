using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")] // => api/products
    public class ProductsController : ControllerBase
    {

        private readonly ProductsContext mainDb; //injection

        public ProductsController(ProductsContext context)
        {
            mainDb = context;
        }

        /* Part 1: Basic C# Skills */
        [HttpGet]
        public IActionResult HelloWorld()  
        {
            return Ok("Hello World!");
        }

        /* Part 2: Intermediate Problem Solving*/

        [HttpGet("list")]
        // use async so that the system is not blocked
        public async Task<IActionResult> GetProducts() // GET Request api/products 
        {
            var productList = await mainDb.Products.ToListAsync();
            return Ok(productList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int? id) // Get Request api/products/1
        {
            if (id == null)
            {
                return NotFound("Id is not valid");
            }

            var product = await mainDb.Products
            .Where(x => x.ProductId == id)
            .Select(x => new ResponseProductModel
            {
                ProductName = x.ProductName,
                Price = x.Price
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

            if (product is null) return NotFound("product is not found.");

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            if (entity is null) return NotFound("Product fields are not valid");

            _ = mainDb.Products.Add(entity);
            _ = await mainDb.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { Id = entity.ProductId, Name = entity.ProductName }, entity);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductModel model)
        {

            var product = await mainDb.Products
            .FirstOrDefaultAsync(x => x.ProductId == id);

            if (product is null) return NotFound("Product is not found.");

            product.ProductName = model.ProductName;
            product.Price = model.Price;

            try
            {
                _ = await mainDb.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound(); // can also be rollback for better development 
            }

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {

            var product = await mainDb.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (product is null) return NotFound("Product is not found");

            _ = mainDb.Remove(product);

            try
            {
                _ = await mainDb.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}