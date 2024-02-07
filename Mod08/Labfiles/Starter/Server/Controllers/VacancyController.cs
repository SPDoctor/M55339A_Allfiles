using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Website.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private SchoolContext _context;

        public VacancyController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<EmployeeRequirements>> Get()
        {
            var requirements = from r in _context.EmployeesRequirements
                               orderby r.JobTitle
                               select r;
            return requirements.ToList();
        }
    }
}