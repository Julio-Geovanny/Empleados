﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Empleados.XML
{
    public partial class Form1 : Form
    {
        private List<Empleado> empleados;
        private EmpleadoDataManager dataManager;
        private ErrorProvider errorProvider = new ErrorProvider();
        private List<string> puestos = new List<string> 
        { "Gerente", "Desarrollador", "Diseñador", "Analista", "Soporte" };

        public Form1()
        {
            InitializeComponent();
            empleados = new List<Empleado>();
            dataManager = new EmpleadoDataManager();
            cmbPuesto.DataSource = puestos;
        }

        private bool ValidarCampos()
        {
            bool esValido = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || !EsTextoValido(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Por favor, ingrese un nombre válido.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text) || !EsTextoValido(txtApellido.Text))
            {
                errorProvider2.SetError(txtApellido, "Por favor, ingrese un apellido válido.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtEdad.Text) || !int.TryParse(txtEdad.Text, out int edad) || edad < 18 || edad > 100)
            {
                errorProvider3.SetError(txtEdad, "Por favor, ingrese una edad válida.");
                esValido = false;
            }

            if (cmbPuesto.SelectedItem == null)
            {
                errorProvider4.SetError(cmbPuesto, "Por favor, seleccione un puesto.");
                esValido = false;
            }

            return esValido;
        }

        private bool EsTextoValido(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsLetter(c) && c != ' ')
                {
                    return false;
                }
            }
            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtEdad.Clear();
            cmbPuesto.SelectedIndex = -1;
        }

        // Guardar los datos de empleados en un archivo XML
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (empleados.Count > 0)
            {
                try
                {
                    dataManager.GuardarDatos(empleados, "empleados.xml");
                    MessageBox.Show("Datos guardados correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar los datos: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No hay datos para guardar.", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    Empleado nuevoEmpleado = new Empleado
                    {
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        Edad = int.Parse(txtEdad.Text),
                        Puesto = cmbPuesto.SelectedItem.ToString()
                    };

                    empleados.Add(nuevoEmpleado);
                    listBoxEmpleados.Items.Add($"{nuevoEmpleado.Nombre} {nuevoEmpleado.Apellido} - {nuevoEmpleado.Puesto}");
                    LimpiarCampos();
                    MessageBox.Show("Empleado añadido correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Error de formato: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cargar los empleados desde un archivo XML
        private void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("empleados.xml"))
                {
                    empleados = dataManager.LeerDatos("empleados.xml");

                    if (empleados.Count > 0)
                    {
                        ActualizarListaEmpleados();
                        MessageBox.Show("Datos cargados correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("El archivo empleados.xml está vacío.", "Advertencia", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("El archivo empleados.xml no existe.", "Advertencia", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarListaEmpleados()
        {
            listBoxEmpleados.Items.Clear();
            foreach (var empleado in empleados)
            {
                listBoxEmpleados.Items.Add($"{empleado.Nombre} {empleado.Apellido} - {empleado.Puesto}");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (listBoxEmpleados.SelectedIndex != -1)
            {
                Empleado empleadoSeleccionado = empleados[listBoxEmpleados.SelectedIndex];
                txtNombre.Text = empleadoSeleccionado.Nombre;
                txtApellido.Text = empleadoSeleccionado.Apellido;
                txtEdad.Text = empleadoSeleccionado.Edad.ToString();
                cmbPuesto.SelectedItem = empleadoSeleccionado.Puesto;

                empleados.RemoveAt(listBoxEmpleados.SelectedIndex);
                listBoxEmpleados.Items.RemoveAt(listBoxEmpleados.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Seleccione un empleado para editar.", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (listBoxEmpleados.SelectedIndex != -1)
            {
                empleados.RemoveAt(listBoxEmpleados.SelectedIndex);
                listBoxEmpleados.Items.RemoveAt(listBoxEmpleados.SelectedIndex);
                MessageBox.Show("Empleado eliminado correctamente.", "Éxito", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Seleccione un empleado para eliminar.", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPuesto.SelectedItem != null)
                {
                    string filtro = cmbPuesto.SelectedItem.ToString();
                    var empleadosFiltrados = empleados.Where(emp => emp.Puesto == filtro).ToList();

                    listBoxEmpleados.Items.Clear();
                    foreach (var empleado in empleadosFiltrados)
                    {
                        listBoxEmpleados.Items.Add($"{empleado.Nombre} {empleado.Apellido} - {empleado.Puesto}");
                    }

                    if (empleadosFiltrados.Count == 0)
                    {
                        MessageBox.Show("No se encontraron empleados con el puesto seleccionado.", 
                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un puesto para filtrar.", 
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al filtrar los empleados: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Validación en tiempo real para el nombre
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (!EsTextoValido(txtNombre.Text))
            {
                errorProvider.SetError(txtNombre, "Nombre inválido.");
            }
            else
            {
                errorProvider.SetError(txtNombre, string.Empty);
            }
        }
    }
}
