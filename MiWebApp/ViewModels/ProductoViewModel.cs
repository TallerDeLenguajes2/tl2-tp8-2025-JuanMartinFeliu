using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Web.ViewModels
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }

        //Descripcion con validacion de 250 caracteres. Si no tiene Required es opcional.
        [Display(Name = " Descripcion del Producto")]
        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        public string Descripcion { get; set; }

        //Precio debe ser positivo y requerido
        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage = "El precio debe ser obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El precio debe tener un valor positivo")]
        public int Precio { get; set; }
    }
}