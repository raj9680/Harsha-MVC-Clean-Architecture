using Contacts_Manager.Filters.ActionFilters;
using Contacts_Manager.Filters.AuthorizationFilters;
using Contacts_Manager.Filters.ExceptionFilters;
using Contacts_Manager.Filters.ResourceFilters;
using Contacts_Manager.Filters.ResultFilters;
using Contacts_Manager.Filters.SkipFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Remove.Contacts_Manager.Filters.ActionFilters;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Contacts_Manager.Controllers
{
    [Route("[controller]")]
    // class/controller level filter trigger on each method of current controller
    
    // In case of TypeFilter
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Controller", "My-Value-From-Controller", 2 }, Order = 2)]

    // In case of IFilterFactory
    [ResponseHeaderFilterFactory("My-Key-From-Method", "My-Value-From-Method", 1)]

    //[TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonsAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(IPersonService personService, ICountryService countryService, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _countryService = countryService;
            _logger = logger;
        }


        [Route("/")]
        [Route("[action]")]
        [TypeFilter(typeof(PersonsListActionFilter))]

        // In case of type filter
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Method", "My-Value-From-Method", 1 }, Order = 1)]

        // In case of IFilterFactory
        [ResponseHeaderFilterFactory("My-Key-From-Method", "My-Value-From-Method", 1)]

        // In case of attribute filter
        // [ResponseHeaderActionFilter("My-Key-From-Method", "My-Value-From-Method", 1)]

        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter] // this is filter but we converted that specific filter class to attribute
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");

            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");


            // Implemented in PersonsListActionFilter
            //// Search
            //ViewBag.SearchFields = new Dictionary<string, string>()
            //{
            //    { nameof(PersonResponse.PersonName), "Person Name" },
            //    { nameof(PersonResponse.Email), "Email" },
            //    { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
            //    { nameof(PersonResponse.Gender), "Gender" },
            //    { nameof(PersonResponse.CountryID), "Country" }
            //};


            List<PersonResponse> persons = await _personService.GetFilteredPersons(searchBy, searchString);
            
            // Implemented in PersonsListActionFilter
            //ViewBag.CurrentSearchBy = searchBy;
            //ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = await _personService.GetSortedPerson(persons, sortBy, sortOrder);

            // Implemented in PersonsListActionFilter
            //ViewBag.CurrentSortBy = sortBy;
            //ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons); //Views/Persons/Index.cshtml
        }


        // Executes when user click create person
        [Route("[action]")]
        [HttpGet]
        // In case of TypeFilter
        // [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "Y-Custom-Key", "Y-Custom-Value", 1})]
        
        // In case of IFilterFactory
        // In case of IFilterFactory
        [ResponseHeaderFilterFactory("My-Key-From-Method", "My-Value-From-Method", 1)]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countryService.GetAllCountries();
            // Options 1
            // ViewBag.Countries = countries;

            // Option 2 - recommended - SelectListItem() type 
            // new SelectListItem() { Text="Raj", Value="1" };
            // <option value="1">Raj</option>  // So for ViewBag.Countries we can use this as

            ViewBag.Countries = countries.Select(x =>
                new SelectListItem()
                {
                    Text = x.CountryName,
                    Value = x.CountryID.ToString()
                }
            );

            return View();
        }


        // Executes when submit form to create user
        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter))] // isDisabled is bydefaul true
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            // Below has been implemenetd on ActionFilter i.e PersonCreateAndEditPostActionFilter

            //if (!ModelState.IsValid)
            //{
            //    List<CountryResponse> countries = await _countryService.GetAllCountries();
            //    // Options 1
            //    // ViewBag.Countries = countries;

            //    // Option 2 - recommended - SelectListItem() type 
            //    // new SelectListItem() { Text="Raj", Value="1" };
            //    // <option value="1">Raj</option>  // So for ViewBag.Countries we can use this as

            //    ViewBag.Countries = countries.Select(x =>
            //        new SelectListItem()
            //        {
            //            Text = x.CountryName,
            //            Value = x.CountryID.ToString()
            //        }
            //    );

            //    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    return View(personRequest);
            //}

            PersonResponse personResponse = await _personService.AddPerson(personRequest);

            // navigate to index action method
            return RedirectToAction("Index", "Persons");
        }


        [HttpGet]
        [Route("[action]/{personID}")] // Eg: /persons/edit/1
        [TypeFilter(typeof(TokenResultFilter))] // To Set Auth Cookies
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);

            if (personResponse == null)
            {
                return RedirectToAction("index");
            }

            // Converting personResponse to persoUpdateRequest
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            // Populating Countries List
            List<CountryResponse> countries = await _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            return View(personUpdateRequest);
        }


        [HttpPost]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("index");
            }

            // Validation is getting checked in Filter
            //if (ModelState.IsValid)
            //{

            // uncommnet to trigger InvalidPersonIDException, ExceptionClass Library
            // personRequest.PersonID = Guid.NewGuid();
                PersonResponse updatedPerson = await _personService.UpdatePerson(personRequest);
                return RedirectToAction("Index");
            
            //}
            //else
            //{
            //    // Populating Countries List
            //    List<CountryResponse> countries = await _countryService.GetAllCountries();
            //    ViewBag.Countries = countries.Select(temp =>
            //    new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            //    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            //    return View(personResponse.ToPersonUpdateRequest());
            //}
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid personID)
        {
            if (!String.IsNullOrEmpty(personID.ToString()))
            {
                PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);
                return View(personResponse);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest person)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(person.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            await _personService.DeletePerson(person.PersonID);
            return View();
        }


        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            // Get list of persons
            List<PersonResponse> persons = await _personService.GetAllPersons();
            // ViewData, if any ViewData object is there in controller
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                // bydefault portrait/landscape page orientation
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }


        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personService.GetPersonCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }



        [Route("[action]")]
        public async Task<IActionResult> PersonsCSVCustomFields()
        {
            MemoryStream memoryStream = await _personService.GetPersonCSVCustomFields();
            return File(memoryStream, "application/octet-stream", "persons-custom-fields.csv");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personService.GetPersonExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons-custom-fields.xlsx");
        }
    }
}
