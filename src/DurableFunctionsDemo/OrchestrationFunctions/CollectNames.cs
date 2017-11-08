using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.OrchestrationFunctions
{
    public static class CollectNames
    {
        [FunctionName("CollectNames")]
        public static async Task<List<string>> Run(
            [OrchestrationTrigger]DurableOrchestrationContext context,
            TraceWriter log)
        {
            var nameList = context.GetInput<List<string>>() ?? new List<string>();

            var addNameTask = context.WaitForExternalEvent<string>("addname");
            var removeNameTask = context.WaitForExternalEvent<string>("removename");
            var isCompletedTask = context.WaitForExternalEvent<bool>("iscompleted");

            var resultingEvent = await Task.WhenAny(addNameTask, removeNameTask, isCompletedTask);

            if (resultingEvent == addNameTask)
            {
                nameList.Add(addNameTask.Result);
                log.Info($"Added {addNameTask.Result} to the list.");
            }
            else if (resultingEvent == removeNameTask)
            {
                nameList.Remove(removeNameTask.Result);
                log.Info($"Removed {removeNameTask.Result} from the list.");
            }

            if (resultingEvent == isCompletedTask &&
                isCompletedTask.Result)
            {
                log.Info("Completed updating the list.");
            }
            else
            {
                context.ContinueAsNew(nameList);
            }

            return nameList;
        }
    }
}
