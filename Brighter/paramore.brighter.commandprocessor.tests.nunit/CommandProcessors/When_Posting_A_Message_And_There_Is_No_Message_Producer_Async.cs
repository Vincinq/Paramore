﻿#region Licence
/* The MIT License (MIT)
Copyright © 2015 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

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
using nUnitShouldAdapter;
using Newtonsoft.Json;
using Nito.AsyncEx;
using NUnit.Specifications;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.tests.nunit.CommandProcessors.TestDoubles;
using Polly;

namespace paramore.brighter.commandprocessor.tests.nunit.CommandProcessors
{
    [Subject(typeof(CommandProcessor))]
    public class When_Posting_A_Message_And_There_Is_No_Message_Producer_Async : NUnit.Specifications.ContextSpecification
    {
        private static CommandProcessor s_commandProcessor;
        private static readonly MyCommand s_myCommand = new MyCommand();
        private static Message s_message;
        private static FakeMessageStore s_fakeMessageStore;
        private static FakeMessageProducer s_fakeMessageProducer;
        private static Exception s_exception;

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_myCommand.Value = "Hello World";

            s_fakeMessageStore = new FakeMessageStore();

            s_message = new Message(
                header: new MessageHeader(messageId: s_myCommand.Id, topic: "MyCommand", messageType: MessageType.MT_COMMAND),
                body: new MessageBody(JsonConvert.SerializeObject(s_myCommand))
                );

            var messageMapperRegistry = new MessageMapperRegistry(new SimpleMessageMapperFactory(() => new MyCommandMessageMapper()));
            messageMapperRegistry.Register<MyCommand, MyCommandMessageMapper>();

            var retryPolicy = Policy
                .Handle<Exception>()
                .Retry();

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromMilliseconds(1));

            s_commandProcessor = new CommandProcessor(
                new InMemoryRequestContextFactory(),
                new PolicyRegistry() { { CommandProcessor.RETRYPOLICY, retryPolicy }, { CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy } },
                messageMapperRegistry,
                (IAmAMessageStoreAsync<Message>)s_fakeMessageStore,
                (IAmAMessageProducerAsync)null,
                logger);
        };

        private Because _of = () => s_exception = Catch.Exception(() => AsyncContext.Run(() => s_commandProcessor.PostAsync(s_myCommand)));

        private Cleanup cleanup = () => s_commandProcessor.Dispose();

        It _should_throw_an_exception = () => s_exception.ShouldBeOfExactType<InvalidOperationException>();

    }
}
