using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Infra.Interfaces;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserRepository repository;
        public UserController(ILogger<UserController> logger, IUserRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
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

                logger.LogError("Falha ao Obter Usuários", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorsDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Error = "Ocorreu Error ao Obter Usuário Tente novamente !"

                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                var errors = new List<string>();
                if(string.IsNullOrEmpty(user.Nome) || string.IsNullOrWhiteSpace(user.Nome) 
                    || user.Nome.Length < 2)
                {
                    errors.Add("Nome Inválido");
                }

                // forma sem usar o regex
                //var obrigatorios = new List<string>() {"@", "!", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
                //if(string.IsNullOrEmpty(user.Senha) || string.IsNullOrWhiteSpace(user.Senha) 
                //    || user.Senha.Length >= 4 && obrigatorios.Any(o => user.Senha.Contains(o))){}

                // utilizado o regex
                if (string.IsNullOrEmpty(user.Senha) || string.IsNullOrWhiteSpace(user.Senha)
                    || user.Senha.Length < 4 && Regex.IsMatch(user.Senha, "[a-zA-Z0-9]+", RegexOptions.IgnoreCase))
                {
                    errors.Add("Senha Inválida");
                }

                Regex regex = new Regex(@"^([\w\.\-\+\d]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (string.IsNullOrEmpty(user.Email) || string.IsNullOrWhiteSpace(user.Email) 
                    || !regex.Match( user.Email).Success)
                {
                    errors.Add("Email Inválido");
                }

                if(errors.Count > 0)
                {
                    return BadRequest(new ErrorsDto() { 
                        Status = StatusCodes.Status400BadRequest,
                        Errors = errors
                    
                    });
                }

                repository.SaveUser(user);

                return Ok(new { sucess = "Usuário Criado com sucesso !"});
            }
            catch (Exception ex)
            {

                logger.LogError("Falha ao Adicionar Usuário", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorsDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Error = "Ocorreu Error ao Adicionar Usuário Tente novamente !"

                });
            }
        }
    }
}
