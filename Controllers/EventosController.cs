using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using API_Projeto_Casa.Models;
using API_Projeto_Casa.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.Controllers
{
    [Route ("api/v1/eventos")]
    [ApiController]
    public class EventosController : ControllerBase {

        private readonly ApplicationDbContext database;

        public EventosController (ApplicationDbContext database) {

            this.database = database;
        }

        /// <summary>
        /// Listar Eventos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get () {
            var eventos = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).Where (p => p.CasaDeShow.Status == true).ToList ();
            return Ok (eventos); //Status code = 200 && Dados 
        }
        /// <summary>
        /// Listar eventos por capacidade ascendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("capacidade/asc")]
        public IActionResult GetAsc () {
            var local = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).ToList ().OrderBy (p => p.Capacidade);
            return Ok (local);
            //Status code = 200 && Dados 

        }
        /// <summary>
        /// Listar eventos por capacidade descendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("capacidade/desc")]
        public IActionResult GetDesc () {
            var local = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).ToList ().OrderByDescending (p => p.Capacidade);
            return Ok (local);
            //Status code = 200 && Dados 

        }
        /// <summary>
        /// Listar eventos por data ascendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("data/asc")]
        public IActionResult GetDataAsc () {
            var local = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).ToList ().OrderBy (p => p.Data);
            return Ok (local);
            //Status code = 200 && Dados 

        }
        /// <summary>
        /// Listar eventos por data descendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("data/desc")]
        public IActionResult GetDataDesc () {
            var local = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).ToList ().OrderByDescending (p => p.Data);
            return Ok (local);
            //Status code = 200 && Dados 
        }
        /// <summary>
        /// Listar eventos por preço ascendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("preco/asc")]
        public IActionResult GetPrecoAsc () {
            var local = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).ToList ().OrderBy (p => p.Preco);
            return Ok (local);
            //Status code = 200 && Dados 

        }

        /// <summary>
        /// Listar eventos por preço descendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("preco/desc")]
        public IActionResult GetPrecoDesc () {
            var local = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).ToList ().OrderByDescending (p => p.Preco);
            return Ok (local);
            //Status code = 200 && Dados 
        }
        /// <summary>
        /// Listar eventos por nome ascendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("nome/asc")]
        public IActionResult GetNomeAsc () {
            var local = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).ToList ().OrderBy (p => p.Nome);
            return Ok (local);
        }
        /// <summary>
        /// Listar eventos por nome descendente
        /// </summary>
        /// <returns></returns>
        [HttpGet ("nome/desc")]
        public IActionResult GetNomeDesc () {
            var local = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (p => p.Status == true).ToList ().OrderByDescending (p => p.Nome);
            return Ok (local);
        }
        /// <summary>
        /// Listar evento por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet ("{id}")]
        public IActionResult Get (int id) {
            try {
                Evento evento = database.Evento.Include (e => e.CasaDeShow).Where (p => p.CasaDeShow.Status == true).Where (e => e.Status == true).First (p => p.Id == id);
                return Ok (evento);
            } catch (Exception) {

                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "ID inválido" });
            }

        }

        /// <summary>
        /// Criar eventos (Necessita Autorização)
        /// </summary>
        /// <param name="eTemp"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult Post ([FromBody] EventoTemp eTemp) {
            //Validação
            if (eTemp != null) {
                try {
                    if (eTemp.CasaDeShowID == 0) {
                        return new ObjectResult (new { msg = "O local do evento não existe!" });
                    }
                    Evento e = new Evento ();
                    e.Nome = eTemp.Nome;
                    e.Capacidade = eTemp.Capacidade;
                    e.Data = eTemp.Data;
                    e.Preco = eTemp.Preco;
                    e.CasaDeShow = database.Local.First (c => c.Id == eTemp.CasaDeShowID);
                    e.Estilo = eTemp.Estilo.ToString ();
                    e.Imagem = eTemp.Imagem;
                    e.Status = eTemp.Status;
                    database.Evento.Add (e);
                    database.SaveChanges ();

                    //Set do Status Code
                    Response.StatusCode = 201;
                    return new ObjectResult (new { msg = "Evento criado com sucesso!" });
                } catch (Exception) {
                    Response.StatusCode = 400;
                    return BadRequest (new { msg = "Verifique se o ID da casa de show está correto" });
                }
            }
            return BadRequest (new { msg = "Verifique se as informações do evento estão corretas" });
        }
        /// <summary>
        /// Atualizar evento (Necessita Autorização)
        /// </summary>
        /// <param name="ptemp"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize]
        public IActionResult Patch ([FromBody] EventoPatch ptemp) {

            if (ptemp.Id >= 0) {

                try {
                    var evento = database.Evento.First (p => p.Id == ptemp.Id);
                    //Editar 
                    if (ptemp.Nome != null) {
                        if (ptemp.Nome.Length > 2 && ptemp.Nome.Length < 150) {
                            evento.Nome = ptemp.Nome;
                        } else {
                            evento.Nome = evento.Nome;
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Nome inválido" });
                        }
                    }
                    if (ptemp.Capacidade != 0) {
                        if (ptemp.Capacidade > 10 && ptemp.Capacidade < 1000000) {
                            evento.Capacidade = ptemp.Capacidade;
                        } else {
                            evento.Capacidade = evento.Capacidade;
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Capaidade inválida" });
                        }
                    }
                    if (ptemp.Preco != 0) {
                        if (ptemp.Preco > 10 || ptemp.Preco < 1000000) {
                            evento.Preco = ptemp.Preco;
                        } else {
                            evento.Preco = evento.Preco;
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Preço inválida" });
                        }
                    }

                    if (ptemp.CasaDeShowID != 0) {
                        if (database.Local.First (p => p.Id == ptemp.CasaDeShowID) != null) {
                            evento.CasaDeShow = database.Local.First (p => p.Id == ptemp.CasaDeShowID);
                        } else {
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Local inválido" });
                        }
                    }
                    if (ptemp.Estilo != 0) {
                        if (evento.Estilo == ptemp.Estilo.ToString () && ptemp.Estilo > 0 || ptemp.Estilo < 8) {
                            evento.Estilo = ptemp.Estilo.ToString ();
                        } else {
                            evento.Estilo = evento.Estilo;
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Estilo de inválido" });
                        }
                    }

                    if (ptemp.Imagem != null) {
                        if (ptemp.Imagem.Length > 10 && ptemp.Imagem.Length < 1024) {
                            evento.Imagem = ptemp.Imagem;
                        } else {
                            evento.Imagem = evento.Imagem;
                            Response.StatusCode = 404;
                            return new ObjectResult (new { msg = "Imagem: Url inválida" });
                        }
                    }

                    database.SaveChanges ();
                    return Ok (new { msg = "Evento atualizado!" });

                } catch (System.Exception) {
                    Response.StatusCode = 400;
                    return new ObjectResult (new { msg = "Requisição inválida" });
                }

            } else {
                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "ID inválido" });
            }

        }
        /// <summary>
        /// Deletar evento (Necessita Autorização)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete ("{id}")]
        [Authorize]
        public IActionResult Delete (int id) {
            try {
                var evento = database.Evento.First (p => p.Id == id);
                evento.Status = false;
                database.SaveChanges ();
                Response.StatusCode = 201;
                return new ObjectResult (new { msg = "Evento excluido!" });
            } catch (Exception) {
                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "ID inválido" });
            }
        }

        public class EventoTemp {
            public int Id { get; set; }

            [Required (ErrorMessage = "O Nome é obrigatório.")]
            [StringLength (150, ErrorMessage = "Nome grande demais.")]
            [MinLength (2, ErrorMessage = "Nome curto demais.")]
            public string Nome { get; set; }

            [Range (10, 1000000, ErrorMessage = "Capacidade inválida.")]
            public int Capacidade { get; set; }

            [Required (ErrorMessage = "A data é obrigatória.")]
            public System.DateTime Data { get; set; }

            [Range (1, 1000000, ErrorMessage = "Preço inválido.")]
            public double Preco { get; set; }

            [Required]
            public int CasaDeShowID { get; set; }

            [Range (0, 8, ErrorMessage = "Estilo inválido.")]
            public int Estilo { get; set; }

            [Required (ErrorMessage = "A URL da imagem é obrigatória.")]
            [MinLength (2, ErrorMessage = "Url curta demais.")]
            [StringLength (1024, ErrorMessage = "Url grande demais.")]
            public string Imagem { get; set; }

            public bool Status { get; set; }
        }
        public class EventoPatch {
            public int Id { get; set; }
            public string Nome { get; set; }
            public int Capacidade { get; set; }

            public System.DateTime Data { get; set; }

            public double Preco { get; set; }

            public int CasaDeShowID { get; set; }

            public int Estilo { get; set; }

            public string Imagem { get; set; }

            public bool Status { get; set; }
        }
    }
}