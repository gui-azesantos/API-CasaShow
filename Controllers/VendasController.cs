using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using API_Projeto_Casa.Data;
using API_Projeto_Casa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiRest.Controllers {
    [Route ("api/v1/vendas")]
    [ApiController]
    public class VendasController : ControllerBase {

        private readonly ApplicationDbContext database;

        public VendasController (ApplicationDbContext database) {

            this.database = database;
        }

        /// <summary>
        /// Listar vendas 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult GetUser () {
            var local = database.Venda.Include (e => e.CasaDeShow).ToList ().OrderByDescending (p => p.Nome);
            return Ok (local);
        }
        /// <summary>
        /// Listar venda por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet ("{id}")]
        public IActionResult GetUser (int id) {
            try {
                Venda venda = database.Venda.Include (e => e.CasaDeShow).First (p => p.Id == id);
                return Ok (venda);
            } catch (Exception) {

                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "ID Inválido" });
            }
        }
    }
}