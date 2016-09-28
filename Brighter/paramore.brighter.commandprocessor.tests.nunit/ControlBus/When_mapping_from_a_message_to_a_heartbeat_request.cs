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

using System;
using nUnitShouldAdapter;
using paramore.brighter.serviceactivator.Ports.Commands;
using paramore.brighter.serviceactivator.Ports.Mappers;

namespace paramore.brighter.commandprocessor.tests.nunit.ControlBus
{
    public class When_mapping_from_a_message_to_a_heartbeat_request : NUnit.Specifications.ContextSpecification
    {
        private static IAmAMessageMapper<HeartbeatRequest> s_mapper;
        private static Message s_message;
        private static HeartbeatRequest s_request;
        private const string TOPIC = "test.topic";
        private static readonly Guid s_correlationId = Guid.NewGuid();
        private static readonly Guid s_commandId = Guid.NewGuid();

        private Establish _context = () =>
        {
            s_mapper = new HeartbeatRequestCommandMessageMapper();
            var messageHeader = new MessageHeader(
                messageId: Guid.NewGuid(),
                topic: "Heartbeat",
                messageType: MessageType.MT_COMMAND,
                timeStamp: DateTime.UtcNow,
                correlationId: s_correlationId, replyTo: TOPIC);

            var body = String.Format("\"Id\": \"{0}\"", s_commandId);
            var messageBody = new MessageBody("{" + body + "}");
            s_message = new Message(header: messageHeader, body: messageBody);
        };

        private Because _of = () => s_request = s_mapper.MapToRequest(s_message);

        private It _should_put_the_message_reply_topic_into_the_address = () => s_request.ReplyAddress.Topic.ShouldEqual(TOPIC);
        private It _should_put_the_message_correlation_id_into_the_address = () => s_request.ReplyAddress.CorrelationId.ShouldEqual(s_correlationId);
        private It _should_set_the_id_of_the_request = () => s_request.Id.ShouldEqual(s_commandId);

    }
}