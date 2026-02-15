using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request = {Request} - Response={Response} - ResponseData={ResponseData}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = new Stopwatch();
            timer.Start();

            var response = next();

            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
            {
                logger.LogWarning("[SLOW] The request = {Request} - Response={Response} - ResponseData={ResponseData} - TimeTaken={TimeTaken}",
                    typeof(TRequest).Name, typeof(TResponse).Name, request, timeTaken.Seconds);
            }

            logger.LogInformation("[END] Handle request = {Request} - Response={Response} - ResponseData={ResponseData} - TimeTaken={TimeTaken}",
                typeof(TRequest).Name, typeof(TResponse).Name, request, timeTaken.Seconds);

            return response;

        }
    }
}
