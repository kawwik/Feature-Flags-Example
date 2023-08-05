using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace WebApplication2.Controllers;

public record LoginDto(string Login, string Password);

public record LoginResponseDto(string Token);

public record Account(string Login, string Password);

[ApiController]
[Route("[controller]")]
public class FeatureFlagExampleController : ControllerBase
{
    private readonly List<Account> _accounts = new()
    {
        new Account(Login: "login", Password: "pass")
    };

    private readonly IFeatureManager _featureManager;
    
    public FeatureFlagExampleController(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var account = _accounts.SingleOrDefault(x => x.Login == loginDto.Login);

        if (account is null)
            return Problem("Не найден пользователь с таким логином", statusCode: (int)HttpStatusCode.NotFound);

        if (await _featureManager.IsEnabledAsync(FeatureFlags.CheckPassword))
        {
            if (loginDto.Password != account.Password)
                return Problem("Неверный пароль", statusCode: (int)HttpStatusCode.Unauthorized);
        }

        return Ok(new LoginResponseDto(Guid.NewGuid().ToString()));
    }
}