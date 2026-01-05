using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceBooking.Application.Reservations.Ports
{
    public interface IUserContext
    {
        Guid UserId { get; }
    }
}
