﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pipelines
{
    public class MetricsBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<MetricsBehaviour<TRequest, TResponse>> _logger;

        public MetricsBehaviour(ILogger<MetricsBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await next();

            stopwatch.Stop();

            _logger.LogInformation($"Finished in {stopwatch.ElapsedMilliseconds}ms");

            return result;
        }
    }
}
