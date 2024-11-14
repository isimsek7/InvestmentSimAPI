using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CryptoAnalyzer.WebApi.Filters
{
	public class TimeControlFilter:ActionFilterAttribute
	{
		public string StartTime { get; set; }

		public string EndTime { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var now = DateTime.Now.TimeOfDay;

            StartTime = "08:00";
            EndTime = "17:00";

            if(now >= TimeSpan.Parse(StartTime) && now <= TimeSpan.Parse(EndTime))
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = "Endpoints are only availabe between 08:00 to 17:00",
                    StatusCode = 403
                };
            }

            base.OnActionExecuting(context);
        }
    }
}

