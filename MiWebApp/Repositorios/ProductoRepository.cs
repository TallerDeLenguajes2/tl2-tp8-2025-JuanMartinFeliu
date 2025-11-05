using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Productos;

namespace productoRepository
{
    public interface IProductoRepository
    {
        void CrearProducto(Producto prod);
        void ModificarProductos(Producto prod, int idProductoBuscado);
        List<Producto> ListarProductos();
        Producto ObtenerDetallesProducto(int idBuscado);
        void EliminarProductos(int idBuscado);
    }

    public class ProductoRepository : IProductoRepository
    {
        private string connectionString = "Data Source=Data/Tienda.db";

        public void CrearProducto(Producto prod)
        {
            using var conexion = new SqliteConnection(connectionString);
            conexion.Open();
            string sql = "INSERT INTO Productos (idProducto, Descripcion, Precio) VALUES (@idProducto, @Descripcion, @Precio)";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@idProducto", prod.IdProducto));
            comando.Parameters.Add(new SqliteParameter("@Descripcion", prod.Descripcion));
            comando.Parameters.Add(new SqliteParameter("@Precio", prod.Precio)); comando.ExecuteNonQuery();
        }


        public void ModificarProductos(Producto prod, int idProductoBuscado)
        {
            using var conexion = new SqliteConnection(connectionString);
            conexion.Open();

            // Actualizamos la fila cuyo idProducto coincida con idProductoBuscado
            string sql = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @Id";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Descripcion", prod.Descripcion);
            comando.Parameters.AddWithValue("@Precio", prod.Precio);
            comando.Parameters.AddWithValue("@Id", idProductoBuscado);

            comando.ExecuteNonQuery();
        }

        public List<Producto> ListarProductos()
        {
            var productos = new List<Producto>();
            using var conexion = new SqliteConnection(connectionString);
            conexion.Open();

            string sql = "SELECT idProducto, Descripcion, Precio FROM Productos";
            using var comando = new SqliteCommand(sql, conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                var p = new Producto
                {
                    IdProducto = Convert.ToInt32(lector["idProducto"]),
                    Descripcion = lector["Descripcion"].ToString(),
                    Precio = Convert.ToInt32(lector["Precio"])
                };
                productos.Add(p);
            }

            return productos;
        }

        public Producto ObtenerDetallesProducto(int idBuscado)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // WHERE sobre la columna correcta
            string consulta = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @Id";
            using var comando = new SqliteCommand(consulta, connection);
            comando.Parameters.AddWithValue("@Id", idBuscado);

            using var lector = comando.ExecuteReader();
            if (lector.Read())
            {
                var prod = new Producto
                {
                    IdProducto = Convert.ToInt32(lector["idProducto"]),
                    Descripcion = lector["Descripcion"].ToString(),
                    Precio = Convert.ToInt32(lector["Precio"])
                };
                return prod;
            }

            return null;
        }

        public void EliminarProductos(int idBuscado)
        {
            using var conexion = new SqliteConnection(connectionString);
            conexion.Open();

            string sql = "DELETE FROM Productos WHERE idProducto = @Id";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Id", idBuscado);

            comando.ExecuteNonQuery();
        }
    }
}