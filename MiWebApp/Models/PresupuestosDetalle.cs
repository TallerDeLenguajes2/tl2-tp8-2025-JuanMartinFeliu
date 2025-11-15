using System;
using Productos;

namespace PresupuestosDetalle
{
    public class PresupuestoDetalle
    {
        private Producto producto;
        private int cantidad;


        public PresupuestoDetalle()
        {

        }

        public PresupuestoDetalle(Producto prod, int cant)
        {
            this.Producto = prod;
            this.Cantidad = cant;
        }

        public Producto Producto { get => producto; set => producto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
    }
}