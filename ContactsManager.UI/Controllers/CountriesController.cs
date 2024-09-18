using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace Contacts_Manager.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountryService _countryService;
        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }


        [Route("[action]")]
        [HttpGet]
        public IActionResult UploadFromExcel()
        {
            return View();
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. '.xlsx' file expected.";
                return View();
            }

            int numberOfCountriesInserted = await _countryService.UploadCountriesFromExcelFile(excelFile);

            ViewBag.Message = $"{numberOfCountriesInserted} Countries Uploaded";
            return View();
        }
    }
}
