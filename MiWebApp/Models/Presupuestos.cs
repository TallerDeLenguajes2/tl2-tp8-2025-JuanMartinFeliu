using System;
using PresupuestosDetalle;

namespace Presupuestos{

    public class Presupuesto{
        private int idPresupuesto;
        private string nombreDestinatario;
        private DateTime fechaCreacion;
        private List<PresupuestoDetalle> detalle;

        //Defino los get set publicos
        public int IdPresupuesto { get; set; }
        public string NombreDestinatario { get; set; }
        public DateTime FechaCreacion1 { get; set; }
        public List<PresupuestoDetalle> Detalle { get; set; }

        // Hago los Constructores

        public Presupuesto()
        {

        }

        public Presupuesto(int id, string nombre, DateTime fecha, List<PresupuestoDetalle> detalle)
        {
            this.idPresupuesto = id;
            this.nombreDestinatario = nombre;
            this.fechaCreacion = fecha;
            this.detalle = detalle;
        }

        


        // Empiezan los metodos

        public float MontoPresupuesto()
        {
            float monto = 0;
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