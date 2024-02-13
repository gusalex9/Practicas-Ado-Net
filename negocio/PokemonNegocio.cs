using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.Diagnostics.Contracts;
using System.Net;

namespace negocio
{
    public class PokemonNegocio
    {
        public List<Pokemons> listar()
        {
            List<Pokemons> lista = new List<Pokemons >();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad FROM POKEMONS P, ELEMENTOS E, ELEMENTOS D WHERE E.Id = P.IdTipo AND D.ID = P.IdDebilidad ");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pokemons aux = new Pokemons();
                    aux.Numero = (int)datos.Lector["Numero"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    //Validacion de la lectura null de la columna UrlImagen
                    if (!(datos.Lector["UrlImagen"] is DBNull))
                    aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    //Como la propiedad Tipo es una clase primero hay que instanciar
                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                datos.cerrarConexion();
            }
        }
        public void agregar(Pokemons nuevoPokemon)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta($"Insert into POKEMONS(Numero, Nombre, Descripcion, UrlImagen, Activo, IdTipo, IdDebilidad )values({nuevoPokemon.Numero}, '{nuevoPokemon.Nombre}', '{nuevoPokemon.Descripcion}', @urlImagen, 1, @IdTipo, @idDebilidad)");
                datos.setearParametro("@idTipo", nuevoPokemon.Tipo.Id);
                datos.setearParametro("@idDebilidad", nuevoPokemon.Debilidad.Id);
                datos.setearParametro("@urlImagen", nuevoPokemon.UrlImagen);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            { 
                datos.cerrarConexion();
            }
        }
    }
}
