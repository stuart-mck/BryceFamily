
export class Person {
    public EntityId: number;
    public FirstName: string;
    public MiddleName: Array<string>;
    public LastName: string;
    public DateOfBirth: Date;
    public DateOfDeath: Date;
    public Relationships: Array<Relationship>;

    constructor(entityId: number){
        this.EntityId = entityId;
    }

    public addRelationship(target: Person, type: relationshipType, startDate: Date){
        //this.Relationships.push(new )
    }

    public getParents(): Array<Person> {
        return this.Relationships.filter(function (e){
            return e.RelationshipType === relationshipType.parent;
        }).map(function(p){
            return p.Relationship1
        });
    }

    public getChildren() : Array<Person>{
        return this.Relationships.filter(function (e){
            return e.RelationshipType === relationshipType.child;
        }).map(function(p){
            return p.Relationship1
        });
    } 

    public getSiblings(): Array<Person>{

        var parents = this.getParents();
        let siblings = new Array<Person>();

        for(var i = 0; i < parents.length; i++){
            let p = parents[i];
            var children = p.getChildren();
            for (var c = 0; c < children.length; c++)
                if (!siblings.some(function(child) {return child.EntityId === children[c].EntityId;})){
                    siblings.push(children[c]);
            }
        }

        return siblings;
    }

}

export class Relationship {
    public EntityId: number;
    public RelationshipType: relationshipType;
    public RelationshipStatus: relationshipStatus;
    public Relationship1: Person;
    public Relationship2: Person;
    public Children : Array<Person>;
    constructor(entityId: number, source: Person, target: Person){
        this.EntityId = entityId;
        this.Relationship1 = source;
        this.Relationship2 = target;
    }
}

export enum relationshipType{
    parent,
    child,
    spouse
}

export enum relationshipStatus{
    married,
    divorced,
    adopted,
    natural
}