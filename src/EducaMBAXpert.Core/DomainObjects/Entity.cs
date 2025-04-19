﻿namespace EducaMBAXpert.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);

        }

        public static bool operator ==(Entity a, Entity b)
        {

            if (ReferenceEquals(null, a) && ReferenceEquals(b, null))
                return true;


            if (ReferenceEquals(null, a) || ReferenceEquals(b, null))
                return true;


            return a.Equals(b);

        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
