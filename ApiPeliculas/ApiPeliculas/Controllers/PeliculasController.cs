using ApiPeliculas.Models;
using ApiPeliculas.Models.DTOs;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApiPeliculas.Controllers
{
    [Route("api/Peliculas")]
    [ApiController]
    public class PeliculasController : Controller
    {
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepository peliculaRepository, IWebHostEnvironment hostingEnvironment, IMapper mapper)
        {
            _peliculaRepository = peliculaRepository;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _peliculaRepository.GetPeliculas();

            var listaPeliculasDTO = new List<PeliculaDTO>();

            foreach (var item in listaPeliculas)
                listaPeliculasDTO.Add(_mapper.Map<PeliculaDTO>(item));

            return Ok(listaPeliculasDTO);
        }

        [HttpGet("{idPelicula:int}", Name = "GetPelicula")]
        public IActionResult GetPelicula(int idPelicula)
        {
            var Pelicula = _peliculaRepository.GetPelicula(idPelicula);

            if (Pelicula == null)
                return NotFound();

            var PeliculaDTO = _mapper.Map<PeliculaDTO>(Pelicula);
            return Ok(PeliculaDTO);
        }

        [HttpGet("GetPeliculasEnCategoria/{idCategoria:int}")]
        public IActionResult GetPeliculasEnCategoria(int idCategoria)
        {
            var listaPeliculas = _peliculaRepository.GetPeliculasEnCategoria(idCategoria);

            if (listaPeliculas == null)
                return NotFound();

            var peliculas = new List<PeliculaDTO>();
            foreach (var p in listaPeliculas)
            {
                peliculas.Add(_mapper.Map<PeliculaDTO>(p));
            }

            return Ok(peliculas);
        }

        [HttpGet("BuscarPelicula")]
        public IActionResult Buscar(string nombrePelicula)
        {
            try
            {
                var resultado = _peliculaRepository.BuscarPelicula(nombrePelicula);
                if (resultado.Any())
                    return Ok(resultado);

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos.");
            }
        }

        [HttpPost]
        public IActionResult CrearPelicula([FromForm] PeliculaCreateDTO peliculaDTO)
        {
            if (peliculaDTO == null)
                return BadRequest(ModelState);

            if (_peliculaRepository.ExistePelicula(peliculaDTO.Nombre))
            {
                ModelState.AddModelError("", "La película ya existe.");
                return StatusCode(404, ModelState);
            }

            // Subida de archivo

            var archivo = peliculaDTO.UrlImg;
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length > 0)
            {
                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                peliculaDTO.UrlImg = @"\fotos\" + nombreFoto + extension;
            }    

            var pelicula = _mapper.Map<Pelicula>(peliculaDTO);

            if (!_peliculaRepository.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal al crear el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { idPelicula = pelicula.Id }, pelicula);
        }

        [HttpPut("{idPelicula:int}", Name = "ActualizarPelicula")]
        public IActionResult ActualizarPelicula(int idPelicula, [FromBody]PeliculaUpdateDTO peliculaDTO)
        {
            if (peliculaDTO == null || idPelicula != peliculaDTO.Id)
                return BadRequest(ModelState);

            var pelicula = _mapper.Map<Pelicula>(peliculaDTO);

            if (!_peliculaRepository.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal al actualizar el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{idPelicula:int}", Name = "BorrarPelicula")]
        public IActionResult BorrarPelicula(int idPelicula)
        {
            if (!_peliculaRepository.ExistePelicula(idPelicula))
                return NotFound();

            var pelicula = _peliculaRepository.GetPelicula(idPelicula);

            if (!_peliculaRepository.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal al borrar el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
