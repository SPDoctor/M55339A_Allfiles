using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Website.Models;

namespace Client.Controllers
{
    public class MeetingController : Controller
    {
        private IHttpClientFactory _httpClientFactory;

        public MeetingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDepartmentsDropDownListAsync();
            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePostAsync(Meeting meeting)
        {
            HttpClient httpclient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpclient.PostAsJsonAsync("http://localhost:54517/api/Meeting", meeting);
            if (response.IsSuccessStatusCode)
            {
                Meeting resMeeting = await response.Content.ReadAsAsync<Meeting>();
                return RedirectToAction("ThankYou", new { id = resMeeting.Id });
            }
            else
            {
                return View("Error");
            }
        }

        private async Task PopulateDepartmentsDropDownListAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:54517");
            HttpResponseMessage response = await httpClient.GetAsync("api/Department");
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Department> departments = await response.Content.ReadAsAsync<IEnumerable<Department>>();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
            }
        }

        public async Task<IActionResult> ThankYou(int id)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:54517");
            HttpResponseMessage response = await httpClient.GetAsync("api/Meeting/" + id);
            if (response.IsSuccessStatusCode)
            {
                Meeting meeting = await response.Content.ReadAsAsync<Meeting>();
                return View(meeting);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
