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

    // 🟢 CREAR (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {
        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }

        if (presupuestoVM.FechaCreacion.Date > DateTime.Now.Date)
        {
            ModelState.AddModelError("FechaCreacion", "La fecha de creación no puede ser futura.");
            return View(presupuestoVM);
        }
        
        // 3. LÓGICA DE MAPEO (VM -> Modelo de Dominio)
    // (Aquí está el mapeo completo que solicitaste)
    var nuevoPresupuesto = new Presupuesto
    {
        // Mapeamos las propiedades del ViewModel a la entidad de dominio
        NombreDestinatario = presupuestoVM.NombreDestinatario,
        
        // ¡Atención! Tu modelo Presupuestos.cs usa "FechaCreacion1"
        // Mapeamos presupuestoVM.FechaCreacion a nuevoPresupuesto.FechaCreacion1
        FechaCreacion1 = presupuestoVM.FechaCreacion, 
        
        // Al crear un presupuesto nuevo, la lista de detalles empieza vacía.
        // (Probablemente necesites agregar 'using PresupuestosDetalle;' 
        // al inicio de tu PresupuestoController.cs para que reconozca la clase PresupuestoDetalle)
        Detalle = new List<PresupuestoDetalle>() 
    };

        // 4. GUARDADO (Llamada al Repositorio)
        // (Usando el 'presupuestoRepository' inicializado en tu constructor)
        presupuestoRepository.CrearPresupuesto(nuevoPresupuesto);

        // 5. REDIRECCIÓN
        // Redirigimos al Index cuando todo sale bien
        return RedirectToAction(nameof(Index));
    }

    // 🟡 EDITAR (GET)
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = presupuestoRepository.ObtenerDetallesPresupuesto(id);
        if (presupuesto == null)
            return NotFound();

        return View(presupuesto);
    }

    // 🟡 EDITAR (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Presupuesto presup)
    {
        if (!ModelState.IsValid)
            return View(presup);

        // No tenés un método específico para editar, así que reutilizamos el delete + insert
        presupuestoRepository.EliminarPresupuesto(presup.IdPresupuesto);
        presupuestoRepository.CrearPresupuesto(presup);

        return RedirectToAction("Index");
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
