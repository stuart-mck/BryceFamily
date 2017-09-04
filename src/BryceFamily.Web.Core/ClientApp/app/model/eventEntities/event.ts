export class Event {
    public entityId: number;
    public title: string;
    public dates: EventDateTime;
    public location: Location;
    public details: string; 
    public eventType: eventType;
    public eventStatus: eventStatus;
    public organiser: string;
    
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