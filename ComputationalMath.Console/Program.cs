using ComputationalMath.Core.Domain.Matrices;
using ComputationalMath.Core.Domain.Sets;
using ComputationalMath.Methods;
using ComputationalMath.Methods.Matrices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ComputationalMath.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddScoped<IMatrixMethods, MatrixMethods>()
                .AddSingleton<RunMatrixExamples>()
                .BuildServiceProvider();

            //do the actual work here
            var runMatrixExamples = serviceProvider.GetService<RunMatrixExamples>();
            runMatrixExamples.Run();

        }
    }
}
