using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ApiPeliculas.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            _db.Categoria.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _db.Categoria.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            _db.Categoria.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombreCategoria)
        {
            bool existe = _db.Categoria.Any(c => c.Nombre.ToLower().Trim() == nombreCategoria.ToLower().Trim());
            return existe;
        }

        public bool ExisteCategoria(int idCategoria)
        {
            return _db.Categoria.Any(c => c.Id == idCategoria);
        }

        public Categoria GetCategoria(int idCategoria)
        {
            return _db.Categoria.FirstOrDefault(c => c.Id == idCategoria);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _db.Categoria.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
