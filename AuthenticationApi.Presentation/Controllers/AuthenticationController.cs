using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using ECommmerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController(IUser userInterface):ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<Response>> Register(AppUserDTO appUserDTO)
    {
        if(!ModelState.IsValid)
            return BadRequest("Ponga bien los datos");
        var result = await userInterface.Register(appUserDTO);
        return result.Flag ? Ok(result) : BadRequest(result);

    }

    [HttpPost("login")]
    public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest("Ponga bien los datos");
        var result = await userInterface.Login(loginDTO);
        return result.Flag ? Ok(result) : BadRequest(result);

    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<Response>> GetUser(int id)
    {
        if (id <= 0) return BadRequest("Id de usuario Invalido");
        var user = await userInterface.GetUser(id);
        return user.Id > 0 ? Ok(user) : BadRequest("Error al buscar el usuario");
    }
}
