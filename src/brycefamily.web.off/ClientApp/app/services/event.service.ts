import { Injectable } from '@angular/core';
import { Event, EventDateTime, eventType, Location } from './../model/eventEntities/event';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class EventService {
   
   
   
  constructor(private http: Http) {
    
   }

  getAllActiveEvents(): Promise<Event[]>{
      return this.http.get('/api/Events/ActiveEvents?activeOnly=true')
      .toPromise()
      .then(results => {
        results.json() as Event[]
      })
      .catch(err => {
          return err;
          });
  }
}