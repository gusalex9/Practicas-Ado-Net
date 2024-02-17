﻿using System;
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
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            //Capturar el objeto que esta en la fila seleccionada.
            Pokemons seleccionado = (Pokemons)dgvPokemons.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.UrlImagen);
        }

        private void cargar()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;
                dgvPokemons.Columns["Id"].Visible = false;
                dgvPokemons.Columns["UrlImagen"].Visible = false;

                cargarImagen(listaPokemon[0].UrlImagen);
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
    } 
}