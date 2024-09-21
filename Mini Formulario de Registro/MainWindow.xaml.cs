using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Mini_Formulario_de_Registro
{
    public partial class MainWindow : Window
    {
        private static int id;
        private readonly string rutaArchivo = @"C:\Users\IK\Desktop\registro.txt";

        public MainWindow()
        {
            InitializeComponent();
            CargarUltimoIdDesdeRegistros();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CargarUltimoIdDesdeRegistros()
        {
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    string[] lineas = File.ReadAllLines(rutaArchivo);

                    foreach (var linea in lineas)
                    {
                        if (linea.StartsWith("ID: "))
                        {
                            string[] partes = linea.Split(' ');
                            if (int.TryParse(partes[1], out int idLeido))
                            {
                                id = Math.Max(id, idLeido);
                            }
                        }
                    }
                    id++;
                }
                else
                {
                    id = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el ID desde los registros: {ex.Message}");
                id = 1;
            }
        }

        public void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text;
                string correo = txtCorreo.Text;
                string direccion = txtDireccion.Text;
                string telefono = txtTelelefono.Text;

                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) ||
                    string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(direccion))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                if (!Regex.IsMatch(correo, patronCorreo))
                {
                    MessageBox.Show("Por favor, ingrese un correo electrónico válido.");
                    return;
                }



                if (miCheckBox.IsChecked == true)
                {
                    string lineaDatos = $"ID: {id} Nombre: {nombre}, Correo: {correo}, Teléfono: {telefono}, Dirección: {direccion}";

                    using (StreamWriter sw = new StreamWriter(rutaArchivo, true))
                    {
                        sw.WriteLine(lineaDatos);
                    }

                    MessageBox.Show("Datos registrados exitosamente.");
                    id++;

                    txtNombre.Clear();
                    txtCorreo.Clear();
                    txtDireccion.Clear();
                    txtTelelefono.Clear();
                }
                else
                {
                    MessageBox.Show("Debes aceptar los términos y condiciones");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar datos: {ex.Message}");
            }
        }
    }
}
