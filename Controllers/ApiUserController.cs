using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using API_Projeto_Casa.Data;
using API_Projeto_Casa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiRest.Controllers {
    [Route ("api/v1/user")]
    [ApiController]
    public class UsuarioController : ControllerBase {

        private readonly ApplicationDbContext database;

        public UsuarioController (ApplicationDbContext database) {

            this.database = database;
        }
        /// <summary>
        /// Listar usuário
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get () {
            var user = database.Usuario.Select (p => p.Id + " " + p.Email).ToList ();
            return Ok (user); //Status code = 200 && Dados 
        }

        /// <summary>
        /// Lista usuário por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet ("{id}")]
        public IActionResult Get (int id) {
            try {
                var user = database.Usuario.First (p => p.Id == id).Email;
                return Ok (user); //Status code = 200 && Dados 
            } catch (System.Exception) {
                Response.StatusCode = 404;
                return new ObjectResult ("");
            }

        }
        /// <summary>
        /// Cadastro de usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost ("Cadastro")]
        public IActionResult Cadastro ([FromBody] Usuario user) {
            try {
                database.Add (user);
                database.SaveChanges ();
                return Ok ((new { msg = "Usuário cadastrado com sucesso" }));
            } catch (Exception) {
                return BadRequest ((new { msg = "Requisição Inválida" }));
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="credenciais"></param>
        /// <returns></returns>
        [HttpPost ("Login")]
        public IActionResult Login ([FromBody] Usuario credenciais) {
            try {
                var user = database.Usuario.First (p => p.Email.Equals (credenciais.Email));
                if (user != null) {
                    //Achou usuário
                    if (user.Senha.Equals (credenciais.Senha)) {
                        //Usuário inseriu credenciais corretas => Logou!
                        string Key = "ProjetoApiEvento.com";
                        var KeySimetrica = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Key));
                        var KeyAcesso = new SigningCredentials (KeySimetrica, SecurityAlgorithms.HmacSha256Signature);

                        var TokenJwt = new JwtSecurityToken (
                            issuer: "Eventos.com", //Quem está fornecendo o Jwt
                            expires : DateTime.Now.AddHours (1),
                            audience: "Admin",
                            signingCredentials : KeyAcesso
                            /*  claims : claims */
                        );
                        Response.StatusCode = 200;
                        return new ObjectResult ("Você está Logado! \t" + new JwtSecurityTokenHandler ().WriteToken (TokenJwt));
                    } else {
                        //Não existe nenhum usuário
                        Response.StatusCode = 401;
                        return new ObjectResult ("");
                    }
                } else {
                    Response.StatusCode = 401;
                    return new ObjectResult ("");
                }
            } catch (Exception) {
                Response.StatusCode = 400;
                return new ObjectResult ("");;
            }
        }

    }
}