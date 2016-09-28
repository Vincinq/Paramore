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

using FakeItEasy;
using NUnit.Specifications;

namespace paramore.brighter.commandprocessor.tests.nunit.MessagingGateway
{
    [Subject(typeof(Channel))]
    public class When_Disposing_Input_Channel : ContextSpecification
    {
        private static IAmAChannel s_channel;
        private static IAmAMessageConsumer s_messageConsumer;

        private Establish _context = () =>
        {
            s_messageConsumer = A.Fake<IAmAMessageConsumer>();

            s_channel = new Channel("test", s_messageConsumer);
        };

        private Because _of = () => s_channel.Dispose();

        private It _should_call_dipose_on_messaging_gateway = () => A.CallTo(() => s_messageConsumer.Dispose()).MustHaveHappened();
    }
}