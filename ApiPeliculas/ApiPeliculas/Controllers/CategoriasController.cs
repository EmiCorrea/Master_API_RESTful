using ApiPeliculas.Models;
using ApiPeliculas.Models.DTOs;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Controllers
{
    [Route("api/Categorias")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _categoriaRepository.GetCategorias();

            var listaCategoriasDTO = new List<CategoriaDTO>();

            foreach (var item in listaCategorias)
                listaCategoriasDTO.Add(_mapper.Map<CategoriaDTO>(item));

            return Ok(listaCategoriasDTO);
        }

        [HttpGet("{idCategoria:int}", Name = "GetCategoria")]
        public IActionResult GetCategoria(int idCategoria)
        {
            var categoria = _categoriaRepository.GetCategoria(idCategoria);

            if (categoria == null)
                return NotFound();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDTO);
        }

        [HttpPost]
        public IActionResult CrearCategoria([FromBody] CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null)
                return BadRequest(ModelState);

            if (_categoriaRepository.ExisteCategoria(categoriaDTO.Nombre))
            {
                ModelState.AddModelError("", "La categoría ya existe.");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (!_categoriaRepository.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal al crear el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { idCategoria = categoria.Id }, categoria);
        }

        [HttpPut("{idCategoria:int}", Name = "ActualizarCategoria")]
        public IActionResult ActualizarCategoria(int idCategoria, [FromBody]CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null || idCategoria != categoriaDTO.Id)
                return BadRequest(ModelState);

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (!_categoriaRepository.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal al actualizar el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{idCategoria:int}", Name = "BorrarCategoria")]
        public IActionResult BorrarCategoria(int idCategoria)
        {
            if (!_categoriaRepository.ExisteCategoria(idCategoria))
                return NotFound();

            var categoria = _categoriaRepository.GetCategoria(idCategoria);

            if (!_categoriaRepository.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal al borrar el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
