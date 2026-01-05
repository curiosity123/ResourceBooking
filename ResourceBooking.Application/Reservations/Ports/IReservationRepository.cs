using ResourceBooking.Domain.Reservation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceBooking.Application.Reservations.Ports
{
    public interface IReservationRepository
    {
        Task<bool> TryCreateAsync(Reservation reservation, CancellationToken ct);
    }
}
