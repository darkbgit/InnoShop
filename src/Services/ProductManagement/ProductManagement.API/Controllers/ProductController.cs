using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Features.CreateProduct;
using ProductManagement.Core.Features.DeleteProduct;
using ProductManagement.Core.Features.GetPaginatedProduct;
using ProductManagement.Core.Features.GetProductById;
using ProductManagement.Core.Features.UpdateProduct;
using Shared.Core.Models;

namespace ProductManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ProductController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(long id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _mediator.Send(query);

        return Ok(product);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetPaginatedProducts([FromQuery] GetPaginatedProductsQuery query)
    {
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductForCreateDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentUserId = Guid.NewGuid(); //TODO: Get from auth context

        var command = _mapper.Map<CreateProductCommand>(product);
        command.CreatedBy = currentUserId;

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result }, null);
    }

    [HttpPut]
    public async Task<IActionResult> Update(long id, ProductForUpdateDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentUserId = Guid.NewGuid(); //TODO: Get from auth context

        var command = _mapper.Map<UpdateProductCommand>(product);
        command.Id = id;
        command.UpdatedBy = currentUserId;

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var command = new DeleteProductCommand { Id = id };
        await _mediator.Send(command);

        return NoContent();
    }
}
