namespace ResourceBooking.Application.Exceptions
{
    public sealed class ReservationConflictException : Exception
    {
        public ReservationConflictException(string message) : base(message)
        {
        }
    }
}
