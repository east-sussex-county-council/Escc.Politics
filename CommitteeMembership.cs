using System;
using System.Xml.Serialization;

namespace Escc.Politics
{
    /// <summary>
    /// A committee member's membership of a committee
    /// </summary>
    [XmlInclude(typeof(Councillor))]
    public class CommitteeMembership : IComparable<CommitteeMembership>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitteeMembership"/> class.
        /// </summary>
        public CommitteeMembership() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitteeMembership"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        public CommitteeMembership(CommitteeMember member)
        {
            this.Member = member;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitteeMembership"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="role">The role.</param>
        public CommitteeMembership(CommitteeMember member, string role)
        {
            this.Member = member;
            this.MemberRole = role;
        }

        /// <summary>
        /// Gets or sets the role the committee member performs with the organisation
        /// </summary>
        public string MemberRole { get; set; }

        /// <summary>
        /// Gets or sets the committee member.
        /// </summary>
        /// <value>The member.</value>
        public CommitteeMember Member { get; set; }

        /// <summary>
        /// Gets or sets the committee the membership relates to
        /// </summary>
        public Committee Committee { get; set; }

        /// <summary>
        /// Gets the committee name with the role appended in brackets
        /// </summary>
        /// <returns></returns>
        public string MembershipText
        {
            get
            {
                string text = this.Committee.Name;
                if (!String.IsNullOrEmpty(this.MemberRole)) text += " (" + this.MemberRole + ")";
                return text;
            }
        }




        #region IComparable<T> with overrides recommended by code analysis
        /// <summary>
        /// Sort one committee membership against another using the committee name
        /// </summary>
        /// <param name="other">The other committeee membership.</param>
        /// <returns></returns>
        public int CompareTo(CommitteeMembership other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return string.Compare(this.Committee.Name, other.Committee.Name, StringComparison.CurrentCultureIgnoreCase);
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
        /// <param name="firstCommitteeMembership">The first committee membership.</param>
        /// <param name="secondCommitteeMembership">The second committee membership.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(CommitteeMembership firstCommitteeMembership, CommitteeMembership secondCommitteeMembership)
        {
            if (!(firstCommitteeMembership is CommitteeMembership && secondCommitteeMembership is CommitteeMembership)) return false;
            return firstCommitteeMembership.Equals(secondCommitteeMembership);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="firstCommitteeMembership">The first committee membership.</param>
        /// <param name="secondCommitteeMembership">The second committee membership.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(CommitteeMembership firstCommitteeMembership, CommitteeMembership secondCommitteeMembership)
        {
            if (!(firstCommitteeMembership is CommitteeMembership && secondCommitteeMembership is CommitteeMembership)) return true;
            return !firstCommitteeMembership.Equals(secondCommitteeMembership);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="firstCommitteeMembership">The first committee membership.</param>
        /// <param name="secondCommitteeMembership">The second committee membership.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(CommitteeMembership firstCommitteeMembership, CommitteeMembership secondCommitteeMembership)
        {
            if (firstCommitteeMembership == null) throw new ArgumentNullException("firstCommitteeMembership");
            return firstCommitteeMembership.CompareTo(secondCommitteeMembership) > 0;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="firstCommitteeMembership">The first committee membership.</param>
        /// <param name="secondCommitteeMembership">The second committee membership.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(CommitteeMembership firstCommitteeMembership, CommitteeMembership secondCommitteeMembership)
        {
            if (firstCommitteeMembership == null) throw new ArgumentNullException("firstCommitteeMembership");
            return firstCommitteeMembership.CompareTo(secondCommitteeMembership) < 0;
        }
        #endregion
    }
}
