using MediatR;
using ResourceBooking.Application.Exceptions;
using ResourceBooking.Application.Reservations.Ports;
using ResourceBooking.Application.Subscriptions;
using ResourceBooking.Domain.Reservation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ResourceBooking.Application.Reservations.Commands.CreateReservation
{
    public sealed class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Guid>
    {
        private readonly IUserContext _userContext;
        private readonly IClock _clock;
        private readonly IResourceReadRepository _resources;
        private readonly IBookingPolicy _bookingPolicy;
        private readonly IReservationWriteRepository _reservations;

        public CreateReservationCommandHandler(IUserContext userContext, IClock clock, IResourceReadRepository resources, IBookingPolicy bookingPolicy, IReservationWriteRepository reservations)
        {
            _userContext = userContext;
            _clock = clock;
            _resources = resources;
            _bookingPolicy = bookingPolicy;
            _reservations = reservations;
        }
        public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken ct)
        {
            var range = new TimeRange(request.StartUtc, request.EndUtc);
            var userId = _userContext.UserId;
            var resourceType = await _resources.GetResourceTypeAsync(request.ResourceId, ct);

            var allowed = await _bookingPolicy.CanCreateReservationAsync(userId, resourceType, range, ct);

            if (!allowed)
            {
                throw new BookingForbiddenException("Reservation cannot be created due to booking policy.");
            }

            var reservationId = Guid.NewGuid();
            var utcNow = _clock.UtcNow;
            var reservation = Reservation.Create(reservationId, userId, request.ResourceId, range);

            var created = await _reservations.TryCreateAsync(reservation, ct);
            if (!created)
            {
                throw new ReservationConflictException("Reservation overlaps with an existing booking.");
            }
            return reservationId;

        }
    }
}
