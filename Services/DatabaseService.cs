using System.Collections.Generic;
using System.Threading.Tasks;
using Iso810.Entities;
using Iso810.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Iso810.Services
{
    public class DatabaseService : IDatabaseService
    {
        private MainContext _context;

        public DatabaseService(MainContext context)
        {
            _context = context;
        }

        public async Task SaveData(List<StudentsView> source)
        {
            foreach (var info in source)
            {
                var grado = await _context.Grados
                    .FirstOrDefaultAsync(x => x.Nombre.ToLower() == info.Grado.ToLower());
                
                if (grado is null)
                {
                    grado = new Grados { Nombre = info.Grado };
                    await _context.Grados.AddAsync(grado);
                }

                var seccion = await _context.Secciones
                    .FirstOrDefaultAsync(x => x.Nombre.ToLower() == info.Seccion.ToLower());
                
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
                    .FirstOrDefaultAsync(x => x.Nombre.ToLower() == info.Sector.ToLower());
                
                if (sector is null)
                {
                    sector = new Sectores { Nombre = info.Sector };
                    await _context.Sectores.AddAsync(sector);
                }

                var provincia = await _context.Provincias
                    .FirstOrDefaultAsync(x => x.Nombre.ToLower() == info.Provincia.ToLower());

                if (provincia is null)
                {
                    provincia = new Provincias { Nombre = info.Provincia };
                    await _context.Provincias.AddAsync(provincia);
                }

                var tanda = await _context.Tandas
                    .FirstOrDefaultAsync(x => x.Nombre.ToLower() == info.Tanda.ToLower());
                
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
                    .FirstOrDefaultAsync(x => x.Nombre.ToLower().ToLower() == info.Asignatura.ToLower());

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
            }
            
            await _context.SaveChangesAsync();
        }
    }
}