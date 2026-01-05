using ResourceBooking.Domain.Reservation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceBooking.Application.Reservations.Ports
{
    public interface IReservationWriteRepository
    {
        Task<bool> TryCreateAsync(Reservation reservation, CancellationToken ct);
        Task<Reservation?> GetByIdAsync(Guid reservationId, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
