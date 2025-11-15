using System;
using PresupuestosDetalle;

namespace Presupuestos {

    public class Presupuesto {
        private int idPresupuesto;
        private string nombreDestinatario;
        private DateTime fechaCreacion;
        private List<PresupuestoDetalle> detalle;

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

        //Defino los get set publicos

        // Hago los Constructores

        public Presupuesto()
        {
        detalle =new  List<PresupuestoDetalle>();
        }

        public Presupuesto(int id, string nombre, DateTime fecha, List<PresupuestoDetalle> detalle)
        {
            this.IdPresupuesto = id;
            this.NombreDestinatario = nombre;
            this.FechaCreacion = fecha;
            this.Detalle = detalle;
        }




        // Empiezan los metodos

        public double MontoPresupuesto()
        {
            double monto = 0;
            foreach(var det in Detalle)
            {
                monto = monto + det.Producto.Precio * det.Cantidad;
            }
            return monto;
        }
        public double MontoPresupuestoConIva()
        {
            return MontoPresupuesto() * (1.21);
        }
        public int CantidadProductos()
        {
            int total = 0;
            foreach (var det in Detalle)
            {
                total = total + det.Cantidad;
            }
            return total;
        }

    }

}