﻿using CapaControladora;
using CapaEntidad;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmAuditoriaCompra : Form
    {
        private Usuario _usuarioActual;
        private CC_AuditoriaCompra oCC_AuditoriaCompra = new CC_AuditoriaCompra();

        public frmAuditoriaCompra(Usuario oUsuario)
        {
            _usuarioActual = oUsuario;
            InitializeComponent();
        }

        private void frmAuditoriaCompra_Load(object sender, EventArgs e)
        {
            //CONFIGURACION DEL OPCION COMBO SELECCIONAR
            foreach (DataGridViewColumn columna in dataGridView.Columns)
            {
                if (columna.Visible && columna.Name != "buttonSeleccionar")
                {
                    comboBoxBusqueda.Items.Add(new OpcionCombo(columna.Name, columna.HeaderText));
                }
            }
            comboBoxBusqueda.SelectedIndex = 0;
            comboBoxBusqueda.DisplayMember = "Texto";
            comboBoxBusqueda.ValueMember = "Valor";

            //MODULO DE SEGURIDAD - VISIBILIDAD DE LOS MENUES
            foreach (ToolStripMenuItem menu in menu.Items)
            {
                bool encontrado = _usuarioActual.GetPermisos().Any(p => p.NombreMenu == menu.Name);

                if (encontrado)
                {
                    menu.Visible = true;
                }
                else
                {
                    menu.Visible = false;
                }
            }

            menuVerDetalleAuditoriaCompra.Visible = true;

            buttonActualizar_Click(sender, e);
        }

        private void menuVerDetalleAuditoriaCompra_Click(object sender, EventArgs e)
        {
            if (textBoxId.Text != "")
            {
                using (var modal = new mdDetalleAuditoriaCompra(Convert.ToInt32(textBoxId.Text)))
                {
                    var resultado = modal.ShowDialog();

                    if (resultado == DialogResult.OK)
                    {
                        buttonActualizar_Click(null, null);
                    }
                }
                buttonActualizar_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Seleccione una compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();

            //MOSTRAR LOS COMPRA
            List<AuditoriaCompra> listaAuditoriaCompras = oCC_AuditoriaCompra.ListarAuditoriaCompras();

            foreach (AuditoriaCompra oAuditoriaCompra in listaAuditoriaCompras)
            {
                dataGridView.Rows.Add(
                    "",
                    oAuditoriaCompra.IdAuditoriaCompra,
                    oAuditoriaCompra.oUsuarioAuditoria.IdUsuario,
                    oAuditoriaCompra.oUsuarioAuditoria.IdPersona,
                    oAuditoriaCompra.oUsuarioAuditoria.NombreCompleto,
                    oAuditoriaCompra.oUsuarioAuditoria.Documento,
                    oAuditoriaCompra.DescripcionAuditoria,
                    oAuditoriaCompra.FechaAuditoria,
                    oAuditoriaCompra.IdCompra,
                    oAuditoriaCompra.oUsuarioCompra.IdUsuario,
                    oAuditoriaCompra.oUsuarioCompra.IdPersona,
                    oAuditoriaCompra.oUsuarioCompra.NombreCompleto,
                    oAuditoriaCompra.oUsuarioCompra.Documento,
                    oAuditoriaCompra.oProveedor.IdProveedor,
                    oAuditoriaCompra.oProveedor.RazonSocial,
                    oAuditoriaCompra.oProveedor.CUIT,
                    oAuditoriaCompra.oProveedor.Correo,
                    oAuditoriaCompra.TipoDocumento,
                    oAuditoriaCompra.NumeroDocumento,
                    oAuditoriaCompra.MontoTotal,
                    oAuditoriaCompra.FechaRegistro
                    );
            }

            //CONFIGURA QUE NO ESTE SELECCIONADA NINGUNA FILA
            dataGridView.ClearSelection();

            textBoxId.Text = "";
            textBoxNumeroDocumento.Text = "";
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)comboBoxBusqueda.SelectedItem).Valor.ToString();

            if (dataGridView.Rows.Count > 0)
            {
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    if (fila.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(textBoxBusqueda.Text.Trim().ToUpper()))
                    {
                        fila.Visible = true;
                    }
                    else
                    {
                        fila.Visible = false;
                    }
                }
            }
        }

        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            textBoxBusqueda.Text = "";

            foreach (DataGridViewRow fila in dataGridView.Rows)
            {
                fila.Visible = true;
            }
        }

        private void textBoxBusqueda_TextChanged(object sender, EventArgs e)
        {
            buttonBuscar_Click(sender, e);
            if (textBoxBusqueda.Text.Trim() == "")
            {
                buttonLimpiar_Click(sender, e);
            }
        }

        private void comboBoxBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonBuscar_Click(sender, e);
            if (textBoxBusqueda.Text.Trim() == "")
            {
                buttonLimpiar_Click(sender, e);
            }
        }

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                e.PaintBackground(e.ClipBounds, true);

                var w = Properties.Resources.Seleccion.Width;
                var h = Properties.Resources.Seleccion.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.Seleccion, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indice = e.RowIndex;

            if (indice >= 0)
            {
                textBoxId.Text = dataGridView.Rows[indice].Cells["IdAuditoriaCompra"].Value.ToString();
                textBoxNumeroDocumento.Text = dataGridView.Rows[indice].Cells["NumeroDocumento"].Value.ToString();
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int indiceFila = e.RowIndex;
            int indiceColumna = e.ColumnIndex;

            if (indiceFila >= 0 && indiceColumna >= 0)
            {
                menuVerDetalleAuditoriaCompra_Click(sender, e);
            }
        }
    }
}
