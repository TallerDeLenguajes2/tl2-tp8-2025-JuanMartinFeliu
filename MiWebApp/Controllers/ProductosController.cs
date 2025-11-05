using Microsoft.AspNetCore.Mvc;
using Productos;
using productoRepository;

namespace tl2_tp7_2025_JuanMartinFeliu.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductoController : Controller
{
    private ProductoRepository productoRepository;
    public ProductoController()
    {
        productoRepository = new ProductoRepository();
    }

    // Listo productos desde el index
    [HttpGet]
    public IActionResult Index()
    {
        List<Producto> productos = productoRepository.ListarProductos();
        return View(productos);

    } 

    // Crear GET
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Crear POST
    [HttpPost]
    public IActionResult Create(Producto nuevo)
    {
        if (!ModelState.IsValid)
            return View(nuevo);

        productoRepository.CrearProducto(nuevo);
        return RedirectToAction("Index");
    }

    //  EDITAR (GET)
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var prod = productoRepository.ObtenerDetallesProducto(id);
        if (prod == null)
            return NotFound();
        return View(prod);
    }

    //  EDITAR (POST)
    [HttpPost]
    public IActionResult Edit(Producto prod)
    {
        productoRepository.ModificarProductos(prod, prod.IdProducto);
        return RedirectToAction("Index");
    }

    //  ELIMINAR (GET)
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var prod = productoRepository.ObtenerDetallesProducto(id);
        if (prod == null)
            return NotFound();
        return View(prod);
    }

    // ELIMINAR (POST)
    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        productoRepository.EliminarProductos(id);
        return RedirectToAction("Index");
    }
}
