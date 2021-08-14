using Microsoft.AspNetCore.Mvc;
using MimicAPI.Models;
using MimicAPI.Repositories.Interfaces;
using MimicAPI.Utilitarios;
using System.Text.Json;

namespace MimicAPI.Controllers
{
    [Route("palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly IPalavraRepository _palavraRepository;
        public PalavrasController(IPalavraRepository palavraRepository)
        {
            _palavraRepository = palavraRepository;
        }

        [HttpGet]
        [Route("")]
        public IActionResult ObterTodas([FromQuery] PalavraUrlQuery queryParams)
        {
            var paginationList = _palavraRepository.ObterTodas(queryParams);
            if (queryParams.PagAtual > paginationList.Paginacao.TotalPaginas)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationList.Paginacao));
            return new JsonResult(paginationList);
        }

        [HttpGet]
        [Route("{Id}")]
        public ActionResult Obter(int Id)
        {
            var objPalavra = _palavraRepository.Obter(Id);
            return objPalavra != null ? new JsonResult(objPalavra) : NotFound();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Cadastrar([FromBody] Palavra palavra)
        {
            _palavraRepository.Cadastrar(palavra);
            return Created($"/palavras/{palavra.Id}", palavra);
        }

        [HttpPut]
        [Route("{Id}")]
        public ActionResult Atualizar(int Id, [FromBody] Palavra palavra)
        {
            var objPalavra = _palavraRepository.Obter(Id);

            if (objPalavra == null)
            {
                return NotFound();
            }

            palavra.Id = Id;
            _palavraRepository.Atualizar(palavra);
            return Ok();
        }

        [HttpDelete]
        [Route("{Id}")]
        public ActionResult Remover(int Id)
        {
            var objPalavra = _palavraRepository.Obter(Id);

            if (objPalavra == null)
            {
                return NotFound();
            }
            _palavraRepository.Remover(Id);
            return NoContent();
        }
    }
}
