using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iso810.Entities;
using Iso810.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Iso810.Services
{
    public class CsvService : ICsvService
    {
        private readonly MainContext _context;
        private readonly IDatabaseService _databaseService;

        public CsvService(MainContext context, IDatabaseService databaseService)
        {
            _context = context;
            _databaseService = databaseService;
        }

        public async Task<ICollection<StudentsView>> UploadData(string source)
        {
            var arr = source.Split("\n")
                .Skip(1) /* If csv has header */
                .ToList();
            var list = ArrayToList(arr);

            await _databaseService.SaveData(list);

            return list;
        }

        public async Task<string> DownloadData()
        {
            var source = await _context.StudentsView.ToListAsync();
            var content = source.Select(x => Object2Line(x));
            var result = string.Join('\n', content);

            return result;
        }

        public string GetHeaders()
        {
            var text = "";
            var properties = typeof(StudentsView).GetProperties();

            foreach (var property in properties)
            {
                text += property == properties.Last()
                    ? property.Name
                    : property.Name + ",";
            }

            return text;
        }

        private string Object2Line(StudentsView obj)
        {
            var text = "";
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                text += property == properties.Last()
                    ? property.GetValue(obj, null).ToString()
                    : property.GetValue(obj, null).ToString() + ",";
            }

            return text;
        }

        private List<StudentsView> ArrayToList(List<string> lines)
        {
            var output = new List<StudentsView>();

            foreach (var line in lines)
            {
                string[] values = line.Split(',');
                output.Add(new StudentsView
                {
                    CodigoDelCentro = int.Parse(values[0]),
                    NombreDelCentro = values[1],
                    Sector = values[2],
                    Provincia = values[3],
                    Matricula = int.Parse(values[4]),
                    NombreDelEstudiante = values[5],
                    Asignatura = values[6],
                    Tanda = values[7],
                    Calificacion = int.Parse(values[8]),
                    CondicionAcademica = values[9],
                    Seccion = values[10],
                    Grado = values[11]
                });
            }

            return output;
        }
    }
}