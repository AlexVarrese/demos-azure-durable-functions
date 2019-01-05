using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Starters
{
    public static class HttpStart
    {

        /// <summary>
        /// This function starts a new orchestration in the same Function App
        /// which matches the functionName parameter.
        /// </summary>
        /// <param name="req">The HttpRequestMessage which can contain input data for the orchestration.</param>
        /// <param name="orchestrationClient">An instance of the DurableOrchestrationClient used to start a new orchestration.</param>
        /// <param name="functionName">The name of the orchestration function to start.</param>
        /// <param name="id">Optional id for the orchestration function instance.</param>
        /// <param name="log">ILogger implementation.</param>
        /// <returns>An HttpResponseMessage containing the id and status of the orchestration instance.</returns>
        [FunctionName(nameof(HttpStart))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "start/{functionName}/{id?}")]HttpRequestMessage req, 
            [OrchestrationClient]DurableOrchestrationClientBase orchestrationClient,
            string functionName,
            string id,
            ILogger log)
        {
            dynamic functionData = await req.Content.ReadAsAsync<object>();

            string instanceId = id;
            if (string.IsNullOrEmpty(instanceId))
            {
                // Start a new orchestration and let Durable Functions generate the instance id.
                instanceId = await orchestrationClient.StartNewAsync(
                    functionName,
                    functionData);
            }
            else
            {
                // Start a new orchestration and use your own instance id.
                instanceId = await orchestrationClient.StartNewAsync(
                    functionName,
                    instanceId,
                    functionData);
            }

            log.LogInformation($"Started orchestration with ID = '{instanceId}'...");

            return orchestrationClient.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
