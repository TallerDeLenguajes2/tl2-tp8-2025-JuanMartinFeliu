using System;

namespace Productos{

    public class Producto{
        private int idProducto;
        private string descripcion;
        private int precio;

        public int IdProducto {get; set;}
        public string Descripcion {get; set;}
        public int Precio {get; set;}

        public Producto()
        {

        }

        public Producto(int idProducto,string descripcion,int precio)
        {
            this.idProducto = idProducto;
            this.descripcion = descripcion;
            this.precio = precio;
        }
    }
}