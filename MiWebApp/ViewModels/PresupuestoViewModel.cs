using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Web.ViewModels
{
    public class PresupuestoViewModel
    {
        public int IdPresupuesto { get; set; }

        //NombreDestinatario (email) requerido
        [Display(Name = "Nombre o email del destinatario")]
        [Required(ErrorMessage = "El nombre o el emial es obligatorio")]
        public string NombreDestinatario { get; set; }

        //FechaCreacion tiene que ser requerido y controlar el tipo de dato
        [Display(Name = " Fecha de Creacion")]
        [Required(ErrorMessage = "La fechs es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion{ get; set; }
    }
}