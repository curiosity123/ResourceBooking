using ResourceBooking.Domain.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceBooking.Application.Reservations.Ports
{
    public interface IResourceReadRepository
    {
        Task<ResourceType> GetResourceTypeAsync(Guid resourceId, CancellationToken ct);
    }
}
