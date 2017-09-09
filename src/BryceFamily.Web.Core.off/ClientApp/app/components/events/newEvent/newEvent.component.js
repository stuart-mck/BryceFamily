"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var event_1 = require("./../../../model/eventEntities/event");
var event_service_1 = require("./../../../services/event.service");
var http_1 = require("@angular/http");
var NewEventComponent = (function () {
    function NewEventComponent(http, eventService) {
        this.http = http;
        this.eventService = eventService;
        this.Event = new event_1.Event();
    }
    NewEventComponent.prototype.ngOnInit = function () {
        //this.eventService.getAllActiveEvents().then(events  => this.Events = events);
    };
    NewEventComponent.prototype.submitNewEvent = function () {
        console.log(this.Event);
    };
    return NewEventComponent;
}());
NewEventComponent = __decorate([
    core_1.Component({
        selector: 'newEvent',
        templateUrl: './newEvent.component.html',
        styleUrls: ['./newEvent.component.css']
    }),
    __metadata("design:paramtypes", [http_1.Http, event_service_1.EventService])
], NewEventComponent);
exports.NewEventComponent = NewEventComponent;
//# sourceMappingURL=newEvent.component.js.map