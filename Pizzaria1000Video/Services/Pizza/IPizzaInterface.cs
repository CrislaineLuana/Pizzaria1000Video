using Pizzaria1000Video.Dto;
using Pizzaria1000Video.Models;

namespace Pizzaria1000Video.Services.Pizza
{
    public interface IPizzaInterface
    {
        Task<PizzaModel> CriarPizza(PizzaCriacaoDto pizzaCriacaoDto, IFormFile foto);
        Task<List<PizzaModel>> GetPizzas();
        Task<PizzaModel> GetPizzaPorId(int id);
        Task<PizzaModel> EditarPizza(PizzaModel pizza, IFormFile? foto);
        Task<PizzaModel> RemoverPizza (int id);
        Task<List<PizzaModel>> GetPizzasFiltro(string? pesquisar);
    }
}
