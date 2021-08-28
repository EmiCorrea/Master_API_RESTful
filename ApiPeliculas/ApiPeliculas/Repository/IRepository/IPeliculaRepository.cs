using ApiPeliculas.Models;
using System.Collections.Generic;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IPeliculaRepository
    {
        ICollection<Pelicula> GetPeliculas();
        ICollection<Pelicula> GetPeliculasEnCategoria(int idCategoria);
        Pelicula GetPelicula(int idPelicula);
        bool ExistePelicula(string nombrePelicula);
        bool ExistePelicula(int idPelicula);
        IEnumerable<Pelicula> BuscarPelicula(string nombrePelicula);
        bool CrearPelicula(Pelicula nuevaPelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);
        bool Guardar();

    }
}
