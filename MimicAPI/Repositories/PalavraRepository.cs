using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Models;
using MimicAPI.Repositories.Interfaces;
using MimicAPI.Utilitarios;
using System;
using System.Linq;

namespace MimicAPI.Repositories
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly MimicContext _banco;
        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }

        public PaginationList<Palavra> ObterTodas(PalavraUrlQuery queryParams)
        {
            var lista = new PaginationList<Palavra>();
            var responsePalavras = _banco.palavras.AsNoTracking().AsQueryable();

            if (queryParams.Data.HasValue)
            {
                responsePalavras = responsePalavras.Where(registroPalavra =>
                    registroPalavra.Criacao > queryParams.Data.Value || registroPalavra.Atualizacao > queryParams.Data.Value
                );
            }

            if (queryParams.PagAtual.HasValue)
            {
                var totalRegistros = responsePalavras.Count();
                lista.Paginacao = new Paginacao
                {
                    TotalRegistros = totalRegistros,
                    TotalPaginas = (int)Math.Ceiling((decimal)(totalRegistros / queryParams.ItensPorPag))
                };

                responsePalavras = responsePalavras
                    .Skip((queryParams.PagAtual.Value - 1) * queryParams.ItensPorPag.Value)
                    .Take(queryParams.ItensPorPag.Value);
            }

            lista.AddRange(responsePalavras.ToList());
            return lista;
        }

        public Palavra Obter(int Id)
        {
            return _banco.palavras.AsNoTracking().FirstOrDefault(item => item.Id == Id);
        }

        public void Cadastrar(Palavra palavra)
        {
            _banco.palavras.Add(palavra);
            _banco.SaveChanges();
        }

        public void Atualizar(Palavra palavra)
        {
            _banco.palavras.Update(palavra);
            _banco.SaveChanges();
        }

        public void Remover(int Id)
        {
            var objPalavra = Obter(Id);
            objPalavra.Ativo = false;
            _banco.palavras.Update(objPalavra);
            _banco.SaveChanges();
        }
    }
}
