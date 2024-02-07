using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Website.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private SchoolContext _context;

        public DepartmentController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Department>> Get()
        {
            var branches = from r in _context.Departments
                           orderby r.Name
                           select r;
            return branches.ToList();
        }
    }
}
