using Microsoft.AspNetCore.Mvc;
using Presupuestos;
using presupuestosRepository;
using productoRepository;
using SistemaVentas.Web.ViewModels; //Necesario para poder llegar a los ViewModels
using Microsoft.AspNetCore.Mvc.Rendering;
using Productos; // Necesario para SelectList


namespace tl2_tp7_2025_JuanMartinFeliu.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestoRepository presupuestoRepository;
    // Necesito el repositorio productos para el dropdown
    private ProductoRepository productoRepository;

    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestoRepository();
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
    public IActionResult Create(AgregarProductoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // LÓGICA CRÍTICA DE RECARGA: Si falla la validación,
            // debemos recargar el SelectList porque se pierde en el POST.
            var productos = productoRepository.ListarProductos();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");

            //Devolvemos el modelo con los errores y el dropdown recargado
            return View(model);
        }
            
        // 2. Si es VÁLIDO: Llamamos al repositorio para guardar la relación
        _repo.AddDetalle(model.IdPresupuesto, model.IdProducto, model.Cantidad);

        // 3. Redirigimos al detalle del presupuesto
        return RedirectToAction(nameof(Details), new { id = model.IdPresupuesto });
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
