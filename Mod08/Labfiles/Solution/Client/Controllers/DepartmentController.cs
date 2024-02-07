using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Client.Controllers
{
    public class DepartmentController : Controller
    {
        private IHttpClientFactory _httpClientFactory;

        public DepartmentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:54517");
            HttpResponseMessage response = await httpClient.GetAsync("api/Department");
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Department> departments = await response.Content.ReadAsAsync<IEnumerable<Department>>();
                return View(departments);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
