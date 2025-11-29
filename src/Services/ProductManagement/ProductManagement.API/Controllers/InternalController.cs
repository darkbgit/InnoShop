using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Core.Features.DeleteProductsByUserId;
using ProductManagement.Core.Features.RestoreProductsByUserId;

namespace ProductManagement.API.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class InternalController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        
        [HttpPost("restore-products/{userId}")]
        public async Task<IActionResult> RestoreUserProducts(string userId)
        {
            var result =await _mediator.Send(new RestoreProductsByUserIdCommand { UserId = userId });
            if (result.Succeeded)
                 return Ok();

            return BadRequest(result.Errors);
        }

        [HttpPost("delete-products/{userId}")]
        public async Task<IActionResult> DeleteUserProducts(string userId)
        {
            var result = await _mediator.Send(new DeleteProductsByUserIdCommand { UserId = userId });

            if (result.Succeeded)
                 return Ok();

            return BadRequest(result.Errors);
        }
    }

