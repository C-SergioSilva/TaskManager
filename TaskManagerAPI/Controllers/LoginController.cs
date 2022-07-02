﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TaskManagerAPI.Dtos;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")] // Quando se usa o prefixo api é para separa quais são as rotas da api e as rotas do site quando as paginas
    // são tambem produzida no lado do servidor tambem sem prefixo ex: /login estou acssando uma pagina quando uso ex: /api estou acessando um recurso .
    public class LoginController : ControllerBase
    {
        // todas as api usará aquivo de logs, que são arquivos que anota tudo que acontece durante a execução do projeto no ambiente de produção.
        private readonly ILogger<LoginController> _logger;
        
        //criação do construtor
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        
        [HttpPost]
        public IActionResult LoginIn([FromBody] LoginRequestDto request) // a idéia de um Model ou entity e ser uma cópia do que estar no banco de dados
        {
            try
            {
                // Validando Erro de responsabilidade do usuário 
                if (request == null || request.Login == null || request.Password == null)
                {
                    return BadRequest(new ErrorsDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Error = "Parametros de Entrada Inválido"
                    });

                }
                // Resposta positiva a requisição Realizada status 200
                return Ok("Usuário autenticado com Sucesso");

            }
            // Erro de Responsabiliidade do Desenvolvimento
            catch(Exception exception)
            {
                _logger.LogError($"Ocorreu um erro ao afetuar Login {exception.Message}", exception);

                //através da herança do controller base permite o acesso ao Status code
                // retorna o codigo do erro em questão onde a classe de error está passando o codigo e a mensagem
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorsDto() { 
                    Status = StatusCodes.Status500InternalServerError,
                    Error = "Ocorreu um erro ao efetuar o Login, Tente Novamente!"
                });
            }
        }
    }
}
 