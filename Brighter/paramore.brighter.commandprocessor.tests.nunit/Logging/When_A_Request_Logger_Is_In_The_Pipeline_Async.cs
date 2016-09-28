﻿using NUnit.Specifications;
using nUnitShouldAdapter;
using Nito.AsyncEx;
using paramore.brighter.commandprocessor.logging.Handlers;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.tests.nunit.CommandProcessors.TestDoubles;
using paramore.brighter.commandprocessor.tests.nunit.Logging.TestDoubles;
using TinyIoC;
using System.Linq;

namespace paramore.brighter.commandprocessor.tests.nunit.Logging
{
    class When_A_Request_Logger_Is_In_The_Pipeline_Async : NUnit.Specifications.ContextSpecification
    {
        private static SpyLog s_logger;
        private static MyCommand s_myCommand;
        private static IAmACommandProcessor s_commandProcessor;

        private Establish _context = () =>
        {
            s_logger = new SpyLog();
            s_myCommand = new MyCommand();

            var registry = new SubscriberRegistry();
            registry.RegisterAsync<MyCommand, MyLoggedHandlerAsync>();
            var container = new TinyIoCContainer();
            container.Register<IHandleRequestsAsync<MyCommand>, MyLoggedHandlerAsync>();
            container.Register<IHandleRequestsAsync<MyCommand>, RequestLoggingHandlerRequestHandlerAsync<MyCommand>>();
            container.Register<ILog>(s_logger);
           
            var handlerFactory = new TinyIocHandlerFactoryAsync(container);

            s_commandProcessor = new CommandProcessor(registry, handlerFactory, new InMemoryRequestContextFactory(), new PolicyRegistry(), s_logger);
        };

        private Because _of = () => AsyncContext.Run(async () => await s_commandProcessor.SendAsync(s_myCommand));

        private It _should_log_the_request_handler_call = () => s_logger.Logs.ShouldMatch(logs => logs.Any(log => log.Message.Contains("Logging handler pipeline call")));
        private It _should_log_the_type_of_handler_in_the_call = () => s_logger.Logs.ShouldMatch(logs => logs.Any( log => log.Message.Contains(typeof(MyCommand).ToString())));
    }
}
