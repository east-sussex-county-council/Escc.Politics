using System;
using System.ComponentModel;
using System.Xml.Serialization;
using EsccWebTeam.Gdsc;

namespace Escc.Politics
{
    /// <summary>
    /// A meeting of a committee, panel, board or forum
    /// </summary>
    public class CommitteeMeeting : Meeting, IComparable<CommitteeMeeting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitteeMeeting"/> class.
        /// </summary>
        public CommitteeMeeting()
        {
            this.Status = MeetingStatus.Confirmed;
        }

        /// <summary>
        /// Gets or sets the committee.
        /// </summary>
        /// <value>The committee.</value>
        public Committee Committee { get; set; }

        /// <summary>
        /// Url of webpage with the agenda, minutes and reports 
        /// </summary>
        [XmlIgnore]
        public Uri MeetingPapersUrl { get; set; }

        /// <summary>
        /// Url of webpage with the agenda, minutes and reports. Synonym for <seealso cref="MeetingPapersUrl"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("MeetingPapersUrl")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string MeetingPapersUrlSerialisable
        {
            get { return (this.MeetingPapersUrl != null) ? this.MeetingPapersUrl.ToString() : null; }
            set { this.MeetingPapersUrl = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }



        #region  IComparable<T> with overrides recommended by code analysis
        /// <summary>
        /// Compares one committee meeting to another by its date, allowing sorting.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(CommitteeMeeting other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return this.MeetingDate.Ticks.CompareTo(other.MeetingDate.Ticks);
        }


        /// <summary>
        /// Compares one committee meeting to another by the date it was last modified, allowing sorting.
        /// </summary>
        public static Comparison<CommitteeMeeting> CompareByMeetingDateDescending
        {
            get
            {
                return new Comparison<CommitteeMeeting>(CompareCommitteeMeetingByMeetingDateDescending);
            }
        }

        /// <summary>
        /// Compares the committee meeting by date with newer meetings sorted first.
        /// </summary>
        /// <param name="object1">The object1.</param>
        /// <param name="object2">The object2.</param>
        /// <returns></returns>
        private static int CompareCommitteeMeetingByMeetingDateDescending(CommitteeMeeting object1, CommitteeMeeting object2)
        {
            if (object1 == null) throw new ArgumentNullException("object1");
            if (object2 == null) throw new ArgumentNullException("object2");
            return object1.MeetingDate.Ticks.CompareTo(object2.MeetingDate.Ticks) * -1;
        }

        /// <summary>
        /// Compares one committee meeting to another by the date it was last modified, allowing sorting.
        /// </summary>
        public static Comparison<CommitteeMeeting> CompareByDateModifiedDescending
        {
            get
            {
                return new Comparison<CommitteeMeeting>(CompareCommitteeMeetingByDateModifiedDescending);
            }
        }

        /// <summary>
        /// Compares the committee meeting by date modified with newer meetings sorted first.
        /// </summary>
        /// <param name="object1">The object1.</param>
        /// <param name="object2">The object2.</param>
        /// <returns></returns>
        private static int CompareCommitteeMeetingByDateModifiedDescending(CommitteeMeeting object1, CommitteeMeeting object2)
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
        public static bool operator ==(CommitteeMeeting firstInstance, CommitteeMeeting secondInstance)
        {
            if (!(firstInstance is CommitteeMeeting && secondInstance is CommitteeMeeting)) return false;
            return firstInstance.Equals(secondInstance);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="firstInstance">The first instance.</param>
        /// <param name="secondInstance">The second instance.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(CommitteeMeeting firstInstance, CommitteeMeeting secondInstance)
        {
            if (!(firstInstance is CommitteeMeeting && secondInstance is CommitteeMeeting)) return true;
            return !firstInstance.Equals(secondInstance);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="firstInstance">The first instance.</param>
        /// <param name="secondInstance">The second instance.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(CommitteeMeeting firstInstance, CommitteeMeeting secondInstance)
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
        public static bool operator <(CommitteeMeeting firstInstance, CommitteeMeeting secondInstance)
        {
            if (firstInstance == null) throw new ArgumentNullException("firstInstance");
            return firstInstance.CompareTo(secondInstance) < 0;
        }
        #endregion
    }
}
