using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hardware
{
    public enum AnswerMode
    {
        NoAsynchronousResponses=0,
        AllowAsynchronousResponses,
        AllCommandsWithConfirmationAndAsyncResponses,
        SentCommandsAreReturned
    }
}
