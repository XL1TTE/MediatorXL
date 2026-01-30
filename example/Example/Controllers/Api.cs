
using Example;
using MediatorXL.Abstractions;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("api")]
public sealed class Api(IMediator mediator) : ControllerBase
{

    [HttpGet("/send-goodby")]
    public async Task<IActionResult> SendGoodBy()
    {
        var response = await mediator.Request(new GoodNightMediatorRequest { Name = nameof(Api) });
        return Ok(response);
    }
    [HttpGet("/send-hello")]
    public async Task<IActionResult> SendHello()
    {
        await mediator.Notify(new HelloMediatorRequest { Name = nameof(Api) });
        return Ok("Sended.");
    }
}
