using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Website.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private SchoolContext _context;

        public MeetingController(SchoolContext context)
        {
            _context = context;
        }

        [Route("{id:int}")]
        public ActionResult<Meeting> GetById(int id)
        {
            var order = _context.Meetings.FirstOrDefault(p => p.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public ActionResult<Meeting> Create(Meeting meeting)
        {
            _context.Add(meeting);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = meeting.Id }, meeting);
        }
    }
}
