
using System;
using Escc.AddressAndPersonalDetails;

namespace Escc.Politics
{
    /// <summary>
    /// A meeting involving councillors
    /// </summary>
    public abstract class Meeting
    {
        /// <summary>
        /// Gets or sets the meeting id.
        /// </summary>
        /// <value>The meeting id.</value>
        public int MeetingId { get; set; }

        /// <summary>
        /// Gets or sets the meeting date.
        /// </summary>
        /// <value>The meeting date.</value>
        public DateTime MeetingDate { get; set; }

        /// <summary>
        /// Gets or sets the venue.
        /// </summary>
        /// <value>The venue.</value>
        public BS7666Address Venue { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public MeetingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets when the details of this meeting were last modified.
        /// </summary>
        /// <value>The date last modified.</value>
        public DateTime DateModified { get; set; }
    }
}
