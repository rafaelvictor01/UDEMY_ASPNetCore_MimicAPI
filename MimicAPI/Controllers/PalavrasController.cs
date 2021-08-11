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
            return new JsonResult(_banco.palavras.Find(Id));
        }

        [HttpPost]
        [Route("")]
        public ActionResult Cadastrar(Models.Palavra palavra)
        {
            _banco.palavras.Add(palavra);
            return Ok();
        }

        [HttpPut]
        [Route("")]
        public ActionResult Atualizar(Models.Palavra palavra)
        {
            _banco.palavras.Update(palavra);
            return Ok();
        }

        [HttpDelete]
        [Route("{Id}")]
        public ActionResult Remover(int Id)
        {
            _banco.palavras.Remove(_banco.palavras.Find(Id));
            return Ok();
        }
    }
}
