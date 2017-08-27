using System;

public class EventDateTime{

    public DateTime StartDate;
    public DateTime EndDate;
    
}

public class Location {
    public string Title;
    public string Address1;
    public string Address2;
    public string City;
    public string State;
    public string PostCode;
    public string Geo; 
    public string PhoneNumber;

}

public enum eventType {
    Gathering,
    Birthday,
    Funeral,
    Wedding,
    Other
}

public enum eventStatus {
    Pending,
    Cancelled,
    Expired
}