using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

// Clase que maneja la carga y almacenamiento de datos en XML

namespace Empleados.XML
{
    public class EmpleadoDataManager
    {
        public void GuardarDatos(List<Empleado> empleados, string rutaArchivo)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Empleado>));
                using (StreamWriter writer = new StreamWriter(rutaArchivo))
                {
                    serializer.Serialize(writer, empleados);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los datos: {ex.Message}");
            }
        }

        public List<Empleado> LeerDatos(string rutaArchivo)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Empleado>));
                using (StreamReader reader = new StreamReader(rutaArchivo))
                {
                    return (List<Empleado>)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer los datos: {ex.Message}");
                return new List<Empleado>();
            }
        }
    }
}
