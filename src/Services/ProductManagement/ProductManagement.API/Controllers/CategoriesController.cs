using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Features.GetCategories;

namespace ProductManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories()
    {
        var categories = await _mediator.Send(new GetCategoriesQuery());

        return Ok(categories);
    }
}
