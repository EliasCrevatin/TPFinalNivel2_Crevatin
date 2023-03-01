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
using dominio;
using negocio;
using System.Configuration;


namespace Presentación
{
    public partial class ArticuloNuevo : Form
    {

        private Articulo articulo = null;

        private OpenFileDialog archivo = null;

        public ArticuloNuevo()
        {
            InitializeComponent();
        }

        public ArticuloNuevo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }


        private void ArticuloNuevo_Load(object sender, EventArgs e)
        {
            MarcaNegocio Marca = new MarcaNegocio();
            CategoriaNegocio Categoria = new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = Marca.listarMarca();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = Categoria.listarCategoria();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";


                if (articulo != null)
                {
                    txtboxCodigo.Text = articulo.Codigo.ToString();
                    txtboxNombre.Text = articulo.Nombre;
                    txtboxDescripcion.Text = articulo.Descripcion;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    txtboxImagenUrl.Text = articulo.ImagenUrl;
                    CargarImagen(articulo.ImagenUrl);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {

            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();
                articulo.Codigo = txtboxCodigo.Text;
                articulo.Nombre = txtboxNombre.Text;
                articulo.Descripcion = txtboxDescripcion.Text;
                articulo.Marca = (Elemento)cboMarca.SelectedItem;
                articulo.Categoria = (Elemento)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txtboxImagenUrl.Text;
                articulo.Precio = decimal.Parse(txtboxPrecio.Text);

                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente!");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente!");
                }

                //guardo imagen si la levanto localmente
                if (archivo != null && !(txtboxImagenUrl.Text.ToUpper().Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["catalogo-app"] + archivo.SafeFileName);

                Close();
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


        private void txtboxImagenUrl_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtboxImagenUrl.Text);
        }



        private void CargarImagen(string imagen)
        {
            try
            {
                pictureBoxUrlImagen.Load(imagen);
            }
            catch (Exception)
            {

                pictureBoxUrlImagen.Load("https://media.istockphoto.com/vectors/image-preview-icon-picture-placeholder-for-website-or-uiux-design-vector-id1222357475?k=20&m=1222357475&s=612x612&w=0&h=jPhUdbj_7nWHUp0dsKRf4DMGaHiC16kg_FSjRRGoZEI=");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            archivo.ShowDialog();
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtboxImagenUrl.Text = archivo.FileName;
                CargarImagen(archivo.FileName);


            }
        

        }
    }
    
}
