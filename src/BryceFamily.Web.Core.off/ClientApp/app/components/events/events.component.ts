import { Component, OnInit } from '@angular/core';
import { Event } from './../../model/eventEntities/event';
import { EventService } from './../../services/event.service';
import { Http } from '@angular/http';


@Component({
  selector: 'events',
  templateUrl:'./events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {

  public Events: Event[];
  public showDetail: boolean;

  constructor(http: Http) { 
    
    http.get('/api/Events/Events?activeOnly=true').subscribe(result => {
      this.Events = result.json() as Event[];
    });
    this.showDetail = false;
  }

  ngOnInit() {
    //this.eventService.getAllActiveEvents().then(events  => this.Events = events);
  }

 public switchDetail(){
   this.showDetail = !this.showDetail;
 }

}
