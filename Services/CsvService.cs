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

        public CsvService(MainContext context)
        {
            _context = context;
        }

        public async Task<ICollection<StudentsView>> UploadData(string source)
        {
            var arr = source.Split("\n")
                .Skip(1) /* If csv has header */
                .ToList();
            var list = ArrayToList(arr);

            await SaveData(list);

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

        private async Task SaveData(List<StudentsView> source)
        {
            foreach (var info in source)
            {
                var grado = await _context.Grados
                    .FirstOrDefaultAsync(x => x.Nombre == info.Grado);
                
                if (grado is null)
                {
                    grado = new Grados { Nombre = info.Grado };
                    await _context.Grados.AddAsync(grado);
                }

                var seccion = await _context.Secciones
                    .FirstOrDefaultAsync(x => x.Nombre == info.Seccion);
                
                if (seccion is null)
                {
                    seccion = new Secciones { Nombre = info.Seccion };
                    await _context.Secciones.AddAsync(seccion);
                }

                var seccionGrado = await _context.SeccionesGrados
                    .FirstOrDefaultAsync(x => x.GradoId == grado.Id && x.SeccionId == seccion.Id);

                if (seccionGrado is null)
                {
                    seccionGrado = new SeccionesGrados
                    {
                        Grado = grado,
                        Seccion = seccion
                    };
                    await _context.SeccionesGrados.AddAsync(seccionGrado);
                }

                var sector  = await _context.Sectores
                    .FirstOrDefaultAsync(x => x.Nombre == info.Sector);
                
                if (sector is null)
                {
                    sector = new Sectores { Nombre = info.Sector };
                    await _context.Sectores.AddAsync(sector);
                }

                var provincia = await _context.Provincias
                    .FirstOrDefaultAsync(x => x.Nombre == info.Provincia);

                if (provincia is null)
                {
                    provincia = new Provincias { Nombre = info.Provincia };
                    await _context.Provincias.AddAsync(provincia);
                }

                var tanda = await _context.Tandas
                    .FirstOrDefaultAsync(x => x.Nombre == info.Tanda);
                
                if (tanda is null)
                {
                    tanda = new Tandas { Nombre = info.Tanda };
                    await _context.Tandas.AddAsync(tanda);
                }
                
                var escuela = await _context.Escuelas
                    .FirstOrDefaultAsync(x => x.Id == info.CodigoDelCentro);

                if (escuela is null)
                {
                    escuela = new Escuelas
                    {
                        Id = info.CodigoDelCentro,
                        Nombre = info.NombreDelCentro,
                        Sector = sector,
                        Provincia = provincia
                    };
                    await _context.Escuelas.AddAsync(escuela);
                }

                var estudiante = await _context.Estudiantes
                    .FirstOrDefaultAsync(x => x.Matricula == info.Matricula);

                if (estudiante is null)
                {
                    estudiante = new Estudiantes
                    {
                        Matricula = info.Matricula ?? 0,
                        Nombre = info.NombreDelEstudiante,
                        Escuela = escuela,
                        SeccionGrado = seccionGrado
                    };
                    await _context.Estudiantes.AddAsync(estudiante);
                }

                var asignatura = await _context.Asignaturas
                    .FirstOrDefaultAsync(x => x.Nombre.ToLower() == info.Asignatura.ToLower());

                if (asignatura is null)
                {
                    asignatura = new Asignaturas
                    {
                        Nombre = info.Asignatura,
                        Tanda = tanda
                    };
                    await _context.Asignaturas.AddAsync(asignatura);
                }

                var asignatura_estudiante = await _context.AsignaturasEstudiantes
                    .FirstOrDefaultAsync(x => x.EstudianteId == estudiante.Matricula &&
                                              x.AsignaturaId == asignatura.Id);

                if (asignatura_estudiante is null)
                {
                    asignatura_estudiante = new AsignaturasEstudiantes
                    {
                        Estudiante = estudiante,
                        Asignatura = asignatura,
                        Calificacion = info.Calificacion ?? 0,
                        CondicionAcademica = info.CondicionAcademica
                    };
                    await _context.AsignaturasEstudiantes.AddAsync(asignatura_estudiante);
                }
                    

                await _context.SaveChangesAsync();
            }
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