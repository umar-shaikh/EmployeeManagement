using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Employee.api.Models;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public DepartmentMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllDepartments")]
        public IActionResult GetAllDepartments()
        {
            var departments = _context.Departments.ToList();
            return Ok(departments);
        }
        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] Department dept)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if department name already exists
                bool departmentExists = _context.Departments
                    .Any(x => x.departmentName.ToLower() == dept.departmentName.ToLower());

                if (departmentExists)
                {
                    return BadRequest(new
                    {
                        message = "Department name already exists"
                    });
                }

                _context.Departments.Add(dept);
                _context.SaveChanges();

                return Created("Department added successfully" , dept);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while adding department",
                    error = ex.Message
                });
            }
        }


        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] Department dept)
        {
            var existingDept = _context.Departments.Find(dept.departmentId);
            if (existingDept == null)
            {
                return NotFound();
            }

            existingDept.departmentName = dept.departmentName;
            existingDept.isActive = dept.isActive;
            // Update other properties as needed

            _context.SaveChanges();
            return Created("Department updated successfully",dept);
        }

        [HttpDelete("DeleteDepartment/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept == null)
            {
                return NotFound("Department not found");
            }
            _context.Departments.Remove(dept);
            _context.SaveChanges();
            return Created("Department deleted successfully", dept);


        }
    }
}

