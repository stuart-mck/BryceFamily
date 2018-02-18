namespace BryceFamily.Repo.Core.Model
{
    public enum EventType
    {
        Reunion,
        Gathering,
        Birthday,
        Funeral,
        Wedding,
        Other

    }

    public enum EventStatus
    {
        Pending,
        Cancelled,
        Expired
    }

    public enum ImageReferenceType
    {
        Event,
        Person
    }
}
