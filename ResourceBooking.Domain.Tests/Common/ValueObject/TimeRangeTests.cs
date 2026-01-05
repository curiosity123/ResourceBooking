using Xunit;

namespace ResourceBooking.Domain.Tests.Common.ValueObject
{
    public class TimeRangeTests
    {

        public TimeRangeTests()
        {
        }

        [Fact]
        public void TimeRange_WhenEndEarilerThanStart_ThenThrowArgumentException()
        {

            Assert.Throws<ArgumentException>(() =>
                 new TimeRange(
                     startUtc: new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                     endUtc: new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                 )
             );
        }
    } 
}
