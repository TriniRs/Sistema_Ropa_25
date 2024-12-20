﻿using CapaControladora;
using CapaEntidad;
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

namespace CapaPresentacion.Modales
{
    public partial class mdListaProveedores : Form
    {
        public Proveedor _Proveedor { get; set; }
        public int _idProveedor { get; set; }

        public mdListaProveedores()
        {
            InitializeComponent();
        }

        private void mdListaProveedores_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn columna in dataGridViewData.Columns)
            {
                if (columna.Visible == true)
                {
                    comboBoxBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            comboBoxBusqueda.DisplayMember = "Texto";
            comboBoxBusqueda.ValueMember = "Valor";
            comboBoxBusqueda.SelectedIndex = 0;

            List<Proveedor> lista = new CC_Proveedor().ListarProveedores().Where(p => p.Estado).ToList();
            foreach (Proveedor item in lista)
            {
                dataGridViewData.Rows.Add(new object[] { 
                    item.IdProveedor, 
                    item.CUIT, 
                    item.RazonSocial,
                    item.Correo
                });
            }
        }

        private void dataGridViewData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColumn = e.ColumnIndex;

            if (iRow >= 0 && iColumn > 0)
            {
                _Proveedor = new Proveedor()
                {
                    IdProveedor = Convert.ToInt32(dataGridViewData.Rows[iRow].Cells["idProveedor"].Value.ToString()),
                    CUIT = dataGridViewData.Rows[iRow].Cells["cuit"].Value.ToString(),
                    RazonSocial = dataGridViewData.Rows[iRow].Cells["RazonSocial"].Value.ToString(),
                    Correo = dataGridViewData.Rows[iRow].Cells["Correo"].Value.ToString()
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)comboBoxBusqueda.SelectedItem).Valor.ToString();

            if (dataGridViewData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridViewData.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(textBoxBusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        private void buttonLimpiarBuscardor_Click(object sender, EventArgs e)
        {
            textBoxBusqueda.Clear();
            foreach (DataGridViewRow row in dataGridViewData.Rows)
            {
                row.Visible = true;
            }
        }

        private void buttonAgregarProveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new mdDetalleProveedor("Agregar", 0))
            {
                var resultado = modal.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    _idProveedor = modal.idProveedor;

                    Proveedor oProveedor = new CC_Proveedor().ListarProveedores().Where(c => c.IdProveedor == _idProveedor).FirstOrDefault();

                    _Proveedor = new Proveedor()
                    {
                        IdProveedor = oProveedor.IdProveedor,
                        CUIT = oProveedor.CUIT,
                        RazonSocial = oProveedor.RazonSocial,
                        Correo = oProveedor.Correo
                    };
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void textBoxBusqueda_TextChanged(object sender, EventArgs e)
        {
            buttonBuscar_Click(sender, e);
            if (textBoxBusqueda.Text.Trim() == "")
            {
                buttonLimpiarBuscardor_Click(sender, e);
            }
        }
    }
}
