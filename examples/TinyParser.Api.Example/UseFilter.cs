using Microsoft.AspNetCore.Mvc.Filters;
using TinyParser.Core.Generators.LINQ;

namespace TinyParser.Api.Example;
public class UseFilter<T> : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var filter = context.HttpContext.Request.Query["filter"];
        if (filter.Count != 1) return;

        var expression = LambdaFactory.Produce<T>(filter.First());

        context.ActionArguments["filter"] = expression;

        base.OnActionExecuting(context);
    }
}
