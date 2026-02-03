
using Example;
using MediatorXL.Abstractions;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("api")]
public sealed class Api(IMediator mediator) : ControllerBase
{

    [HttpGet("/send-event")]
    public async Task<IActionResult> SendEvent()
    {
        await mediator.Notify(new MediatorEvent());
        return Ok();
    }
    [HttpGet("/send-request")]
    public async Task<IActionResult> SendRequest()
    {
        var response = await mediator.Request(new MediatorRequest());
        return Ok(response);
    }
}
