using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreZapatillas.Data;
using MvcNetCoreZapatillas.Models;
using System.Data;

#region SQL SERVER
//VUESTRO PROCEDIMIENTO DE PAGINACION DE IMAGENES DE ZAPATILLAS
//CREATE PROCEDURE SP_IMAGENES_ZAPATILLA
//	(@POSICION INT,
//    @IDPRODUCTO INT,
//    @NUMREGISTROS INT OUT)
//AS
//SELECT @NUMREGISTROS = COUNT(@IDPRODUCTO) FROM IMAGENESZAPASPRACTICA WHERE IDPRODUCTO = @IDPRODUCTO
//    SELECT IDIMAGEN, IDPRODUCTO, IMAGEN FROM(
//            SELECT* FROM
//            (SELECT CAST(
//                    ROW_NUMBER() OVER(ORDER BY IDIMAGEN) AS INT) AS POSICION,
//                        IDIMAGEN, IDPRODUCTO, IMAGEN
//            FROM IMAGENESZAPASPRACTICA
//            WHERE @IDPRODUCTO = @IDPRODUCTO) AS QUERY
//            WHERE QUERY.POSICION>=@POSICION AND QUERY.POSICION<(@POSICION+1)) AS QUERY
//    ORDER BY IDIMAGEN
//GO
#endregion

namespace MvcNetCoreZapatillas.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public List<Zapatilla> GetZapatillas()
        {
            return this.context.Zapatillas.ToList();
        }
        public Zapatilla GetZapatilla(int idproducto)
        {
            return this.context.Zapatillas.FirstOrDefault(x => x.IdProducto == idproducto);
        }

        public async Task<PaginarZapatillas> GetImagenesAsync(int posicion, int idproducto)
        {
            string sql = "SP_IMAGENES_ZAPATILLA @POSICION, @IDPRODUCTO, @NUMREGISTROS OUT";
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            SqlParameter pamidproducto = new SqlParameter("@IDPRODUCTO", idproducto);
            SqlParameter pamnumregistros = new SqlParameter("@NUMREGISTROS", -1);
            pamnumregistros.Direction = ParameterDirection.Output;
            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, pamposicion, pamidproducto, pamnumregistros);
            pamnumregistros.Direction = ParameterDirection.Output;
            //Saco los personajes
            List<ImagenZapatilla> imagenes = await consulta.ToListAsync();
            //Saco el num de registros
            int numregistros = (int)pamnumregistros.Value;
            return new PaginarZapatillas
            {
                NumRegistros = numregistros,
                ImagenesZapa = imagenes,
                Posicion = posicion
            };
        }
    }
}