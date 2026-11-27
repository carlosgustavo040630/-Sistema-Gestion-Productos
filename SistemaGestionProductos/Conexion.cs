using System.Data.SqlClient;

namespace SistemaGestionProductos
{
    public class Conexion
    {
        private readonly string cadenaConexion =
            @"Server=DESKTOP-MJ3JFQP\SQLEXPRESS;Database=SistemaGestionProductos;Integrated Security=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}
