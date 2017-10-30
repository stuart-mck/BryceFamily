namespace BryceFamily.Repo.Core.Model
{
    public enum EventType
    {
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
        Gallery,
        Person
    }
}
