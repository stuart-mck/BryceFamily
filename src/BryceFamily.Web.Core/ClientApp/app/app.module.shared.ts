import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { EventsComponent } from './components/events/events.component';
import { FamilyHistoryComponent } from './components/familyHistory/familyHistory.component';

//services
import { EventService } from './services/event.service';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        EventsComponent,
        FamilyHistoryComponent 
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'events', component: EventsComponent },
            { path: 'familyHistory', component: FamilyHistoryComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [EventService]
})
export class AppModuleShared {
}
