using Microsoft.AspNetCore.Mvc;
using MimicAPI.Database;

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
            var palavra = _banco.palavras.Find(Id);
            return palavra != null ? new JsonResult(palavra) : NotFound();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Cadastrar([FromBody] Models.Palavra palavra)
        {
            _banco.palavras.Add(palavra);
            _banco.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("")]
        public ActionResult Atualizar([FromBody] Models.Palavra palavra)
        {
            _banco.palavras.Update(palavra);
            _banco.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{Id}")]
        public ActionResult Remover(int Id)
        {
            var palavra = _banco.palavras.Find(Id);
            palavra.Ativo = false; 
            _banco.palavras.Update(palavra);
            _banco.SaveChanges();
            return Ok();
        }
    }
}
