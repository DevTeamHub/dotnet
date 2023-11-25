using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DevTeam.Messaging.Emails;

public static class EmailControllerExtensions
{
    public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewName, TModel model, string viewParentFolder = "EmailViews/")
    {
        var httpContext = controller.HttpContext;
        var serviceProvider = httpContext.RequestServices;
        var viewEngine = serviceProvider.GetRequiredService<IRazorViewEngine>();
        var tempDataProvider = serviceProvider.GetRequiredService<ITempDataProvider>();

        var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), new ControllerActionDescriptor());


        var viewResult = viewEngine.FindView(actionContext, viewParentFolder + viewName, false);

        if (viewResult.View == null)
        {
            throw new ArgumentNullException($"{viewName} does not match any available view");
        }

        var viewDataDictionary = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        using var writer = new StringWriter();
        var viewContext = new ViewContext(actionContext, viewResult.View, viewDataDictionary, new TempDataDictionary(httpContext, tempDataProvider), writer, new HtmlHelperOptions());

        await viewResult.View.RenderAsync(viewContext);
        return writer.ToString();
    }

    public static string GetCallbackUrl(this Controller controller, string action, object value, string controllerName = "Account")
    {
        var callbackUrl = controller.Url.Action(action, controllerName, value, controller.HttpContext.Request.Scheme);

        if (string.IsNullOrEmpty(callbackUrl))
        {
            throw new Exception($"Incorrect name of action was used on attempt to send an email. Action name: {action}, Controller Name: {controllerName}, Routes: {value}");
        };

        return callbackUrl;
    }
}
