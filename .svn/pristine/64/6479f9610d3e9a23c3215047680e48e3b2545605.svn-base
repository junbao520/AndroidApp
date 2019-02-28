//using System;

//namespace TwoPole.Chameleon3.Domain
//{
//    /// <summary>
//    /// Base class for entities
//    /// </summary>
//    public abstract partial class EntityBase<T> : IEntity<T>
//        where T : struct
//    {
//        #region IEntity
//        /// <summary>
//        /// Gets or sets the entity identifier
//        /// </summary>
//        public T Id { get; set; }
//        #endregion

//        public override bool Equals(object obj)
//        {
//            return Equals(obj as EntityBase<T>);
//        }

//        private static bool IsTransient(EntityBase<T> obj)
//        {
//            return obj != null && Equals(obj.Id, default(T));
//        }

//        private Type GetUnproxiedType()
//        {
//            return GetType();
//        }

//        public bool Equals(EntityBase<T> other)
//        {
//            if (other == null)
//                return false;

//            if (ReferenceEquals(this, other))
//                return true;

//            if (!IsTransient(this) &&
//                !IsTransient(other) &&
//                Equals(Id, other.Id))
//            {
//                var otherType = other.GetUnproxiedType();
//                var thisType = GetUnproxiedType();
//                return thisType.IsAssignableFrom(otherType) ||
//                       otherType.IsAssignableFrom(thisType);
//            }

//            return false;
//        }

//        public override int GetHashCode()
//        {
//            if (Equals(Id, default(T)))
//                return base.GetHashCode();
//            return Id.GetHashCode();
//        }

//        public static bool operator ==(EntityBase<T> x, EntityBase<T> y)
//        {
//            return Equals(x, y);
//        }

//        public static bool operator !=(EntityBase<T> x, EntityBase<T> y)
//        {
//            return !(x == y);
//        }
//    }

//    public abstract partial class EntityBase : EntityBase<int>
//    {
//    }
//}