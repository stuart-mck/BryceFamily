export class Event {
    public EntityId: number;
    public Title: string;
    public Dates: EventDateTime;
    public Location: Location;
    public Details: string; 
    public EventType: eventType;
    public EventStatus: eventStatus;
    public Organiser: string;
    
    constructor(){
    }

}

export class EventDateTime{

    public startDate: Date;
    public endDate: Date;
    
}

export class Location {
    public Title: string;
    public Address1: string;
    public Address2: string;
    public City: string;
    public State: string;
    public PostCode: number;
    public Geo: any; 
    public PhoneNumber: string;

}

export enum eventType {
    Gathering,
    Birthday,
    Funeral,
    Wedding,
    Other
}

export enum eventStatus {
    Pending,
    Cancelled,
    Expired
}