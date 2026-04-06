using legendarios_API.DTO;
using legendarios_API.Entity;
using legendarios_API.Repository;
using Microsoft.Extensions.Configuration;

namespace legendarios_API.Service
{
    public class AnunciosService
    {
        private readonly AnunciosRepository _repo;

        public AnunciosService(IConfiguration configuration)
        {
            _repo = new AnunciosRepository(configuration);
        }

        public AnuncioResponseListDTO GetTodos() => _repo.GetTodos();

        public AnuncioResponseListDTO GetTodosAdm() => _repo.GetTodosAdm();

        public AnuncioResponseOneDTO Criar(AnuncioDTO dto) => _repo.Criar(dto);

        public AnuncioResponseOneDTO Atualizar(AnuncioDTO dto) => _repo.Atualizar(dto);

        public AnuncioResponseOneDTO Deletar(int id) => _repo.Deletar(id);
    }
}
