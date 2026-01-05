using System;

public readonly record struct TimeRange
{

    public DateTime StartUtc { get; }
    public DateTime EndUtc { get; }

    public TimeRange(DateTime startUtc, DateTime endUtc)
    {

        if(startUtc.kind != DateTimeKind.Utc || endUtc.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("from and to must be in UTC");
        }

        if(startUtc > endUtc)
        {
            throw new ArgumentException("from must be earlier than to");
        }
        
    }

    public bool Overlaps(TimeRange other)
    {
        return StartUtc < other.EndUtc && EndUtc > other.StartUtc;
    }
}
