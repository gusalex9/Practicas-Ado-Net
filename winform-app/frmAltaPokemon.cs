using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace winform_app
{
    public partial class frmAltaPokemon : Form
    {
        private Pokemons pokemon = null;
        private OpenFileDialog archivo;
        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        //Por aqui pasa cuando se da click en el boton modificar
        public frmAltaPokemon(Pokemons pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon; //Cargamos el atributo privado de esta clase con un pokemon
            Text = "Modificar Pokemon";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                //Cargar los datos del nuevo pokemon
                //Utilizaremos el atributo privado
                if(pokemon == null)
                    pokemon = new Pokemons(); //Si estas agregando un pokemon tienes que instanciar el atributo que esta en null
                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                //Mapeando la UrlImagen
                pokemon.UrlImagen = txtUrlImagen.Text;
                pokemon.Tipo = (Elemento)cbxTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cbxDebilidad.SelectedItem;

                //Si nosotros queremos modificar un objeto significa que ya tiene un Id existente
                if(pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente");
                    
                }
                else
                {
                    negocio.agregar(pokemon);
                    MessageBox.Show("Agregado exitosamente");
                }

                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    //Guardo la imagen
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app"] + archivo.FileName);
                }

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
                cbxTipo.ValueMember = "Id"; //le asignamos el Id de la propiedad elemento
                cbxTipo.DisplayMember = "Descripcion"; //le asignamos el Descripcion de la propiedad elemento

                cbxDebilidad.DataSource = negocio.listar();
                cbxDebilidad.ValueMember = "Id"; //le asignamos el Id de la propiedad elemento
                cbxDebilidad.DisplayMember = "Descripcion"; //le asignamos el Descripcion de la propiedad elemento

                if (pokemon != null)
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(txtUrlImagen.Text);

                    //Preseleccionar los valores de los combo boxes con el objeto pokemon
                    cbxTipo.SelectedValue = pokemon.Tipo.Id;
                    cbxDebilidad.SelectedValue = pokemon.Debilidad.Id;
                } 
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

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            //Levcantar archicos de la computadora
            archivo = new OpenFileDialog();

            //Indicar que tipo de archivo quieres permitir
            archivo.Filter = "jpg|*.jpg;|png|*.png";

            //Levantar la pantalla para subir el archivo
            archivo.ShowDialog();

            //Validar para capturar el archivho seleccionado
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(txtUrlImagen.Text);
            }
        }
    }
}
