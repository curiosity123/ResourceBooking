using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceBooking.Application.Reservations.Ports
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
