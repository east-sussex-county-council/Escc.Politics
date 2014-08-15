using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Escc.AddressAndPersonalDetails;

namespace Escc.Politics
{
    /// <summary>
    /// Represents a member of a council committee
    /// </summary>
    /// <remarks>Business logic layer</remarks>
    public class CommitteeMember : Person
    {
        /// <summary>
        /// Instantiates a member of a council committee
        /// </summary>
        public CommitteeMember() { }

        /// <summary>
        /// Url of webpage about the person
        /// </summary>
        [XmlIgnore]
        public Uri NavigateUrl { get; set; }

        /// <summary>
        /// Url of webpage about the person. Synonym for <seealso cref="NavigateUrl"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("NavigateUrl")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string NavigateUrlSerialisable
        {
            get { return (this.NavigateUrl != null) ? this.NavigateUrl.ToString() : null; }
            set { this.NavigateUrl = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }

        /// <summary>
        /// Gets or sets the URL for the person's own website
        /// </summary>
        [XmlIgnore]
        public Uri PersonalWebsiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for the person's own website. Synonym for <seealso cref="PersonalWebsiteUrl"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("PersonalWebsiteUrl")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string PersonalWebsiteUrlSerialisable
        {
            get { return (this.PersonalWebsiteUrl != null) ? this.PersonalWebsiteUrl.ToString() : null; }
            set { this.PersonalWebsiteUrl = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }
    }
}
