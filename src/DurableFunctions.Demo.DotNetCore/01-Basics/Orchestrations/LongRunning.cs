using System.Threading.Tasks;
using DurableFunctions.Demo.DotNetCore.Basics.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Orchestrations
{
    public static class LongRunning
    {
        [FunctionName(nameof(LongRunning))]
        public static async Task<string> Run(
            [OrchestrationTrigger]DurableOrchestrationContextBase context,
            ILogger log)
        {
            var sleepTimeSeconds = context.GetInput<int>();

            var result = await context.CallActivityAsync<string>(
                nameof(SleepingActivity),
                sleepTimeSeconds);

            return result;
        }
    }
}
