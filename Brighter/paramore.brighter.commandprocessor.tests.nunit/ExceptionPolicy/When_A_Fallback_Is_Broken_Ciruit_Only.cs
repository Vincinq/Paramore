﻿#region Licence
/* The MIT License (MIT)
Copyright © 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using System;
using FakeItEasy;
using NUnit.Specifications;
using nUnitShouldAdapter;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.policy.Handlers;
using paramore.brighter.commandprocessor.tests.nunit.CommandProcessors.TestDoubles;
using paramore.brighter.commandprocessor.tests.nunit.ExceptionPolicy.TestDoubles;
using TinyIoC;

namespace paramore.brighter.commandprocessor.tests.nunit.ExceptionPolicy
{
    [Subject(typeof(ExceptionPolicyHandler<>))]
    public class When_A_Fallback_Is_Broken_Ciruit_Only : NUnit.Specifications.ContextSpecification
    {
        private static CommandProcessor s_commandProcessor;
        private static readonly MyCommand s_myCommand = new MyCommand();
        private static Exception s_exception;

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();

            var registry = new SubscriberRegistry();
            registry.Register<MyCommand, MyFailsWithUnsupportedExceptionForFallback>();
            var policyRegistry = new PolicyRegistry();

            var container = new TinyIoCContainer();
            var handlerFactory = new TinyIocHandlerFactory(container);
            container.Register<IHandleRequests<MyCommand>, MyFailsWithUnsupportedExceptionForFallback>().AsSingleton();
            container.Register<IHandleRequests<MyCommand>, FallbackPolicyHandler<MyCommand>>().AsSingleton();
            container.Register<ILog>(logger);


            MyFailsWithFallbackDivideByZeroHandler.ReceivedCommand = false;

            s_commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), policyRegistry, logger);
        };

        private Because _of = () => s_exception = Catch.Exception(() => s_commandProcessor.Send(s_myCommand));

        private It _should_send_the_command_to_the_command_handler = () => MyFailsWithUnsupportedExceptionForFallback.ShouldReceive(s_myCommand);
        private It _should_bubble_out_the_exception = () => s_exception.ShouldNotBeNull();
    }
}