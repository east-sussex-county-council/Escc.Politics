using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Escc.AddressAndPersonalDetails;

namespace Escc.Politics
{
    /// <summary>
    /// Represents a committee of East Sussex County Council
    /// </summary>
    public class Committee : IComparable<Committee>
    {
        private Collection<CommitteeMembership> members = new Collection<CommitteeMembership>();
        private Collection<CommitteeMeeting> meetings = new Collection<CommitteeMeeting>();

        /// <summary>
        /// A committee of East Sussex County Council
        /// </summary>
        public Committee() { }

        /// <summary>
        /// A committee of East Sussex County Council
        /// </summary>
        /// <param name="committeeName">The full title of the committee</param>
        public Committee(string committeeName)
        {
            this.Name = committeeName;
        }

        /// <summary>
        /// Gets the type, either scrutiny, panel, board or forum or other
        /// </summary>
        /// <value>The type.</value>
        public CommitteeType CommitteeType
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Name))
                {
                    if (Regex.IsMatch(this.Name, @"\bscrutiny\b", RegexOptions.IgnoreCase)) return CommitteeType.Scrutiny;
                    else if (Regex.IsMatch(this.Name, @"\b(panel|board|forum)\b", RegexOptions.IgnoreCase)) return CommitteeType.PanelBoardOrForum;
                }
                return CommitteeType.Other;

            }
        }

        /// <summary>
        /// A unique id representing the committee
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The full title of the committee
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the committee's functions
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Some committees' meetings are not open to the public, due to confidential topics (eg adoption)
        /// </summary>
        public bool Confidential { get; set; }

        /// <summary>
        /// Url of webpage representing the committee
        /// </summary>
        [XmlIgnore]
        public Uri NavigateUrl { get; set; }

        /// <summary>
        /// Url of webpage representing the committee. Synonym for <seealso cref="NavigateUrl"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("NavigateUrl")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string NavigateUrlSerialisable
        {
            get { return (this.NavigateUrl != null) ? this.NavigateUrl.ToString() : null; }
            set { this.NavigateUrl = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }

        /// <summary>
        /// Url of webpage listing committee agendas, minutes and reports 
        /// </summary>
        [XmlIgnore]
        public Uri MeetingPapersUrl { get; set; }

        /// <summary>
        /// Url of webpage listing committee agendas, minutes and reports. Synonym for <seealso cref="MeetingPapersUrl"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("MeetingPapersUrl")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string MeetingPapersUrlSerialisable
        {
            get { return (this.MeetingPapersUrl != null) ? this.MeetingPapersUrl.ToString() : null; }
            set { this.MeetingPapersUrl = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }

        /// <summary>
        /// Url of separate website representing the committee
        /// </summary>
        [XmlIgnore]
        public Uri NavigateUrlExternal { get; set; }

        /// <summary>
        /// Url of separate website representing the committee. Synonym for <seealso cref="NavigateUrlExternal"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("NavigateUrlExternal")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string NavigateUrlExternalSerialisable
        {
            get { return (this.NavigateUrlExternal != null) ? this.NavigateUrlExternal.ToString() : null; }
            set { this.NavigateUrlExternal = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }

        /// <summary>
        /// Committees have their membership approved by the Council on a particular date
        /// </summary>
        public DateTime? MembershipApproved { get; set; }

        /// <summary>
        /// Gets or sets the usual meeting venue.
        /// </summary>
        /// <value>The usual meeting venue.</value>
        public BS7666Address UsualMeetingVenue { get; set; }

        /// <summary>
        /// The members of the committee
        /// </summary>
        public Collection<CommitteeMembership> Members
        {
            get { return this.members; }
        }

        /// <summary>
        /// Gets the meetings of this committee.
        /// </summary>
        /// <value>The meetings.</value>
        public Collection<CommitteeMeeting> Meetings { get { return this.meetings; } }

        #region IComparable<T> with overrides recommended by code analysis
        /// <summary>
        /// Sort one committee against another using the name
        /// </summary>
        /// <param name="other">The other committeee.</param>
        /// <returns></returns>
        public int CompareTo(Committee other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return string.Compare(this.Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
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
        /// <param name="firstCommittee">The first committee.</param>
        /// <param name="secondCommittee">The second committee.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Committee firstCommittee, Committee secondCommittee)
        {
            if (!(firstCommittee is Committee && secondCommittee is Committee)) return false;
            return firstCommittee.Equals(secondCommittee);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="firstCommittee">The first committee.</param>
        /// <param name="secondCommittee">The second committee.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Committee firstCommittee, Committee secondCommittee)
        {
            if (!(firstCommittee is Committee && secondCommittee is Committee)) return true;
            return !firstCommittee.Equals(secondCommittee);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="firstCommittee">The first committee.</param>
        /// <param name="secondCommittee">The second committee.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Committee firstCommittee, Committee secondCommittee)
        {
            if (firstCommittee == null) throw new ArgumentNullException("firstCommittee");
            return firstCommittee.CompareTo(secondCommittee) > 0;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="firstCommittee">The first committee.</param>
        /// <param name="secondCommittee">The second committee.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Committee firstCommittee, Committee secondCommittee)
        {
            if (firstCommittee == null) throw new ArgumentNullException("firstCommittee");
            return firstCommittee.CompareTo(secondCommittee) < 0;
        }
        #endregion
    }
}
