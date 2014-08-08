using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Hardware.Hardware__Physical_.FAULHABER_MINIMOTOR_SA
{
    public enum AnswerMode
    {
        NoAsynchronousResponses=0,
        AllowAsynchronousResponses,
        AllCommandsWithConfirmationAndAsyncResponses,
        SentCommandsAreReturned
    }
}
