using Microsoft.AspNetCore.Mvc;
using Productos;
using productoRepository;
using SistemaVentas.Web.ViewModels;
using System.Threading.RateLimiting;

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
    public IActionResult Create(ProductoViewModel productoVM)
    {
        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }

        var nuevoProducto = new Producto
        {
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };

        productoRepository.CrearProducto(nuevoProducto);
        return RedirectToAction(nameof(Index));

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
    public IActionResult Edit(int idBuscado, ProductoViewModel productoVM)
    {
        if (idBuscado != productoVM.IdProducto)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }

        // Mapeo Manual de VM a Modelo de Dominio
        var ProductoAEditar = new Producto
        {
            IdProducto = productoVM.IdProducto,
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };

        // 3. Llamada al Repositorio

        productoRepository.ModificarProductos(ProductoAEditar, idBuscado);
        return RedirectToAction(nameof(Index));

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
