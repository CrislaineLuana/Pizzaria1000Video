using Microsoft.AspNetCore.Mvc;
using Pizzaria1000Video.Dto;
using Pizzaria1000Video.Models;
using Pizzaria1000Video.Services.Pizza;

namespace Pizzaria1000Video.Controllers
{
    public class PizzaController : Controller
    {
        private readonly IPizzaInterface _pizzaInterface;

        public PizzaController(IPizzaInterface pizzaInterface)
        {
            _pizzaInterface = pizzaInterface;
        }

        public async Task<IActionResult> Index()
        {
            var pizzas = await _pizzaInterface.GetPizzas();
            return View(pizzas);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }
        
        public async Task<IActionResult> Detalhes (int id)
        {
            var pizza = await _pizzaInterface.GetPizzaPorId(id);
            return View(pizza);
        }


        public async Task<IActionResult> Editar(int id)
        {
            var pizza = await _pizzaInterface.GetPizzaPorId(id);

            return View(pizza);
        }

        public async Task<IActionResult> Remover(int id)
        {
            var pizza = await _pizzaInterface.RemoverPizza(id);
            return RedirectToAction("Index", "Pizza");
        }


        [HttpPost]
        public async Task<IActionResult> Cadastrar(PizzaCriacaoDto pizzaCriacaoDto, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                var pizza = await _pizzaInterface.CriarPizza(pizzaCriacaoDto, foto);
                return RedirectToAction("Index", "Pizza");
            }
            else
            {
                return View(pizzaCriacaoDto);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Editar(PizzaModel pizzaModel, IFormFile? foto)
        {
            if (ModelState.IsValid)
            {
                var pizza = await _pizzaInterface.EditarPizza(pizzaModel, foto);
                return RedirectToAction("Index", "Pizza");
            }
            else
            {
                return View(pizzaModel);
            }
        }



    }
}
