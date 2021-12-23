using Catalog.API.Entitites;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductReposirory _productReposirory;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductReposirory productReposirory, ILogger<CatalogController> logger)
        {
            _productReposirory = productReposirory;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            IEnumerable<Product> products = await _productReposirory.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProductByIdAsync")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductByIdAsync(string id)
        {
            var product = await _productReposirory.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductsByCategoryAsync")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryAsync(string category)
        {
            var products = await _productReposirory.GetProductsByCategoryAsync(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProductAsync([FromBody] Product product)
        {
            await _productReposirory.CreateProductAsync(product);

            return CreatedAtRoute("GetProductByIdAsync", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProductAsync([FromBody] Product product)
        {
            return Ok(await _productReposirory.UpdateProductAsync(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProductAsync")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductAsync(string id)
        {
            return Ok(await _productReposirory.DeleteProductAsync(id));
        }
    }
}
