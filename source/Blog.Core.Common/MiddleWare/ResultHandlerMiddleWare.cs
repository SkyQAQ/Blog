using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ResultHandlerMiddleWare : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is FileContentResult)
        {

        }
        else if (context.Result is ObjectResult)
        {
            var objectResult = context.Result as ObjectResult;
            if (objectResult.Value == null)
            {
                context.Result = new ObjectResult(new { code = 1, mssg = "未找到资源", data = "" });
            }
            else
            {
                context.Result = new ObjectResult(new { code = 1, mssg = "", data = objectResult.Value });
            }
        }
        //else if (context.Result is EmptyResult)
        //{
        //    context.Result = new ObjectResult(new { code = 404, mssg = "未找到资源", data = "" });
        //}
        else if (context.Result is ContentResult)
        {
            context.Result = new ObjectResult(new { code = 200, mssg = "", data = (context.Result as ContentResult).Content });
        }
        else if (context.Result is StatusCodeResult)
        {
            context.Result = new ObjectResult(new { code = (context.Result as StatusCodeResult).StatusCode, mssg = "", data = "" });
        }
    }
}