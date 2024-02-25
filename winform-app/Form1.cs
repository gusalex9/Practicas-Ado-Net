using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace winform_app
{
    public partial class frmPokemons : Form
    {
        private List<Pokemons> listaPokemon;
        public frmPokemons()
        {
            InitializeComponent();
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {
            cargar();
            //Cargar el combo box campo
            cbxCampo.Items.Add("Numero");
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Descripcion");

        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            //Solucion del error SelecteddIndexChanged
            if(dgvPokemons.CurrentRow != null)
            {
                //Capturar el objeto que esta en la fila seleccionada.
                Pokemons seleccionado = (Pokemons)dgvPokemons.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
            
        }

        private void cargar()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;
                ocultarColumanas();
                cargarImagen(listaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumanas()
        {
            dgvPokemons.Columns["Id"].Visible = false;
            dgvPokemons.Columns["UrlImagen"].Visible = false;
        }
        private bool validarFiltro()
        {
            if(cbxCampo.SelectedIndex < 0) //si es menos 1 es por que no se ha seleccionado nada
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar"); 
                return true;
            }

            if(cbxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar");
                return true;
            }

            if(cbxCampo.SelectedItem.ToString() == "Numero")
            {
                if (string.IsNullOrEmpty(txtFiltroAvando.Text))//Si esta vacio
                {
                    MessageBox.Show("Ingresa un numero");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvando.Text))) //si no ingresa un numero
                {
                    MessageBox.Show("Ingresar solo numeros");
                    return true;
                }
            }
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach(char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if (validarFiltro())
                    return; // Aqui cancelamos el evento si no se ha seleccionado nada

                //Capturar Valores
                string campo = cbxCampo.SelectedItem.ToString();
                string criterio = cbxCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvando.Text;

                dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //Primero capturar el pokemon seleccionado
            Pokemons seleccionado;
            seleccionado = (Pokemons)dgvPokemons.CurrentRow.DataBoundItem;
            //Pasamos el objeto seleccionado al constructor de la clase
            frmAltaPokemon modicar = new frmAltaPokemon(seleccionado);
            modicar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar(bool logico = false)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemons pokemons;
            try
            {
                pokemons = (Pokemons)dgvPokemons.CurrentRow.DataBoundItem;
                DialogResult resu = MessageBox.Show($"Seguro que deseas elimanar a {pokemons.Nombre}", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resu == DialogResult.Yes)
                {
                    if (logico)
                        negocio.eliminarLogico(pokemons.Id);
                    else
                        negocio.eliminarFisico(pokemons.Id);

                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemons> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length > 3)
            {
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaPokemon;
            }

            dgvPokemons.DataSource = null;

            dgvPokemons.DataSource = listaFiltrada;
            ocultarColumanas();
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxCampo.SelectedItem.ToString();

            if(opcion == "Numero")
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Mayor a");
                cbxCriterio.Items.Add("Menor a");
                cbxCriterio.Items.Add("Igual a");
            }
            else
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Comienza con");
                cbxCriterio.Items.Add("Termina con");
                cbxCriterio.Items.Add("Contiene");
            }
        }
    } 
}