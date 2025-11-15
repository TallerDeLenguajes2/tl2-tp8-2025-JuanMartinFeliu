using Microsoft.AspNetCore.Mvc;
using Productos;
using productoRepository;
using SistemaVentas.Web.ViewModels;

namespace tl2_tp7_2025_JuanMartinFeliu.Controllers;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;

    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }

    public IActionResult Index()
    {
        var productos = productoRepository.ListarProductos();
        return View(productos);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        if (!ModelState.IsValid)
            return View(productoVM);

        var nuevoProducto = new Producto
        {
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };

        productoRepository.CrearProducto(nuevoProducto);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        var prod = productoRepository.ObtenerDetallesProducto(id);
        if (prod == null)
        {
            return NotFound();
        }

        return View(prod);
    }


    public IActionResult Edit(int id)
    {
        var prod = productoRepository.ObtenerDetallesProducto(id);
        if (prod == null)
            return NotFound();

        var productoVM = new ProductoViewModel
        {
            IdProducto = prod.IdProducto,
            Descripcion = prod.Descripcion,
            Precio = prod.Precio
        };
        return View(productoVM);
    }

    [HttpPost]
    public IActionResult Edit(int idBuscado, ProductoViewModel productoVM)
    {
        if (idBuscado != productoVM.IdProducto)
            return NotFound();

        if (!ModelState.IsValid)
            return View(productoVM);

        var ProductoAEditar = new Producto
        {
            IdProducto = productoVM.IdProducto,
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };

        productoRepository.ModificarProductos(ProductoAEditar, idBuscado);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var prod = productoRepository.ObtenerDetallesProducto(id);
        if (prod == null)
            return NotFound();
        return View(prod);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        productoRepository.EliminarProductos(id);
        return RedirectToAction(nameof(Index));
    }
}
