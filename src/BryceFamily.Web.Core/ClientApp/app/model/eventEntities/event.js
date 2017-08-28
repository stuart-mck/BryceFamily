"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Event = (function () {
    function Event() {
    }
    return Event;
}());
exports.Event = Event;
var EventDateTime = (function () {
    function EventDateTime() {
    }
    return EventDateTime;
}());
exports.EventDateTime = EventDateTime;
var Location = (function () {
    function Location() {
    }
    return Location;
}());
exports.Location = Location;
var eventType;
(function (eventType) {
    eventType[eventType["Gathering"] = 0] = "Gathering";
    eventType[eventType["Birthday"] = 1] = "Birthday";
    eventType[eventType["Funeral"] = 2] = "Funeral";
    eventType[eventType["Wedding"] = 3] = "Wedding";
    eventType[eventType["Other"] = 4] = "Other";
})(eventType = exports.eventType || (exports.eventType = {}));
var eventStatus;
(function (eventStatus) {
    eventStatus[eventStatus["Pending"] = 0] = "Pending";
    eventStatus[eventStatus["Cancelled"] = 1] = "Cancelled";
    eventStatus[eventStatus["Expired"] = 2] = "Expired";
})(eventStatus = exports.eventStatus || (exports.eventStatus = {}));
//# sourceMappingURL=event.js.map