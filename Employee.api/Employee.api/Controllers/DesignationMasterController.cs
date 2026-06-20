using Employee.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesignationMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public DesignationMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: api/designation
        [HttpGet]
        public async Task<IActionResult> GetAllDesignations()
        {
            try
            {
                var designations = await _context.Set<Designation>().ToListAsync();

                return Ok(designations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while fetching designations",
                    error = ex.Message
                });
            }
        }

        // GET: api/designation/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignationById(int id)
        {
            try
            {
                var designation = await _context.Set<Designation>()
                    .FirstOrDefaultAsync(x => x.designationId == id);

                if (designation == null)
                {
                    return NotFound(new
                    {
                        message = "Designation not found"
                    });
                }

                return Ok(designation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while fetching designation",
                    error = ex.Message
                });
            }
        }

        // POST: api/designation
        [HttpPost]
        public async Task<IActionResult> CreateDesignation([FromBody] Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _context.Set<Designation>().AddAsync(designation);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Designation created successfully",
                    data = designation
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while creating designation",
                    error = ex.Message
                });
            }
        }

        // PUT: api/designation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDesignation(int id, [FromBody] Designation designation)
        {
            try
            {
                if (id != designation.designationId)
                {
                    return BadRequest(new
                    {
                        message = "Designation ID mismatch"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDesignation = await _context.Set<Designation>()
                    .FirstOrDefaultAsync(x => x.designationId == id);

                if (existingDesignation == null)
                {
                    return NotFound(new
                    {
                        message = "Designation not found"
                    });
                }

                existingDesignation.designationName = designation.designationName;
                existingDesignation.departmentId = designation.departmentId;

                _context.Set<Designation>().Update(existingDesignation);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Designation updated successfully",
                    data = existingDesignation
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating designation",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/designation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            try
            {
                var designation = await _context.Set<Designation>()
                    .FirstOrDefaultAsync(x => x.designationId == id);

                if (designation == null)
                {
                    return NotFound(new
                    {
                        message = "Designation not found"
                    });
                }

                _context.Set<Designation>().Remove(designation);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Designation deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while deleting designation",
                    error = ex.Message
                });
            }
        }

        // FILTER API
        // Example:
        // api/designation/filter?designationName=manager&departmentId=1

        [HttpGet("filter")]
        public async Task<IActionResult> FilterDesignation(
            string? designationName,
            int? departmentId)
        {
            try
            {
                var query = _context.Set<Designation>().AsQueryable();

                if (!string.IsNullOrEmpty(designationName))
                {
                    query = query.Where(x =>
                        x.designationName.Contains(designationName));
                }

                if (departmentId.HasValue)
                {
                    query = query.Where(x =>
                        x.departmentId == departmentId.Value);
                }

                var result = await query.ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while filtering designations",
                    error = ex.Message
                });
            }
        }
    }
}
