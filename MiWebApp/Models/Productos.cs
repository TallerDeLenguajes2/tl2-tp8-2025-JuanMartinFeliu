using System;

namespace Productos{

    public class Producto{
        private int idProducto;
        private string descripcion;
        private int precio;

        // public int IdProducto {get; set;}
        // public string Descripcion {get; set;}
        // public double Precio {get; set;}

        public Producto()
        {

        }

        public Producto(int idProdu,string descrip,int prec)
        {
            IdProducto = idProdu;
            Descripcion = descrip;
            Precio = prec;
        }

        public int IdProducto { get => idProducto; set => idProducto = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int Precio { get => precio; set => precio = value; }
    }
}