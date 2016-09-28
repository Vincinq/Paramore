#region Licence
/* The MIT License (MIT)
Copyright � 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the �Software�), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using System.Linq;
using FakeItEasy;
using nUnitShouldAdapter;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.tests.nunit.CommandProcessors.TestDoubles;
using paramore.brighter.serviceactivator;
using paramore.brighter.serviceactivator.controlbus;
using paramore.brighter.serviceactivator.TestHelpers;

namespace paramore.brighter.commandprocessor.tests.nunit.ControlBus
{
    public class When_configuring_a_control_bus : NUnit.Specifications.ContextSpecification
    {
        private static Dispatcher s_controlBus;
        private static ControlBusReceiverBuilder s_busReceiverBuilder;
        private static IDispatcher s_dispatcher;
        private static string s_hostName = "tests";

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_dispatcher = A.Fake<IDispatcher>();
            var messageProducerFactory = A.Fake<IAmAMessageProducerFactory>();

            A.CallTo(() => messageProducerFactory.Create()).Returns(new FakeMessageProducer());

            s_busReceiverBuilder = ControlBusReceiverBuilder
                .With()
                .Dispatcher(s_dispatcher)
                .ProducerFactory(messageProducerFactory)
                .ChannelFactory(new InMemoryChannelFactory()) as ControlBusReceiverBuilder;
        };

        private Because _of = () => s_controlBus = s_busReceiverBuilder.Build(s_hostName);

        private It _should_have_a_configuration_channel = () => s_controlBus.Connections.Any(cn => cn.Name == s_hostName  + "." + ControlBusReceiverBuilder.CONFIGURATION).ShouldBeTrue();
        private It _should_have_a_heartbeat_channel = () => s_controlBus.Connections.Any(cn => cn.Name == s_hostName + "." + ControlBusReceiverBuilder.HEARTBEAT).ShouldBeTrue();
        private It _should_have_a_command_processor = () => s_controlBus.CommandProcessor.ShouldNotBeNull();
    }
}