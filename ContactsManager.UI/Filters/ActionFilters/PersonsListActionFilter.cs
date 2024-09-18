using Contacts_Manager.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Contacts_Manager.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Executes after action method execution
            _logger.LogInformation("PersonsListActionFilter.OnActionExecuted method");

            // Getting ViewData value 
            PersonsController personsController = (PersonsController)context.Controller;

            var parameters = context.HttpContext.Items["myArg"] as IDictionary<string, object>;

            // var parameter = context.HttpContext.Items;

            if (parameters != null)
            {
                if (parameters.ContainsKey("searchBy"))
                {
                    personsController.ViewBag.CurrentSearchBy = Convert.ToString(parameters["searchBy"]);
                }

                if (parameters.ContainsKey("searchString"))
                {
                    personsController.ViewBag.CurrentSearchString = Convert.ToString(parameters["searchString"]);
                }

                if (parameters.ContainsKey("sortBy"))
                {
                    personsController.ViewBag.CurrentSortBy = Convert.ToString(parameters["sortBy"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortBy"] = nameof(PersonResponse.PersonName);
                }

                if (parameters.ContainsKey("sortOrder"))
                {
                    personsController.ViewBag.CurrentSortOrder = Convert.ToString(parameters["sortOrder"]);
                }
                else
                {
                    personsController.ViewBag.CurrentSortOrder = nameof(SortOrderOptions.ASC);
                }
            }

            //Search
            personsController.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" }
            };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Executes before action method execution
            _logger.LogInformation("PersonsListActionFilter.OnActionExecuting method");

            context.HttpContext.Items["myArg"] = context.ActionArguments;

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName), 
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Country)
                    };

                    // if user supply value other than searchOptions, default searchBy value will set to PersonName
                    if(searchOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("searchBy value: {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation(Convert.ToString(context.ActionArguments["searchBy"]));
                    }
                }
            }
        }
    }
}
