import { Injectable, Inject } from '@angular/core';
import { Event, EventDateTime, eventType, Location } from './../model/eventEntities/event';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class EventService {
   
   
   
    constructor(private http: Http, @Inject('BASE_URL') private originUrl: string) {
    
   }

  getAllActiveEvents(): Promise<Event[]>{
      return this.http.get(this.originUrl + '/api/Events/ActiveEvents?activeOnly=true')
      .toPromise()
      .then(results => {
        results.json() as Event[]
      })
      .catch(err => {
          return err;
          });
    }

  saveEvent(event: Event ): Promise<Event> {
      return this.http.post(this.originUrl + '/api/Events', event)
          .toPromise()
          .then(result => {
              if (result.status == 200)
                  true;
              else
                  false;
          })
          .catch(err => {
              return err;
          });
  }
}