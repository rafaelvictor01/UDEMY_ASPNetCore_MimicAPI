using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Utilitarios;
using System;
using System.Linq;
using System.Text.Json;

namespace MimicAPI.Controllers
{
    [Route("palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly MimicContext _banco;
        public PalavrasController(MimicContext banco)
        {
            _banco = banco;
        }

        [HttpGet]
        [Route("")]
        public IActionResult ObterTodas([FromQuery] PalavraUrlQuery queryParams)
        {
            var responsePalavras = _banco.palavras.AsQueryable();

            if (queryParams.Data.HasValue)
            {
                responsePalavras = responsePalavras.Where(registroPalavra =>
                    registroPalavra.Criacao > queryParams.Data.Value || registroPalavra.Atualizacao > queryParams.Data.Value
                );
            }

            if (queryParams.PagAtual.HasValue)
            {
                var totalRegistros = responsePalavras.Count();
                var paginacao = new Paginacao
                {
                    TotalRegistros = totalRegistros,
                    TotalPaginas = (int)Math.Ceiling((decimal)(totalRegistros / queryParams.ItensPorPag))
                };

                responsePalavras = responsePalavras
                    .Skip((queryParams.PagAtual.Value - 1) * queryParams.ItensPorPag.Value)
                    .Take(queryParams.ItensPorPag.Value);

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginacao));

                if (queryParams.PagAtual > paginacao.TotalPaginas)
                {
                    return NotFound();
                }

            }
            return new JsonResult(responsePalavras);
        }

        [HttpGet]
        [Route("{Id}")]
        public ActionResult Obter(int Id)
        {
            var objPalavra = _banco.palavras.Find(Id);
            return objPalavra != null ? new JsonResult(objPalavra) : NotFound();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Cadastrar([FromBody] Models.Palavra palavra)
        {
            _banco.palavras.Add(palavra);
            _banco.SaveChanges();
            return Created($"/palavras/{palavra.Id}", palavra);
        }

        [HttpPut]
        [Route("{Id}")]
        public ActionResult Atualizar(int Id, [FromBody] Models.Palavra palavra)
        {
            var objPalavra = _banco.palavras.AsNoTracking().FirstOrDefault(item => item.Id == Id);

            if (objPalavra == null)
            {
                return NotFound();
            }

            palavra.Id = Id;
            _banco.palavras.Update(palavra);
            _banco.SaveChanges();
            return new JsonResult(palavra);
        }

        [HttpDelete]
        [Route("{Id}")]
        public ActionResult Remover(int Id)
        {
            var objPalavra = _banco.palavras.Find(Id);

            if (objPalavra == null)
            {
                return NotFound();
            }

            objPalavra.Ativo = false; 
            _banco.palavras.Update(objPalavra);
            _banco.SaveChanges();
            return NoContent();
        }
    }
}
