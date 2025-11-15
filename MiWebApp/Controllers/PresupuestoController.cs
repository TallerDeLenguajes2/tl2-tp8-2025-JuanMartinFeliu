using Microsoft.AspNetCore.Mvc;
using Presupuestos;
using presupuestosRepository;
using productoRepository;
using SistemaVentas.Web.ViewModels; //Necesario para poder llegar a los ViewModels
using Microsoft.AspNetCore.Mvc.Rendering;
using Productos; // Necesario para SelectList
using PresupuestosDetalle;


namespace tl2_tp7_2025_JuanMartinFeliu.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestoRepository presupuestoRepository;
    // Necesito el repositorio productos para el dropdown
    private ProductoRepository productoRepository;

    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestoRepository();
        productoRepository = new ProductoRepository();
    }

    // 📘 LISTAR (READ - INDEX)
    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuesto> presupuestos = presupuestoRepository.ListarPresupuestos();
        return View(presupuestos);
    }

    // 📘 DETALLE (READ - DETAILS)
    [HttpGet]
    public IActionResult Details(int id)
    {
        var presupuesto = presupuestoRepository.ObtenerDetallesPresupuesto(id);
        if (presupuesto == null)
            return NotFound();

        return View(presupuesto);
    }

    // 🟢 CREAR (GET)
    [HttpGet]
    public IActionResult Create(int id)
    {
        // 1. Obtener los productos para el SelectList
        List<Producto> productos = productoRepository.ListarProductos();
        // 2. Crear el ViewModel
        AgregarProductoViewModel model = new AgregarProductoViewModel
        {
            IdPresupuesto = id,
            // 3. Crear el SelectList
            ListaProductos = new SelectList(productos, "IdProductos", "Descripcion")
        };
        return View(model);
    }

    // 🟢 CREAR (POST) - CORRECCIÓN
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {
    // 1. Chequeo de Data Annotations
    if (!ModelState.IsValid)
    {
        return View(presupuestoVM);
    }

    // 2. Validación manual de fecha futura (REQUERIDA POR TP)
    if (presupuestoVM.FechaCreacion.Date > DateTime.Now.Date)
    {
        ModelState.AddModelError("FechaCreacion", "La fecha de creación no puede ser futura.");
        return View(presupuestoVM);
    }

    // 3. Mapeo de ViewModel a Modelo de Dominio
    var nuevoPresupuesto = new Presupuesto
    {
        NombreDestinatario = presupuestoVM.NombreDestinatario,
        FechaCreacion = presupuestoVM.FechaCreacion, // Mapear a FechaCreacion1
        Detalle = new List<PresupuestosDetalle.PresupuestoDetalle>() // Nuevo presupuesto inicia con detalle vacío
    };

    // 4. Guardado en Repositorio (con el modelo de dominio)
    presupuestoRepository.CrearPresupuesto(nuevoPresupuesto);

    // 5. Redirección
    return RedirectToAction(nameof(Index));
    }

    // 🟡 EDITAR (GET)
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = presupuestoRepository.ObtenerDetallesPresupuesto(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        // mapeo el model a view model
        var presupuestoVM = new PresupuestoViewModel
        {
            IdPresupuesto = presupuesto.IdPresupuesto,
            NombreDestinatario = presupuesto.NombreDestinatario,
            FechaCreacion = presupuesto.FechaCreacion
        };

        return View(presupuestoVM);
    }

    // 🟡 EDITAR (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id,PresupuestoViewModel presupuestoVM)
    {
        if (id != presupuestoVM.IdPresupuesto)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }

        //valido la fecha
        if (presupuestoVM.FechaCreacion.Date > DateTime.Now.Date)
        {
            ModelState.AddModelError("FechaCreacion", "La fecha de creación no puede ser futura.");
            return View(presupuestoVM);
        }

        //mapeo
        var presupuestoAEditar = new Presupuesto
        {
            IdPresupuesto = presupuestoVM.IdPresupuesto,
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };

        presupuestoRepository.ModificarPresupuesto(presupuestoAEditar.IdPresupuesto, presupuestoAEditar);

        return RedirectToAction(nameof(Index));
    }

    // 🔴 ELIMINAR (GET)
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var presupuesto = presupuestoRepository.ObtenerDetallesPresupuesto(id);
        if (presupuesto == null)
            return NotFound();

        return View(presupuesto);
    }

    // 🔴 ELIMINAR (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        presupuestoRepository.EliminarPresupuesto(id);
        return RedirectToAction("Index");
    }
}
