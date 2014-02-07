
using System;
namespace Escc.Politics
{
    /// <summary>
    /// An opportunity for constituents to meet and ask questions of their councillor
    /// </summary>
    public class Surgery : Meeting, IComparable<Surgery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Surgery"/> class.
        /// </summary>
        public Surgery()
        {
            this.Status = MeetingStatus.Confirmed;
        }

        /// <summary>
        /// Gets or sets the councillor.
        /// </summary>
        /// <value>The councillor.</value>
        public Councillor Councillor { get; set; }


        #region  IComparable<T> with overrides recommended by code analysis
        /// <summary>
        /// Compares one surgery to another by its date, allowing sorting.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(Surgery other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return this.MeetingDate.Ticks.CompareTo(other.MeetingDate.Ticks);
        }


        /// <summary>
        /// Compares one surgery to another by the date it was last modified, allowing sorting.
        /// </summary>
        public static Comparison<Surgery> CompareByMeetingDateDescending
        {
            get
            {
                return new Comparison<Surgery>(CompareSurgeryByMeetingDateDescending);
            }
        }

        /// <summary>
        /// Compares the surgery by date with newer meetings sorted first.
        /// </summary>
        /// <param name="object1">The object1.</param>
        /// <param name="object2">The object2.</param>
        /// <returns></returns>
        private static int CompareSurgeryByMeetingDateDescending(Surgery object1, Surgery object2)
        {
            if (object1 == null) throw new ArgumentNullException("object1");
            if (object2 == null) throw new ArgumentNullException("object2");
            return object1.MeetingDate.Ticks.CompareTo(object2.MeetingDate.Ticks) * -1;
        }

        /// <summary>
        /// Compares one surgery to another by the date it was last modified, allowing sorting.
        /// </summary>
        public static Comparison<Surgery> CompareByDateModifiedDescending
        {
            get
            {
                return new Comparison<Surgery>(CompareSurgeryByDateModifiedDescending);
            }
        }

        /// <summary>
        /// Compares the surgery by date modified with newer meetings sorted first.
        /// </summary>
        /// <param name="object1">The object1.</param>
        /// <param name="object2">The object2.</param>
        /// <returns></returns>
        private static int CompareSurgeryByDateModifiedDescending(Surgery object1, Surgery object2)
        {
            if (object1 == null) throw new ArgumentNullException("object1");
            if (object2 == null) throw new ArgumentNullException("object2");
            return object1.DateModified.Ticks.CompareTo(object2.DateModified.Ticks) * -1;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="firstInstance">The first instance.</param>
        /// <param name="secondInstance">The second instance.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Surgery firstInstance, Surgery secondInstance)
        {
            if (!(firstInstance is Surgery && secondInstance is Surgery)) return false;
            return firstInstance.Equals(secondInstance);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="firstInstance">The first instance.</param>
        /// <param name="secondInstance">The second instance.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Surgery firstInstance, Surgery secondInstance)
        {
            if (!(firstInstance is Surgery && secondInstance is Surgery)) return true;
            return !firstInstance.Equals(secondInstance);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="firstInstance">The first instance.</param>
        /// <param name="secondInstance">The second instance.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Surgery firstInstance, Surgery secondInstance)
        {
            if (firstInstance == null) throw new ArgumentNullException("firstInstance");
            return firstInstance.CompareTo(secondInstance) > 0;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="firstInstance">The first instance.</param>
        /// <param name="secondInstance">The second instance.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Surgery firstInstance, Surgery secondInstance)
        {
            if (firstInstance == null) throw new ArgumentNullException("firstInstance");
            return firstInstance.CompareTo(secondInstance) < 0;
        }
        #endregion
    }
}
