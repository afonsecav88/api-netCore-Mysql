﻿
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mysqlapi.Interfaces;
using mysqlapi.Models;

namespace mysqlapi.Controllers
{
    /*[ApiConventionType(typeof(DefaultApiConventions))]*/
    [ApiController]
    [Route("api/v{version:apiVersion}/StudentDetail")]
    [ApiVersion("1.0")]
    [Authorize]
    public class StudentDetailsController : ControllerBase
    {

        private readonly IStudentDetail _studentDetail;
        private readonly IMapper _mapper;

        public StudentDetailsController(IStudentDetail studentDetail, IMapper mapper)
        {
            _studentDetail = studentDetail;
            _mapper = mapper;
        }

        /// <summary>
        /// Crea un nuevo estudiante
        /// </summary>
        /// <param name="studentDetail"></param>
        /// <returns></returns>

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<StudentDetail>> CreateStudentDetail(StudentDetail studentDetail)
        {
            if (studentDetail == null) return BadRequest(ModelState);

            var result = await _studentDetail.CreateStudentDetail(studentDetail);

            if (!result) return BadRequest(new { mensaje = "Su estudiante no ha sido insertado " });

            else
            {
                return CreatedAtRoute("getStudentById", new { id = studentDetail.Id }, studentDetail);
            }
        }
        //Agregado para la documentacion xml
        /// <summary>
        /// Retorna todos los Estudiantes
        /// </summary>
        /// <returns></returns>
        [HttpGet("StudentDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<StudentDetailDto>>> GetStudentDetails()
        {
            var result = await _studentDetail.GetStudentDetails();

            if (result == null) return NotFound("No existen Estudiantes");

            var resultDto = new List<StudentDetailDto>();

            foreach (var item in result)
            {
                resultDto.Add(_mapper.Map<StudentDetailDto>(item));
            }

            return Ok(resultDto);
        }

        /// <summary>
        /// Busca un estudiante basandose en su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "getStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDetailDto>> GetStudentDetail(int id)
        {
            var result = await _studentDetail.GetStudentDetailById(id);

            if (result == null) return NotFound();

            var resultDto = _mapper.Map<StudentDetailDto>(result);

            return Ok(resultDto);
        }


        /// <summary>
        /// Elimina un estudiante basado en su id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "deleteStudentDetails")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                             nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteStudentDetail(int id)
        {
            var result = await _studentDetail.GetStudentDetailById(id);

            if (result == null) return BadRequest();

            else await _studentDetail.DeleteStudentDetail(result);

            return Ok(new
            {
                status = "terminated",
                message = "The student has been eliminated"
            });
        }

        /// <summary>
        /// Actualiza los datos de un estudiante basado en su id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentDetail"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "updateStudentDetails")]
        /*[ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]*/
        [ApiConventionMethod(typeof(DefaultApiConventions),
                             nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> PutStudentDetail(int id, StudentDetail studentDetail)
        {
            if (id != studentDetail.Id) return BadRequest();

            if (!await _studentDetail.StudentDetailExists(id)) return NotFound();

            try
            {
                await _studentDetail.UpdateStudentDetail(studentDetail, id);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }
    }
}
