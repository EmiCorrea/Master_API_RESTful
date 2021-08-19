using System;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es oblogatorio")]
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }

    }
}
