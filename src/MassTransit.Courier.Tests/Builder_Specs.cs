﻿// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Courier.Tests
{
    using System;
    using NUnit.Framework;
    using TestFramework;


    [TestFixture]
    public class Using_a_class_on_a_routing_slip :
        ActivityTestFixture
    {
        [Test]
        public void Should_properly_map_the_types()
        {
            var builder = new RoutingSlipBuilder(Guid.NewGuid());
            var cmd3 = new ActivityMessageThreeCmd
            {
                Data = "Msg Three in Routing Slip."
            };
            builder.AddActivity("ActivityMessageThreeCmd", new Uri("loopback://localhost/exec_ActivityMessageThreeCmd"), cmd3);
        }


        public interface ITestResult
        {
            string MyResult { get; }
        }


        public interface IActivityMessageThreeCmd
        {
            ITestResult TestResult { get; }
            string Data { get; }
        }


        public class ActivityMessageThreeCmd : IActivityMessageThreeCmd
        {
            public ITestResult TestResult { get; set; }
            public string Data { get; set; }
        }


        protected override void SetupActivities()
        {
        }
    }
}