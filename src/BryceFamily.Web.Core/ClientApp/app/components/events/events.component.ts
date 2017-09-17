import { Component, OnInit, Inject  } from '@angular/core';
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
      public editMode: boolean;  
      public canEdit: boolean;
      public newEvent: Event;
  
      constructor(private eventService : EventService, http: Http, @Inject('BASE_URL') originUrl: string) { 
    
        http.get(originUrl + '/api/Events/Events?activeOnly=true').subscribe(result => {
          this.Events = result.json() as Event[];
        });
        this.showDetail = false;
        this.canEdit = true;
      }

      ngOnInit() {
        //this.eventService.getAllActiveEvents().then(events  => this.Events = events);
      }

     public switchDetail(){
       this.showDetail = !this.showDetail;
     }

     submitNewEvent() {
         this.eventService.saveEvent(this.newEvent)
             .then(result => {
                 this.newEvent = result;
             });
         this.showDetail = false;
         this.canEdit = true;

     }

     public addNewEvent() {

         this.newEvent = new Event();
         this.editMode = true;

     }

}
