using Microsoft.AspNetCore.Mvc;
using Presupuestos;
using presupuestosRepository;

namespace tl2_tp7_2025_JuanMartinFeliu.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestoRepository presupuestoRepository;

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
    public IActionResult Create()
    {
        return View();
    }

    // 🟢 CREAR (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Presupuesto nuevo)
    {
        if (!ModelState.IsValid)
            return View(nuevo);

        presupuestoRepository.CrearPresupuesto(nuevo);
        return RedirectToAction("Index");
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
