namespace InstallPackageTest;

using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using UseValidator;

[ApiController]
[Route("/test")]
public class TestController
{
    [HttpGet("query")]
    [UseQueryValidator(Validator = typeof(FakeValidator))]
    [UsedImplicitly]
    public IActionResult GetWithQuery([FromQuery] FakeBaseRequest query)
    {
        return new JsonResult(query);
    }

    [HttpPost("body")]
    [UseBodyValidator(Validator = typeof(FakeValidator))]
    [UsedImplicitly]
    public IActionResult PostWithBody([FromBody] FakeBaseRequest body)
    {
        return new JsonResult(body);
    }
}
