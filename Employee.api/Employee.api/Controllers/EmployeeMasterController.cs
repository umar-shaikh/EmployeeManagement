using Employee.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public EmployeeMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL EMPLOYEES
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees.ToListAsync();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error fetching employees",
                    error = ex.Message
                });
            }
        }

        // =========================
        // GET EMPLOYEE BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(x => x.employeeId == id);

                if (employee == null)
                {
                    return NotFound(new
                    {
                        message = "Employee not found"
                    });
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error fetching employee",
                    error = ex.Message
                });
            }
        }

        // =========================
        // ADD EMPLOYEE
        // =========================
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeModel emp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool contactExists = await _context.Employees
                    .AnyAsync(x => x.contactNo == emp.contactNo);

                if (contactExists)
                {
                    return BadRequest(new
                    {
                        message = "Contact number already exists"
                    });
                }

                bool emailExists = await _context.Employees
                    .AnyAsync(x => x.email.ToLower() == emp.email.ToLower());

                if (emailExists)
                {
                    return BadRequest(new
                    {
                        message = "Email already exists"
                    });
                }

                // Mapping DTO to Entity
                var employee = new EmployeeModel
                {
                    name = emp.name,
                    contactNo = emp.contactNo,
                    email = emp.email,
                    city = emp.city,
                    state = emp.state,
                    pincode = emp.pincode,
                    altContactNo = emp.altContactNo,
                    address = emp.address,
                    designationId = emp.designationId,
                    createdDate = DateTime.Now,
                    modifiedDate = DateTime.Now
                };

                await _context.Employees.AddAsync(employee);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Employee added successfully",
                    data = employee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error adding employee",
                    error = ex.Message
                });
            }
        }

        // =========================
        // UPDATE EMPLOYEE
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeModel emp)
        {
            try
            {
                if (id != emp.employeeId)
                {
                    return BadRequest(new
                    {
                        message = "Employee ID mismatch"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(x => x.employeeId == id);

                if (existingEmployee == null)
                {
                    return NotFound(new
                    {
                        message = "Employee not found"
                    });
                }

                // Unique contact number check
                bool contactExists = await _context.Employees
                    .AnyAsync(x => x.contactNo == emp.contactNo &&
                                   x.employeeId != id);

                if (contactExists)
                {
                    return BadRequest(new
                    {
                        message = "Contact number already exists"
                    });
                }

                // Unique email check
                bool emailExists = await _context.Employees
                    .AnyAsync(x => x.email.ToLower() == emp.email.ToLower() &&
                                   x.employeeId != id);

                if (emailExists)
                {
                    return BadRequest(new
                    {
                        message = "Email already exists"
                    });
                }

                existingEmployee.name = emp.name;
                existingEmployee.contactNo = emp.contactNo;
                existingEmployee.email = emp.email;
                existingEmployee.city = emp.city;
                existingEmployee.state = emp.state;
                existingEmployee.pincode = emp.pincode;
                existingEmployee.altContactNo = emp.altContactNo;
                existingEmployee.address = emp.address;
                existingEmployee.designationId = emp.designationId;
                existingEmployee.modifiedDate = DateTime.Now;

                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Employee updated successfully",
                    data = existingEmployee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error updating employee",
                    error = ex.Message
                });
            }
        }

        // =========================
        // DELETE EMPLOYEE
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(x => x.employeeId == id);

                if (employee == null)
                {
                    return NotFound(new
                    {
                        message = "Employee not found"
                    });
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Employee deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error deleting employee",
                    error = ex.Message
                });
            }
        }

        // ==================================================
        // FILTER + SORT + PAGINATION API
        // ==================================================
        // Example:
        // api/employee/filter?name=umar&city=pune&pageNumber=1&pageSize=5&sortBy=name&sortOrder=asc
        // ==================================================

        [HttpGet("filter")]
        public async Task<IActionResult> FilterEmployees(
            string? name,
            string? city,
            string? state,
            int? designationId,
            string? sortBy = "employeeId",
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var query = _context.Employees.AsQueryable();

                // FILTERING

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x =>
                        x.name.Contains(name));
                }

                if (!string.IsNullOrEmpty(city))
                {
                    query = query.Where(x =>
                        x.city.Contains(city));
                }

                if (!string.IsNullOrEmpty(state))
                {
                    query = query.Where(x =>
                        x.state.Contains(state));
                }

                if (designationId.HasValue)
                {
                    query = query.Where(x =>
                        x.designationId == designationId.Value);
                }

                // SORTING

                switch (sortBy?.ToLower())
                {
                    case "name":
                        query = sortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.name)
                            : query.OrderBy(x => x.name);
                        break;

                    case "city":
                        query = sortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.city)
                            : query.OrderBy(x => x.city);
                        break;

                    case "createddate":
                        query = sortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.createdDate)
                            : query.OrderBy(x => x.createdDate);
                        break;

                    default:
                        query = sortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(x => x.employeeId)
                            : query.OrderBy(x => x.employeeId);
                        break;
                }

                // PAGINATION

                var totalRecords = await query.CountAsync();

                var employees = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    totalRecords,
                    pageNumber,
                    pageSize,
                    data = employees
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error filtering employees",
                    error = ex.Message
                });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var email = login.email.Trim().ToLower();
                var contact = login.contactNo.Trim(); 

                var employee = await _context.Employees
                    .FirstOrDefaultAsync(x =>
                        x.email.Trim().ToLower() == email &&
                        x.contactNo.Trim() == contact);

                if (employee == null)
                {
                    return Unauthorized(new
                    {
                        message = "Invalid email or contact number"
                    });
                }

                return Ok(new
                {
                    message = "Login successful",
                    data = new
                    {
                           employee.employeeId,
                            employee.name,
                                employee.email,
                                    employee.contactNo,
                                    employee.role,
                                    employee.designationId,
                                   
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error during login",
                    error = ex.Message
                });
            }
        }
    }
}
