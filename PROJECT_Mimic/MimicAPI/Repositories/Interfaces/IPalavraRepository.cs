using MimicAPI.Models;
using MimicAPI.Utilitarios;

namespace MimicAPI.Repositories.Interfaces
{
    public interface IPalavraRepository
    {
        PaginationList<Palavra> ObterTodas(PalavraUrlQuery queryParams);
        Palavra Obter(int Id);
        void Cadastrar(Palavra palavra);
        void Atualizar(Palavra palavra);
        void Remover(int Id);
    }
}
