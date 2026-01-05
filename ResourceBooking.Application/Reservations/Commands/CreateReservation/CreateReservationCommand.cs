using MediatR;

namespace ResourceBooking.Application.Reservations.Commands.CreateReservation
{
    public sealed record CreateReservationCommand(Guid ResourceId, DateTime StartUtc, DateTime EndUtc) : IRequest<Guid>
    {
    }
}
