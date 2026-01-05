using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceBooking.Application.Exceptions
{
    public sealed class BookingForbiddenException:Exception
    {
        public BookingForbiddenException(string message) :base(message)
        {
        }
    }
}
