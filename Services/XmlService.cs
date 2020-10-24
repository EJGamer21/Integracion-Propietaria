using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Iso810.Entities;
using Iso810.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Iso810.Services
{
    public class XmlService : IXmlService
    {
        private MainContext _context;
        private IDatabaseService _databaseService;

        public XmlService(MainContext context, IDatabaseService databaseService)
        {
            _context = context;
            _databaseService = databaseService;
        }

        public async Task<byte[]> SaveFile()
        {
            var ms = new MemoryStream();
            var list = await _context.StudentsView.ToListAsync();

            var root = new XElement(
                "root",
                list.Select(obj => Object2Tag(obj))
            );
            root.Save(ms);

            return ms.ToArray();
        }
        
        private XElement Object2Tag(StudentsView obj)
        {
            var properties = typeof(StudentsView).GetProperties();
            var children = new List<XElement>();

            foreach (var property in properties)
            {
                children.Add(new XElement(
                    property.Name,
                    property.GetValue(obj, null).ToString()
                ));    
            }

            var result = new XElement(
                "Estudiante",
                new XAttribute("Matricula", obj.Matricula),
                children
            );

            return result;
        }

        public async Task<ICollection<StudentsView>> ImportFile(Stream stream)
        {
            var xml = XElement.Load(stream);
            var root = xml.Elements();
            var list = new List<StudentsView>();

            foreach (var item in root)
            {
                list.Add(new StudentsView
                {
                    CodigoDelCentro = int.Parse(item.Element("CodigoDelCentro").Value),
                    NombreDelCentro = item.Element("NombreDelCentro").Value,
                    Sector = item.Element("Sector").Value,
                    Provincia = item.Element("Provincia").Value,
                    Matricula = int.Parse(item.Element("Matricula").Value),
                    NombreDelEstudiante = item.Element("NombreDelEstudiante").Value,
                    Asignatura = item.Element("Asignatura").Value,
                    Tanda = item.Element("Tanda").Value,
                    Calificacion = int.Parse(item.Element("Calificacion").Value),
                    CondicionAcademica = item.Element("CondicionAcademica").Value,
                    Seccion = item.Element("Seccion").Value,
                    Grado = item.Element("Grado").Value
                });
            }

            await _databaseService.SaveData(list);

            return list;
        }
    }

    class StudentsViewList
    {

    }
}