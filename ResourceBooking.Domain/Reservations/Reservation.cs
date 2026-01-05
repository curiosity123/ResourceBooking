namespace ResourceBooking.Domain.Reservation
{
    public class Reservation:AggregateRoot<Guid>
    {

        public Guid UserId { get; private set; }
        public Guid ResourceId { get; private set; }
        public TimeRange TimeRange { get; private set; }
        public ReservationStatus Status { get; private set; }


        private Reservation() { }
        private Reservation(Guid id, Guid userId, Guid resourceId, TimeRange timeRange)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
            if (userId == Guid.Empty) throw new ArgumentException("UserId cannot be empty.", nameof(userId));
            if(resourceId == Guid.Empty) throw new ArgumentException("ResourceId cannot be empty.", nameof(resourceId));

            Id = id;
            UserId = userId;
            ResourceId = resourceId;
            TimeRange = timeRange;
            Status = ReservationStatus.Active;
        }

        public static Reservation Create(Guid id, Guid userId, Guid resourceId, TimeRange timeRange)
        {
            return new Reservation(id, userId, resourceId, timeRange);
        }

        public void Cancel()
        {
            if (Status == ReservationStatus.Cancelled)
            {
                throw new InvalidOperationException("Reservation is already cancelled.");
            }
            Status = ReservationStatus.Cancelled;
        }

    }
}
