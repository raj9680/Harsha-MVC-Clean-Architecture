using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts_Manager.Filters.SkipFilters
{
    // Lets say AlwaysRunFilter is executing at Controller level i.e every method will execute that filter. If we want any specific method to not run AlwaysRunFilter, then we can use SkipFilter for that specific method. There is not any specific way but we build a logic.
    // IFilterMetadata: defines this class acts as filter.
    public class SkipFilter : Attribute, IFilterMetadata
    {

    }
}
