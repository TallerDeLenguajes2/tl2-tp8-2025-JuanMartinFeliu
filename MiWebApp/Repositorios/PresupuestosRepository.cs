using System;
using Microsoft.Data.Sqlite;
using Presupuestos;
using PresupuestosDetalle;
using Productos;

namespace presupuestosRepository
{

    public interface IPresupuestoRepository
    {
        public void CrearPresupuesto(Presupuesto presup);
        public List<Presupuesto> ListarPresupuestos();
        public Presupuesto ObtenerDetallesPresupuesto(int idBuscado);
        public void AgregarProducto(int idBuscado);
        public void EliminarPresupuesto(int idBuscado);
        public void ModificarPresupuesto(int idPresupuesto, Presupuesto presupuesto);

    }

    public class PresupuestoRepository : IPresupuestoRepository
    {
        private string connectionString = "Data Source=Data/Tienda.db";
        public void CrearPresupuesto(Presupuesto presup)
        {
            using var conexion = new SqliteConnection(connectionString);
            conexion.Open();
            string sql = "INSERT INTO Presupuestos (idPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@idPresupuesto, @NombreDestinatario, @FechaCreacion)";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.Add(new SqliteParameter("@idPresupuesto", presup.IdPresupuesto));
            comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", presup.NombreDestinatario));
            comando.Parameters.Add(new SqliteParameter("@FechaCreacion", presup.FechaCreacion)); comando.ExecuteNonQuery();
        }

        public List<Presupuesto> ListarPresupuestos()
        {
            var presupuesto = new List<Presupuesto>();
            using var conexion = new SqliteConnection(connectionString);
            conexion.Open();

            string sql = "SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";
            using var comando = new SqliteCommand(sql, conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                var pre = new Presupuesto
                {
                    IdPresupuesto = Convert.ToInt32(lector["idPresupuesto"]),
                    NombreDestinatario = lector["NombreDestinatario"].ToString(),
                    FechaCreacion = Convert.ToDateTime(lector["FechaCreacion"])
                };
                presupuesto.Add(pre);
            }

            return presupuesto;
        }

 public Presupuesto ObtenerDetallesPresupuesto(int idBuscado)
{var presupuesto = new Presupuesto();
   /* using var connection = new SqliteConnection(connectionString);
    connection.Open();

    // Primero traemos los datos del presupuesto
    string consultaPresu = "SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos WHERE idPresupuesto = @Id";
    using var comandoPresu = new SqliteCommand(consultaPresu, connection);
    comandoPresu.Parameters.AddWithValue("@Id", idBuscado);

    using var lector = comandoPresu.ExecuteReader();
    if (!lector.Read())
        return null;

    var presupuesto = new Presupuesto
    {
        IdPresupuesto = Convert.ToInt32(lector["idPresupuesto"]),
        NombreDestinatario = lector["NombreDestinatario"].ToString(),
        FechaCreacion = Convert.ToDateTime(lector["FechaCreacion"]),
        Detalle = new List<PresupuestoDetalle>()
    };
    lector.Close();

    // Ahora traemos los productos asociados (detalle)
    string consultaDetalle = @"SELECT pd.idProducto, pd.cantidad, p.Descripcion, p.Precio
                               FROM PresupuestosDetalle pd
                               JOIN Productos p ON p.idProducto = pd.idProducto
                               WHERE pd.IdPresupuesto = @Id;";
    using var comandoDet = new SqliteCommand(consultaDetalle, connection);
    comandoDet.Parameters.AddWithValue("@Id", idBuscado);

    using var lectorDet = comandoDet.ExecuteReader();
    while (lectorDet.Read())
    {
      //  var producto= new Productos.Producto(Convert.ToInt32(lectorDet["idProducto"]),Convert.ToString(lectorDet["Descripcion"]),Convert.ToInt32(lectorDet["Precio"]));
          Producto producto= new Producto(1,"hola",100);
        
        // var producto = new Productos.Producto
        // {
        //     IdProducto = Convert.ToInt32(lectorDet["idProducto"]),
        //     Descripcion = lectorDet["Descripcion"].ToString(),
        //     Precio = Convert.ToInt32(lectorDet["Precio"])
        // };

        var detalle = new PresupuestosDetalle.PresupuestoDetalle
        {
            Producto = producto,

            Cantidad = Convert.ToInt32(lectorDet["cantidad"])
        };

        presupuesto.Detalle.Add(detalle);
    }
*/
    return presupuesto;
}


        public void AgregarProducto(int idBuscado)
        {

        }

        public void EliminarPresupuesto(int idBuscado)
        {
            using var conexion = new SqliteConnection(connectionString);
            conexion.Open();

            string sql = "DELETE FROM Presupuestos WHERE idPresupuesto = @Id";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Id", idBuscado);

            comando.ExecuteNonQuery();
        }

        // Añade este método a PresupuestosRepository.cs

        public void ModificarPresupuesto(int idPresupuesto, Presupuesto presupuesto)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                
                // Actualizamos solo los campos que vienen del ViewModel
                command.CommandText = "UPDATE Presupuestos SET NombreDestinatario = @NombreDestinatario, FechaCreacion = @FechaCreacion WHERE idPresupuesto = @Id";
                
                command.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
                command.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);
                command.Parameters.AddWithValue("@Id", idPresupuesto);

                command.ExecuteNonQuery();
            }
        }
    }
}