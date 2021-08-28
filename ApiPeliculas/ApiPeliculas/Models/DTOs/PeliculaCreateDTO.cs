using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static ApiPeliculas.Models.Pelicula;

namespace ApiPeliculas.Models.DTOs
{
    public class PeliculaCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        public string UrlImg { get; set; }
        public IFormFile Foto { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La duración es obligatoria.")]
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }
        public int categoriaId { get; set; }
    }
}
