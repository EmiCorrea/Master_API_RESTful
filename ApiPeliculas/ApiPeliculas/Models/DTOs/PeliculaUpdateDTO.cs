using System.ComponentModel.DataAnnotations;
using static ApiPeliculas.Models.Pelicula;

namespace ApiPeliculas.Models.DTOs
{
    public class PeliculaUpdateDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }
        public int categoriaId { get; set; }
    }
}
