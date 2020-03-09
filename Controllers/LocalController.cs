using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using API_Projeto_Casa.Data;
using API_Projeto_Casa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Projeto_Casa.Controllers.API {

    [Route ("api/v1/local")]
    [ApiController]

    public class LocalController : ControllerBase {
        private readonly ApplicationDbContext database;

        public LocalController (ApplicationDbContext database) {

            this.database = database;
        }
        /// <summary>
        /// Listar Local 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get () {
            var local = database.Local.Where (p => p.Status == true).ToList ();
            return Ok (local);
            //Status code = 200 && Dados 
        }

        /// <summary>
        /// Listar local por nome ascendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("asc")]
        public IActionResult GetAsc () {
            var local = database.Local.Where (p => p.Status == true).ToList ().OrderBy (p => p.Nome);
            return Ok (local);
            //Status code = 200 && Dados 

        }
        /// <summary>
        /// Listar Local por nome decrescente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("desc")]
        public IActionResult GetDesc () {
            var local = database.Local.Where (p => p.Status == true).ToList ().OrderByDescending (p => p.Nome);
            return Ok (local);
            //Status code = 200 && Dados 

        }
        /// <summary>
        /// Listar local por ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet ("{id}")]
        public IActionResult GetId (int id) {
            try {
                var local = database.Local.Where (l => l.Status == true).First (l => l.Id == id);
                return Ok (local);
            } catch (Exception) {

                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "ID inválido" });
            }

        }
        /// <summary>
        /// Listar local por Nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet ("nome/" + "{nome}")]
        public IActionResult GetNome (string nome) {
            try {
                var local = database.Local.Where (l => l.Status == true).First (l => l.Nome.ToLower () == nome.ToLower ());
                return Ok (local);
            } catch (Exception) {

                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "Nome Inválido" });
            }

        }

        /// <summary>
        /// Criar local (Necessita Autorização)
        /// </summary>
        /// <param name="lTemp"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult Post ([FromBody] LocalTemp lTemp) {
            try {
                Local l = new Local ();
                l.Nome = lTemp.Nome;
                l.Endereco = lTemp.Endereco;
                l.LinkEndereco = lTemp.LinkEndereco;
                l.Status = true;
                database.Local.Add (l);
                database.SaveChanges ();

                //Set do Status Code
                Response.StatusCode = 201;
                return new ObjectResult (new { msg = "Local criado com sucesso!" });
            } catch {
                return BadRequest (new { msg = "ID Inválido" });
            }
        }

        /// <summary>
        /// Atualizar local (Necessita Autorização)
        /// </summary>
        /// <param name="ltemp"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch]
        public IActionResult Patch ([FromBody] LocalPatch ltemp) {

            if (ltemp.Id >= 0) {
                try {
                    var local = database.Local.First (p => p.Id == ltemp.Id);
                    //Editar 
                    if (ltemp.Nome != null) {
                        if (ltemp.Nome.Length > 0 && ltemp.Nome.Length < 100) {
                            local.Nome = ltemp.Nome;
                        } else {
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Nome  inválido" });
                        }
                    }

                    if (ltemp.Endereco != null) {
                        if (ltemp.Endereco.Length > 0 && ltemp.Endereco.Length < 100) {
                            local.Endereco = ltemp.Endereco;
                        } else {
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Endereço  inválido" });
                        }
                    }
                    if (ltemp.LinkEndereco != null) {
                        if (ltemp.LinkEndereco.Length > 0 && ltemp.LinkEndereco.Length < 1024) {
                            local.LinkEndereco = ltemp.LinkEndereco;
                        } else {
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Url  inválida" });
                        }
                    }
                    database.SaveChanges ();
                    return Ok (new { msg = "Local Atualilzado!" });

                } catch (System.Exception) {
                    Response.StatusCode = 404;
                    return new ObjectResult (new { msg = "Requisição inválida!" });
                }

            } else {
                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "ID inválido" });
            }
        }

        /// <summary>
        /// Deletar local (Necessita Autorização)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>   
        [Authorize]
        [HttpDelete ("{id}")]
        public IActionResult Delete (int id) {
            try {
                var local = database.Local.First (p => p.Id == id);
                local.Status = false;
                database.SaveChanges ();
                Response.StatusCode = 201;
                return new ObjectResult (new { msg = "Local excluido!" });
            } catch (Exception) {

                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "Id inválido" });
            }
        }
        public class LocalTemp {

            [Required]
            public int Id { get; set; }

            [Required (ErrorMessage = "O Nome é obrigatório.")]
            public string Nome { get; set; }

            [Required (ErrorMessage = "O Endereço é obrigatório.")]
            public string Endereco { get; set; }

            public string LinkEndereco { get; set; }

            public bool Status { get; set; }
        }
        public class LocalPatch {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Endereco { get; set; }
            public string LinkEndereco { get; set; }

        }
    }

}