using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts_Manager.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;
        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // To Do Before Logic
            _logger.LogInformation("{Filtername}.{Methodname} - before", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));

            await next();

            // To Do After Logic
            _logger.LogInformation("{Filtername}.{Methodname} - after");

            // Last minute changes to Result 
            // context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
