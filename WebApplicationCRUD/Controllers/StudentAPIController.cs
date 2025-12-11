using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationCRUD.Data;
using WebApplicationCRUD.Models;

namespace WebApplicationCRUD.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly MyDbContext context;

        public StudentAPIController(MyDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            var students = await context.Students.ToListAsync();
           
            return Ok(students);
        }
        [HttpGet("{ID}")]
        public async Task<ActionResult<Student>> GetStudentsByID(int ID)
        {
            var student = await context.Students.FindAsync(ID);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }
        [HttpPost]
        public async Task<ActionResult> CreateStudent(Student student)
        {
           await context.Students.AddAsync(student);
            await context.SaveChangesAsync();
            // return CreatedAtAction(nameof(GetStudentsByID), new { ID = student.Id }, student);
            return Ok(student);
        }
        private bool StudentExists(int id)
        {
            return context.Students.Any(e => e.Id == id);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateStudent(int ID, [FromBody] Student student)
        {
            if (ID != student.Id)
            {
                return BadRequest();
            }
            context.Entry(student).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!StudentExists(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpDelete("{ID}")]
        public async Task<ActionResult<Student>> DeleteStudent(int ID)
        {
            var student = await context.Students.FindAsync(ID);
          if(student == null)
            {
                return NotFound();
            }
            // context.Students.Remove(student);
            context.Entry(student).State = EntityState.Deleted;
            try
            {
                await context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }


    } 
}