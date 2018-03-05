﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DurableFunctionsDemo.MeetupTravelInfo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DurableFunctionsDemo.MeetupTravelInfo.ActivityFunctions
{
    public static class GetNextEventForGroup
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName("GetNextEventForGroup")]
        public static async Task<MeetupEvent> Run(
            [ActivityTrigger]DurableActivityContext activityContext,
            TraceWriter log)
        {
            var input = activityContext.GetInput<MeetupTravelInfoInput>();
            string endpointUri = ConstructEventUri(input);
            
            var result = await HttpClient.GetAsync(endpointUri);
            var meetupEvents = await result.Content.ReadAsAsync<MeetupEvent[]>();

            return meetupEvents.FirstOrDefault();
        }

        private static string ConstructEventUri(MeetupTravelInfoInput input)
        {
            string meetupGroupUri = Environment.GetEnvironmentVariable("MeetupBaseUri");
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["page"] = "1";
            queryString["sign"] = "true";
            queryString["key"] = Environment.GetEnvironmentVariable("MeetupApiKey");

            return $"{meetupGroupUri}/{input.MeetupGroupUrlName}/events/?{queryString}";
        }
    }
}
