using Microsoft.AspNetCore.Mvc;
using MimicAPI.Database;

namespace MimicAPI.Controllers
{
    public class PalavrasController : ControllerBase
    {
        private readonly MimicContext _banco;
        public PalavrasController(MimicContext banco)
        {
            _banco = banco;
        }
        public IActionResult ObterTodas()
        {
            return new JsonResult(_banco.palavras);
        }

        public ActionResult Obter(int Id)
        {
            return new JsonResult(_banco.palavras.Find(Id));
        }

        public ActionResult Cadastrar(Models.Palavra palavra)
        {
            _banco.palavras.Add(palavra);
            return Ok();
        }

        public ActionResult Atualizar(int Id, Models.Palavra palavra)
        {
            _banco.palavras.Update(palavra);
            return Ok();
        }

        public ActionResult Remover(int Id)
        {
            _banco.palavras.Remove(_banco.palavras.Find(Id));
            return Ok();
        }
    }
}
