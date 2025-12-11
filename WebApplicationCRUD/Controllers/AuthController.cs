using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

using WebApplicationCRUD.Models;

using WebApplicationCRUD;
using WebApplicationCRUD.Data;
using LoginRequest = WebApplicationCRUD.Models.LoginRequest;


namespace WebApplicationCRUD.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwt;
    private readonly MyDbContext _db; // Add this line, or use your actual DbContext type

    public AuthController(IJwtService jwt, MyDbContext db) // Use your actual DbContext type here
    {
        _jwt = jwt;
        _db = db;
    }

    //[HttpPost("login")]
    //public IActionResult Login([FromBody] LoginRequest request)
    //{
    //    if (request is null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
    //        return BadRequest("Invalid request");

    //    // Demo credential - replace with real user validation
    //    if (request.Username == "admin" && request.Password == "password")
    //    {
    //        var token = _jwt.GenerateToken(request.Username);
    //        return Ok(new { token });
    //    }

    //    return Unauthorized();
    //}
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Invalid request");

        var user = await _db.Users.FirstOrDefaultAsync(x =>
            x.Username == request.Username);

        if (user == null)
            return Unauthorized("Invalid username");

        var hashedPassword = new PasswordService().HashPassword(request.Password!);

        if (string.IsNullOrWhiteSpace(user.PasswordHash) || hashedPassword != user.PasswordHash.Trim())
            return Unauthorized("Invalid password");

        var token = _jwt.GenerateToken(user.Username ?? string.Empty, user.Role ?? string.Empty);

        return Ok(new { token });
    }

    //[HttpPost("Register")]
    //public async Task<IActionResult> Register([FromBody] LoginRequest request)
    //{
    //    var existing = await  // Replace with actual user existence check
    //    return Ok("Register endpoint - to be implemented");
    //}
}
