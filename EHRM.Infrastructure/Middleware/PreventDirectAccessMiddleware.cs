using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.Infrastructure.Middleware
{
    public class PreventDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var referer = context.HttpContext.Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(referer))
            {
                // Redirect to error page if no referrer is found
                context.Result = new RedirectToActionResult("Error", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }


}
