using Contacts_Manager.Filters.SkipFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts_Manager.Filters.ResultFilters
{
    // This filter triggers always even after any filter short circuits
    public class PersonsAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        private readonly ILogger<PersonsAlwaysRunResultFilter> _logger;

        public PersonsAlwaysRunResultFilter(ILogger<PersonsAlwaysRunResultFilter> logger)
        {
            _logger = logger;
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogInformation("Always Run Action Filter After");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // skipping this execution for index method
            if (context.Filters.OfType<SkipFilter>().Any())
            {
                // Wherever the skip filter attribute is used in action method, that method is skipped.
                return;
            } // remaining portion of code is skipped.
            _logger.LogInformation("Always Run Action Filter Before");
        }
    }
}
