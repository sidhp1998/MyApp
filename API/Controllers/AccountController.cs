using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using API.Data;
using API.Entities;
using API.DTO;
using System.Numerics;
using Arch.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using API.Interface;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    public readonly DataContext _dataContext;
    public readonly ITokenService _tokenService;
    public AccountController(DataContext dataContext, ITokenService tokenService)
    {
        _dataContext = dataContext;
        _tokenService = tokenService;
    }

    /* [HttpGet("test")]
    public ActionResult Test()
    {
        return Ok("Test is working");
    } */

    /* [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(string username, string password)
    {
        using var hmac = new HMACSHA512(); //when the class is out of scope, it will dispose itself

        var user = new AppUser
        {
            UserName=username,
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            PasswordSalt=hmac.Key
        };
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
        return Ok(user);
    } */
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
    {
        
        if(UserExists(registerDTO.Username)) 
        {
            return BadRequest("Username is taken");
        }
        
        using var hmac = new HMACSHA512(); //when the class is out of scope, it will dispose itself
        var user = new AppUser
        {
            UserName=registerDTO.Username.ToLower(),
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            PasswordSalt=hmac.Key
        };
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
        return Ok(
            new UserDTO
            { 
                Username=user.UserName,
                Token=_tokenService.CreateToken(user)
            }
        );
    }
    
    private bool UserExists(string username)
    {
        
        var count = _dataContext.Users.Where(u => u.UserName.ToLower() == username.ToLower()).Count();
        if(count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    

    [HttpPost]
    [Route("login")]
    public ActionResult<AppUser> Login(LoginDTO loginDTO)
    {
        var user = _dataContext.Users.FirstOrDefault(u => u.UserName==loginDTO.Username.ToLower());
        if(user == null)
        {
            return Unauthorized("Invalid username");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
        for(int i=0; i<computedHash.Length; i++)
        {
            if(computedHash[i]!=user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }
        return Ok(
            new UserDTO
            { 
                Username=user.UserName,
                Token=_tokenService.CreateToken(user)
            }
        );
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public ActionResult Delete(int id)
    {
        var user = _dataContext.Users.FirstOrDefault(u => u.Id==id);
        if(user != null)
        {
            _dataContext.Users.Remove(user);

        }
        _dataContext.SaveChanges();
        return Ok("User deleted");
    }
}
