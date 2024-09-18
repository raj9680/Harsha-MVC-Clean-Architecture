using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// IMPORTANT: Implementing IFilterFactory
// Better version of both ActionFilter & AttributeFilter i.e. IFilterFactory

namespace Contacts_Manager.Filters.ActionFilters
{
    // Need to create an extra class for IFilterFactory
    // We can use DI in IFilterFactory class, in AttributeFilter we can't
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        //private readonly ILogger<ResponseHeaderActionFilter> _log;
        private string? _value { get; set; }
        private string? _key { get; set; }
        private int _order { get; set; }

        public ResponseHeaderFilterFactoryAttribute(string key,
            string value,
            int order
            //ILogger<ResponseHeaderActionFilter> log
            )
        {
            _key = key;
            _value = value;
            _order = order;
            // _log = log;
        }

        public bool IsReusable => false;

        // Process, Controller -> FilterFactory -> Filter
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            // Implemeting DI, using service provider
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();

            //var filter = new ResponseHeaderActionFilter(_key, _value, _order);

            // assign property directly here
            filter._keyy = _key;
            filter._valuee = _value;
            filter.Order = _order;
            // Here we have to return FilterObject
            return filter;
        }
    }


    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        // need to convert the private fields to public properties to use DI
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public string _keyy { get; set; }
        public string _valuee { get; set; }

        public int Order { get; set; }

        // keep the constructor parameterless to use DI
        public ResponseHeaderActionFilter(
             ILogger<ResponseHeaderActionFilter> logger
            //string key,
            //string value,
            //int order
            )
        {
            _logger = logger;
            //_keyy = key;
            //_valuee = value;
            //Order = order;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // code from OnActionExecuting()
            _logger.LogInformation("{FilterName}.{MethodName} before method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));


            await next(); // calls the subsequent filter or action method



            // code from OnActionExecuted()
            context.HttpContext.Response.Headers[_keyy] = _valuee;
            // _logger.LogInformation("{FilterName}.{MethodName} after method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
        }
    }
}
