using System.ComponentModel.DataAnnotations;
using static ApiPeliculas.Models.Pelicula;

namespace ApiPeliculas.Models.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La URL de imagen es obligatoria.")]
        public string UrlImg { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La duración es obligatoria.")]
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }
        public int categoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
