using Microsoft.AspNetCore.Mvc.Filters;

// Differentiating between ActionFilter & AttributeFilter

namespace Remove.Contacts_Manager.Filters.ActionFilters
{
    //public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    public class ResponseHeaderActionFilter : ActionFilterAttribute // this have all all above inheritied interfaces
    {
        // Also we cannot inject logger ineface due to attribute filter,
        // IMPORTANT: biggest drawback of Attribute filter is we cannot use method injection or service injection
        //private readonly ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string _value;

        // Due to implemeting Attribute Filter, Order is defined in interface from which attribute filter is inheriting
        //public int Order { get; set; }

        public ResponseHeaderActionFilter(
            //ILogger<ResponseHeaderActionFilter> logger,
            string key,
            string value,
            int order
            )
        {
           // _logger = logger;
           _key = key;
           _value = value;
           Order = order;
        }

        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)

        // here we are suing override bcz of AttributeFilter, have virtual methods in their corresponding inheriting interfaces
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
         {
            // code from OnActionExecuting()
            // structured serilog logging
            //_logger.LogInformation("{FilterName}.{MethodName} before method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));


            await next(); // calls the subsequent filter or action method



            // code from OnActionExecuted()
            context.HttpContext.Response.Headers[_key] = _value;
            // structured serilog logging
            //_logger.LogInformation("{FilterName}.{MethodName} after method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
        }
    }
}
