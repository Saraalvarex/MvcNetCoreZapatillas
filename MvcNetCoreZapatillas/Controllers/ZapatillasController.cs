using Microsoft.AspNetCore.Mvc;
using MvcNetCoreZapatillas.Models;
using MvcNetCoreZapatillas.Repositories;

namespace MvcNetCoreZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        public RepositoryZapatillas repo;
        public ZapatillasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Zapatilla> zapatillas = this.repo.GetZapatillas();
            return View(zapatillas);
        }
        public IActionResult Details(int idproducto)
        {
            Zapatilla zapatilla = this.repo.GetZapatilla(idproducto);
            return View(zapatilla);
        }

        public async Task<IActionResult> _Details(int? posicion, int idproducto)
        {
            posicion = 1;
            ViewBag.POSICION = posicion;
            ViewBag.IDPRODUCTO = idproducto;
            PaginarZapatillas model = await this.repo.GetImagenesAsync(posicion.Value, idproducto);
            List<ImagenZapatilla> imagenzapas = model.ImagenesZapa;
            int numRegistros = model.NumRegistros;
            ViewBag.REGISTROS = numRegistros;
            return PartialView("_Details", imagenzapas);
        }
    }
}
