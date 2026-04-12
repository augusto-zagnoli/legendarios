using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Interfaces;
using Microsoft.Extensions.Configuration;

namespace legendarios_API.Service
{
    public class AnunciosServiceV2 : IAnunciosService
    {
        private readonly IAnunciosRepository _repo;

        public AnunciosServiceV2(IAnunciosRepository repo)
        {
            _repo = repo;
        }

        public AnuncioResponseListDTO GetTodos() => _repo.GetTodos();
        public AnuncioResponseListDTO GetTodosAdm() => _repo.GetTodosAdm();
        public AnuncioResponseOneDTO Criar(AnuncioDTO dto) => _repo.Criar(dto);
        public AnuncioResponseOneDTO Atualizar(AnuncioDTO dto) => _repo.Atualizar(dto);
        public AnuncioResponseOneDTO Deletar(int id) => _repo.Deletar(id);
    }
}
