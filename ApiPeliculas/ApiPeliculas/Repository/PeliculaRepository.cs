using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ApiPeliculas.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly ApplicationDbContext _db;

        public PeliculaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            _db.Pelicula.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _db.Pelicula.Remove(pelicula);
            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _db.Pelicula;

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));

            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            _db.Pelicula.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombrePelicula)
        {
            bool existe = _db.Pelicula.Any(p => p.Nombre.ToLower().Trim() == nombrePelicula.ToLower().Trim());
            return existe;
        }

        public bool ExistePelicula(int idPelicula)
        {
            return _db.Pelicula.Any(p => p.Id == idPelicula);
        }

        public Pelicula GetPelicula(int idPelicula)
        {
            return _db.Pelicula.FirstOrDefault(p => p.Id == idPelicula);
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int idCategoria)
        {
            return _db.Pelicula.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == idCategoria).ToList();
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _db.Pelicula.OrderBy(p => p.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
