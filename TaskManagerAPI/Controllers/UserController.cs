using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                var userMockado = new User() 
                {
                    Id = 1,
                    Nome = "user Teste",
                    Email = "teste@admin",
                    Senha = "gms120605"
                };
                return Ok(userMockado);
            }
            catch (Exception ex)
            {

                _logger.LogError("Falha ao Obter Usuários", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorsDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Error = "Ocorreu Error ao Obter Usuário Tente novamente !"

                });
            }
        }

        [HttpGet]
        public IActionResult Teste()
        { 
            return Ok();
        }


        [HttpPost] 
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                return Ok(user);
            }
            catch (Exception ex)
            {

                _logger.LogError("Falha ao Adicionar Usuário", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorsDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Error = "Ocorreu Error ao Adicionar Usuário Tente novamente !"

                });
            }
        }
    }
}
