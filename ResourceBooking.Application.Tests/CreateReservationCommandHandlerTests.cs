using ResourceBooking.Application.Exceptions;
using ResourceBooking.Application.Reservations.Commands.CreateReservation;
using ResourceBooking.Application.Reservations.Ports;
using ResourceBooking.Application.Subscriptions;
using ResourceBooking.Domain.Reservation;
using ResourceBooking.Domain.Resources;
using Xunit;

namespace ResourceBooking.Application.Tests
{
    public class CreateReservationCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenAllowedAndNoConflict_CreateReservation()
        {

            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var resourceId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var now = new DateTime(2026, 1, 5, 12, 0, 0, DateTimeKind.Utc);

            var user = new FakeUserContext(userId);
            var clock = new FakeClock(now);
            var policy = new AllowAllPolicy();
            var resources = new FakeResourceReadRepository();
            var repo = new FakeReservationWriteRepository(returnCreated: true);


            var handler = new CreateReservationCommandHandler(user, clock, resources, policy, repo);
            var cmd = new CreateReservationCommand(
                resourceId,
                StartUtc: new DateTime(2026, 1, 6, 10, 0, 0, DateTimeKind.Utc),
                EndUtc: new DateTime(2026, 1, 6, 11, 0, 0, DateTimeKind.Utc));

            // Act
            var id = await handler.Handle(cmd, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, id);
            Assert.Single(repo.Created);
            Assert.Equal(userId, repo.Created[0].UserId);
            Assert.Equal(resourceId, repo.Created[0].ResourceId);
            Assert.Equal(cmd.StartUtc, repo.Created[0].TimeRange.StartUtc);
            Assert.Equal(cmd.EndUtc, repo.Created[0].TimeRange.EndUtc);
        }

        [Fact]
        public async Task Handle_WhenReservationConflict_ThrowReservationConflictException()
        {

            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var resourceId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var now = new DateTime(2026, 1, 5, 12, 0, 0, DateTimeKind.Utc);

            var user = new FakeUserContext(userId);
            var clock = new FakeClock(now);
            var policy = new AllowAllPolicy();
            var resources = new FakeResourceReadRepository();
            var repo = new FakeReservationWriteRepository(returnCreated: false);


            var handler = new CreateReservationCommandHandler(user, clock, resources, policy, repo);
            var cmd = new CreateReservationCommand(
                resourceId,
                StartUtc: new DateTime(2026, 1, 6, 10, 0, 0, DateTimeKind.Utc),
                EndUtc: new DateTime(2026, 1, 6, 11, 0, 0, DateTimeKind.Utc));

          //act + assert
            await Assert.ThrowsAsync<ReservationConflictException>(() =>  handler.Handle(cmd, CancellationToken.None));


        }




        private sealed class FakeUserContext : IUserContext
        {
            public FakeUserContext(Guid userId) => UserId = userId;
            public Guid UserId { get; }
        }

        private sealed class FakeClock : IClock
        {
            public FakeClock(DateTime utcNow) => UtcNow = utcNow;
            public DateTime UtcNow { get; }
        }

        private sealed class AllowAllPolicy : IBookingPolicy
        {
            public Task<bool> CanCreateReservationAsync(Guid userId, ResourceType resourceType, TimeRange timeRange, CancellationToken ct)
            {
                return  Task.FromResult(true);
            }
        }

        private sealed class FakeResourceReadRepository : IResourceReadRepository
        {
            public Task<ResourceType> GetResourceTypeAsync(Guid resourceId, CancellationToken ct)
            {
                return Task.FromResult(ResourceType.Desk);
            }
        }

        private sealed class FakeReservationWriteRepository : IReservationWriteRepository
        {
            private readonly bool _returnCreated;
            public List<Reservation> Created { get; } = new();

            public FakeReservationWriteRepository(bool returnCreated) => _returnCreated = returnCreated;

            public Task<bool> TryCreateAsync(Reservation reservation, CancellationToken ct)
            {
                Created.Add(reservation);
                return Task.FromResult(_returnCreated);
            }

            public Task<Reservation?> GetByIdAsync(Guid reservationId, CancellationToken ct)
            {
                throw new NotImplementedException();
            }

            public Task SaveChangesAsync(CancellationToken ct)
            {
                throw new NotImplementedException();
            }
        }
    }
}
