using ApiPeliculas.Models;
using System.Collections.Generic;

namespace ApiPeliculas.Repository.IRepository
{
    public interface ICategoriaRepository
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int idCategoria);
        bool ExisteCategoria(string nombreCategoria);
        bool ExisteCategoria(int idCategoria);
        bool CrearCategoria(Categoria nuevaCategoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool Guardar();

    }
}
