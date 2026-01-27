using API.Public.DTOs.Product;
using API.Public.DTOs.Product.Payloads;
using API.Public.Filters;
using API.Public.Validators.Product;
using Domain.Data.Entities;
using Domain.Services.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Public.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(IProductService productService) : _BaseController
{
    private readonly IProductService _productService = productService ??
        throw new ArgumentNullException(nameof(productService));

    [AuthAttribute]
    [HttpPost]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateNewProduct([FromBody] ProductDTO body, CancellationToken cancellationToken = default)
    {
        var userId = Authenticated.User.Id;

        var model = await _productService.CreateAsync(ProductDTO.DtoToModel(body)!, userId, cancellationToken);
        var response = ProductDTO.ModelToDTO(model);

        return CreatedAtAction(nameof(CreateNewProduct), new { id = response.Id }, response);
    }

    [AuthAttribute]
    [HttpGet]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserProducts(CancellationToken cancellationToken = default)
    {
        var userId = Authenticated.User.Id;

        var products = await _productService.ListProductsByUserIdAsync(userId, cancellationToken);
        var response = ProductDTO.ModelToDTO(products);

        return Ok(response);
    }

    [AuthAttribute]
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProductByUserId(string userId, CancellationToken cancellationToken = default)
    {
        var products = await _productService.ListProductsByUserIdAsync(userId, cancellationToken);
        var response = ProductDTO.ModelToDTO(products);

        return Ok(response);
    }

    [AuthAttribute]
    [HttpPatch("{productId}")]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductInformation(string productId, [FromBody] UpdateProductDTO body, CancellationToken cancellationToken = default)
    {
        await new ProductUpdateValidator().ValidateAndThrowAsync(body);

        var userId = Authenticated.User.Id;

        var productModel = UpdateProductDTO.DTOToModel(body);
        if (productModel is null)
        {
            return BadRequest("INVALID_REQUEST_BODY");
        }

        var model = await _productService.UpdateProductAsync(productModel, productId, userId);

        return Ok(ProductDTO.ModelToDTO(model));
    }

    [AuthAttribute]
    [HttpDelete("{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteProduct(string productId, CancellationToken cancellationToken = default)
    {
        var userId = Authenticated.User.Id;

        await _productService.DeleteProductAsync(productId, userId, cancellationToken);

        return Ok();
    }
}
