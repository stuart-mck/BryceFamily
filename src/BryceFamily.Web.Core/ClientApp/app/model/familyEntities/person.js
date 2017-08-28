"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Person = (function () {
    function Person(entityId) {
        this.EntityId = entityId;
    }
    Person.prototype.addRelationship = function (target, type, startDate) {
        //this.Relationships.push(new )
    };
    Person.prototype.getParents = function () {
        return this.Relationships.filter(function (e) {
            return e.RelationshipType === relationshipType.parent;
        }).map(function (p) {
            return p.Relationship1;
        });
    };
    Person.prototype.getChildren = function () {
        return this.Relationships.filter(function (e) {
            return e.RelationshipType === relationshipType.child;
        }).map(function (p) {
            return p.Relationship1;
        });
    };
    Person.prototype.getSiblings = function () {
        var parents = this.getParents();
        var siblings = new Array();
        for (var i = 0; i < parents.length; i++) {
            var p = parents[i];
            var children = p.getChildren();
            for (var c = 0; c < children.length; c++)
                if (!siblings.some(function (child) { return child.EntityId === children[c].EntityId; })) {
                    siblings.push(children[c]);
                }
        }
        return siblings;
    };
    return Person;
}());
exports.Person = Person;
var Relationship = (function () {
    function Relationship(entityId, source, target) {
        this.EntityId = entityId;
        this.Relationship1 = source;
        this.Relationship2 = target;
    }
    return Relationship;
}());
exports.Relationship = Relationship;
var relationshipType;
(function (relationshipType) {
    relationshipType[relationshipType["parent"] = 0] = "parent";
    relationshipType[relationshipType["child"] = 1] = "child";
    relationshipType[relationshipType["spouse"] = 2] = "spouse";
})(relationshipType = exports.relationshipType || (exports.relationshipType = {}));
var relationshipStatus;
(function (relationshipStatus) {
    relationshipStatus[relationshipStatus["married"] = 0] = "married";
    relationshipStatus[relationshipStatus["divorced"] = 1] = "divorced";
    relationshipStatus[relationshipStatus["adopted"] = 2] = "adopted";
    relationshipStatus[relationshipStatus["natural"] = 3] = "natural";
})(relationshipStatus = exports.relationshipStatus || (exports.relationshipStatus = {}));
//# sourceMappingURL=person.js.map