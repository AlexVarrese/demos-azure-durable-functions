﻿using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DurableFunctions.Demo.DotNetCore.Basics.Activities
{
    public class FailingActivity
    {
        [FunctionName(nameof(FailingActivity))]
        public string Run(
            [ActivityTrigger] string input,
            ILogger logger)
        {
            throw new ArgumentException($"The {nameof(FailingActivity)} will always throw an exception!");
        }
    }
}
