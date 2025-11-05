using System;
using Microsoft.Data.Sqlite;
using Presupuestos;
using PresupuestosDetalle;

namespace presupuestosRepository
{

    public interface IPresupuestoRepository
    {
        public void CrearPresupuesto(Presupuesto presup);
        public List<Presupuesto> ListarPresupuestos();
        public Presupuesto ObtenerDetallesPresupuesto(int idBuscado);
        public void AgregarProducto(int idBuscado);
        public void EliminarPresupuesto(int idBuscado);

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
            comando.Parameters.Add(new SqliteParameter("@FechaCreacion", presup.FechaCreacion1)); comando.ExecuteNonQuery();
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
                    FechaCreacion1 = Convert.ToDateTime(lector["FechaCreacion"])
                };
                presupuesto.Add(pre);
            }

            return presupuesto;
        }

        public Presupuesto ObtenerDetallesPresupuesto(int idBuscado)
{
    using var connection = new SqliteConnection(connectionString);
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
        FechaCreacion1 = Convert.ToDateTime(lector["FechaCreacion"]),
        Detalle = new List<PresupuestoDetalle>()
    };
    lector.Close();

    // Ahora traemos los productos asociados (detalle)
    string consultaDetalle = @"SELECT pd.IdProducto, pd.Cantidad, p.Descripcion, p.Precio
                               FROM PresupuestosDetalle pd
                               JOIN Productos p ON p.IdProducto = pd.IdProducto
                               WHERE pd.IdPresupuesto = @Id";
    using var comandoDet = new SqliteCommand(consultaDetalle, connection);
    comandoDet.Parameters.AddWithValue("@Id", idBuscado);

    using var lectorDet = comandoDet.ExecuteReader();
    while (lectorDet.Read())
    {
        var producto = new Productos.Producto
        {
            IdProducto = Convert.ToInt32(lectorDet["IdProducto"]),
            Descripcion = lectorDet["Nombre"].ToString(),
            Precio = Convert.ToInt32(lectorDet["Precio"])
        };

        var detalle = new PresupuestosDetalle.PresupuestoDetalle
        {
            Producto = producto,
            Cantidad = Convert.ToInt32(lectorDet["Cantidad"])
        };

        presupuesto.Detalle.Add(detalle);
    }

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


    }
}