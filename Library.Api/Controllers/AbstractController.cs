using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers;

public abstract class AbstractController : ControllerBase
{
    protected long GetContextUserId()
    {
        return long.Parse(User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
    }
    protected string GetContextUserIdentificationNumber()
    {
        return User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
    }
}