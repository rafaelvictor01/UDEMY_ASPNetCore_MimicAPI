using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using System.Linq;

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
        public IActionResult ObterTodas()
        {
            return new JsonResult(_banco.palavras);
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
