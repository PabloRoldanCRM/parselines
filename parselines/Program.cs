using IllyumL2T.Core.Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parselines
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "lineas_semanal_.csv";
            string path = Path.Combine(@"../../CSV", fileName);
    
            List<LineasFile> lineasFiles =  ObtieneDatosArchivo(path);
            Console.ReadKey();
        }

        public static List<LineasFile> ObtieneDatosArchivo(string path)
        {
            try
            {
                List<LineasFile> lineas = new List<LineasFile>();

                using (var reader = new StreamReader(path, Encoding.GetEncoding("iso8859-1")))
                {
                    var fileParser = new DelimiterSeparatedValuesFileParser<LineasFile>();
                    var parseResults = fileParser.Read(reader, delimiter: '|', includeHeaders: true);

                    List<IEnumerable<string>> errores = null;
                    foreach (var parseResult in parseResults)
                    {
                        if (parseResult.Errors != null && parseResult.Errors.Any())
                            errores.Add(parseResult.Errors);
                        if (parseResult.Instance != null)
                            lineas.Add(parseResult.Instance);
                    }

                    if (errores != null && errores.Any())
                    {
                        List<string> mensajes = new List<string>();
                        errores.ForEach(l => mensajes.Add(l.FirstOrDefault()));
                       // _logger.Error(mensajes);
                    }
                }
                // _logger.Info("Se encontraron - " + lineas.Count + " - registros en el archivo de Líneas Vencidas y por Vencer.");
                if (MoverArchivo(path))
                    Console.WriteLine($"Se proceso el archivo y el total de elementos el {lineas.Count}");
                else
                    Console.WriteLine($"No se proceso el archivo y el total de elementos el {lineas.Count}");
                return lineas;
            }
            catch (Exception ex)
            {
               throw new Exception("La ruta del archivo " + path + " es incorrecta." + ex);
        
            }
        
        }
        public static bool MoverArchivo(string path) {
            if (File.Exists(path))
                File.Move(path, @"../../CSV_Process/lineas_semanal_.csv");
            return true;
        }
    }

    public class LineasFile
    {
        public string IdPAC { get; set; }

        public string IdCliente { get; set; }

        public string NombreCliente { get; set; }

        public string FechaAutorizacionPAC { get; set; }

        public string PlazoLinea { get; set; }

        public string FechaVencimiento { get; set; }

        public string MontoSaldo { get; set; }

        public string TotalLineasBanco { get; set; }

        public string DíasVencido { get; set; }

        public string MesVencimiento { get; set; }

        public string NumEmpleado { get; set; }

        public string EstatusLineas { get; set; }
    }
}
