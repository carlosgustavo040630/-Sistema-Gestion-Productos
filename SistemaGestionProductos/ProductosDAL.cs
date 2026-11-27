using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemaGestionProductos
{
    public class ProductosDAL
    {
        private Conexion conexion = new Conexion();

        public DataTable ConsultarProductos()
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand("sp_ConsultarProductos", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tabla = new DataTable();

                    cn.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(tabla);
                    }

                    return tabla;
                }
            }
        }
        public void RegistrarProducto(string codigo, string nombre,string descripcion, decimal precio, int stock)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Codigo", codigo);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@Precio", precio);
                    cmd.Parameters.AddWithValue("@Stock", stock);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ModificarProducto(int idProducto, string codigo,string nombre, string descripcion, decimal precio, int stock)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand("sp_ModificarProducto", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.Parameters.AddWithValue("@Codigo", codigo);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@Precio", precio);
                    cmd.Parameters.AddWithValue("@Stock", stock);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProducto(int idProducto)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand("dbo.EliminarProducto", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
