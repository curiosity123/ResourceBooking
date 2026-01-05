using ResourceBooking.Domain.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceBooking.Application.Subscriptions
{
    public interface IBookingPolicy
    {
        Task<bool> CanCreateReservationAsync(Guid userId, ResourceType resourceType,TimeRange timeRange,CancellationToken ct);
    }
}
