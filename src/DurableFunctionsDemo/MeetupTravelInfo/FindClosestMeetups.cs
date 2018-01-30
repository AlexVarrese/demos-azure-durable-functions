using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsDemo.MeetupTravelInfo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.MeetupTravelInfo
{
    public static class FindClosestMeetups
    {
        [FunctionName("FindClosestMeetups")]
        public static async Task<TravelInfo[]> Run(
            [OrchestrationTrigger]DurableOrchestrationContext orchestrationContext,
            TraceWriter log)
        {
            var input = orchestrationContext.GetInput<FindClosestMeetupsInput>();
            var meetupEvents = await orchestrationContext.CallActivityAsync<MeetupEvent[]>("GetUpcomingEventsByText", input);

            var tasks = new List<Task<TravelInfo>>();
            foreach (var meetupEvent in meetupEvents)
            {
                var travelTimeInput = GetTravelTimeInput(input, meetupEvent);
                tasks.Add(
                    orchestrationContext.CallActivityAsync<TravelInfo>(
                        "GetTravelTime", 
                        travelTimeInput)
                    );
            }

            await Task.WhenAll(tasks);

            return tasks.Select(task => task.Result)
                .OrderBy(travelInfo => travelInfo.DurationSeconds)
                .ToArray();
        }

        private static TravelTimeInput GetTravelTimeInput(FindClosestMeetupsInput input, MeetupEvent meetupEvent)
        {
            return new TravelTimeInput
            {
                EventName = meetupEvent.Name,
                GroupName = meetupEvent.Group.Name,
                EventStartUnixTimeSeconds = meetupEvent.UnixTimeMilliseconds / 1000,
                TravelMode = input.TravelMode,
                DepartureAddress = input.DepartureAddress,
                DestinationAddress = $"{meetupEvent.Venue.Name}, {meetupEvent.Venue.Address}, {meetupEvent.Venue.City}",
                TrafficModel = "pessimistic"
            };
        }
    }
}
