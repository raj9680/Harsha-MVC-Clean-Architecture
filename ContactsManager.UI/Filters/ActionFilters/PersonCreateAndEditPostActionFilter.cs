using Contacts_Manager.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Contacts_Manager.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountryService _countryService;
        public PersonCreateAndEditPostActionFilter(ICountryService countryService)
        {
            _countryService = countryService;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // To Do Before Logic
            if(context.Controller is PersonsController personsController) // To Get ModelState
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await _countryService.GetAllCountries();
                    // Options 1
                    // ViewBag.Countries = countries;

                    // Option 2 - recommended - SelectListItem() type 
                    // new SelectListItem() { Text="Raj", Value="1" };
                    // <option value="1">Raj</option>  // So for ViewBag.Countries we can use this as

                    personsController.ViewBag.Countries = countries.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.CountryName,
                            Value = x.CountryID.ToString()
                        }
                    );

                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    // getting action method custom argument, model
                    // its works for both create & update post, bcoz parameter name is same in both action method i.e personRequest
                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result = personsController.View(personRequest); 
                    
                    // short circuits or skips the subsequent filters & action method,
                    // To short circuit ActionFilter we should need to return any type of IActionResult
                    // Not calling next(), so it leads remaining method & action filter short circuit.
                    // Even calling next(), after returning any type of IActionResult, it is also consider as short circuit.
                }
                else
                {
                    await next(); // call the subsequent filter or action method
                    // To Do After Logic
                }
            }
            else
            {
                await next(); // call the subsequent filter or action method
                              // To Do After Logic
            }
        }
    }
}
