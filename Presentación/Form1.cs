using System;
using System.CodeDom;
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

namespace Presentación
{
    internal partial class Form1 : Form
    {
        private List<Articulo> ListaArticulo;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cargar();
        }


        private void Cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                filtroAvanzado();
                ListaArticulo = negocio.listar();
                dgvArticulos.DataSource = ListaArticulo;
                ocultarColumnas();
                CargarImagen(ListaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["id"].Visible = false;
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pBoxArticulo.Load(imagen);
            }
            catch (Exception)
            {

                pBoxArticulo.Load("https://media.istockphoto.com/vectors/image-preview-icon-picture-placeholder-for-website-or-uiux-design-vector-id1222357475?k=20&m=1222357475&s=612x612&w=0&h=jPhUdbj_7nWHUp0dsKRf4DMGaHiC16kg_FSjRRGoZEI=");
            }
        }


        private void filtroAvanzado()
        {
            //Campo
            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Precio");


        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }


        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                CargarImagen(seleccionado.ImagenUrl);
            }
        }



        private void txtboxFiltro_TextChanged(object sender, EventArgs e)
        {

            try
            {
                List<Articulo> listaFiltrada;
                string filtro = txtboxFiltro.Text;

                if (filtro.Length >= 2)
                {
                    listaFiltrada = ListaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()));
                }
                else
                {
                    listaFiltrada = ListaArticulo;
                }

                dgvArticulos.DataSource = null;
                dgvArticulos.DataSource = listaFiltrada;
                ocultarColumnas();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private bool validarFiltro()
        {
            //CAMPO SIN SELECCIONAR
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione un campo a filtrar!");
                return true;
            }


            /////////////////////////////

            //CRITERIO SIN SELECCIONAR
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione un creterio a filtrar!");
                return true;
            }

            ///////////////////////////////////
            // ESCRIBIR SOLO NUMEROS EN CASO DE SELECCIONAR "PRECIO" EN EL CAMPO 
            //
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (!(soloNumeros(txtboxFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Por favor introduzca solo números");
                    return true;
                }
            }

            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;

            }
            return true;
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            {
                ArticuloNegocio negocio = new ArticuloNegocio();

                try
                {
                    if (validarFiltro())
                        return;

                    string campo = cboCampo.SelectedItem.ToString();
                    string criterio = cboCriterio.SelectedItem.ToString();
                    string filtro = txtboxFiltroAvanzado.Text;
                    dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ArticuloNuevo nuevo = new ArticuloNuevo();
            nuevo.ShowDialog();
            Cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            ArticuloNuevo Modificar = new ArticuloNuevo(seleccionado);
            Modificar.ShowDialog();
            Cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void eliminar(bool logico = false)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Esta seguro que desea eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    Cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }   
    }
}
