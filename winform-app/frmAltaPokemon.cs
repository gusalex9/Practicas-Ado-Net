using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winform_app
{
    public partial class frmAltaPokemon : Form
    {
        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Pokemons poke = new Pokemons();
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                //Cargar los datos del nuevo pokemon
                poke.Numero = int.Parse(txtNumero.Text);
                poke.Nombre = txtNombre.Text;
                poke.Descripcion = txtDescripcion.Text;
                //Mapeando la UrlImagen
                poke.UrlImagen = txtUrlImagen.Text;
                poke.Tipo = (Elemento)cbxTipo.SelectedItem;
                poke.Debilidad = (Elemento)cbxTipo.SelectedItem;

                //Enviar los datos cargado a la base de datos
                negocio.agregar(poke);
                MessageBox.Show("Agregado exitosamente");
                Close(); //Aqui cierro ventana
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            //Para llamar el metodo listar 
            ElementoNegocio negocio = new ElementoNegocio();
            try
            {
                //Cargamos los comboboxes con el metodo listar
                cbxTipo.DataSource = negocio.listar();
                cbxDebilidad.DataSource = negocio.listar(); 
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            //Se usa el try catch para capturar todos los errores posbles que puedan presentarse al cargar la imagen
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxPokemon.Load("https://uning.es/wp-content/uploads/2016/08/ef3-placeholder-image.jpg");
            }
        }
    }
}
