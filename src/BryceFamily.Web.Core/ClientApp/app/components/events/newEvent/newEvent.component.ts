import { Component, OnInit } from '@angular/core';
import { Event } from './../../../model/eventEntities/event';
import { EventService } from './../../../services/event.service';
import { Http } from '@angular/http';

@Component({
    selector: 'newEvent',
    templateUrl: './newEvent.component.html',
    styleUrls: ['./newEvent.component.css']
})
export class NewEventComponent implements OnInit {

    public Event: Event;

    constructor(private http: Http, private eventService: EventService) {

        this.Event = new Event();
    }

    ngOnInit() {
        //this.eventService.getAllActiveEvents().then(events  => this.Events = events);
    }
}
