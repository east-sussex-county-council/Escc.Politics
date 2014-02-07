using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Escc.Politics
{
    /// <summary>
    /// A political party
    /// </summary>
    public class PoliticalParty
    {
        /// <summary>
        /// Gets or sets the name of the party
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a unique database identifier for the party
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Creates a political party
        /// </summary>
        public PoliticalParty()
        {
        }

        /// <summary>
        /// Creates a political party
        /// </summary>
        /// <param name="partyName">The name of the party</param>
        public PoliticalParty(string partyName)
        {
            this.Name = partyName;
        }

        /// <summary>
        /// Gets or sets the linked data URI of the party
        /// </summary>
        [XmlIgnore]
        public Uri PartyUri { get; set; }

        /// <summary>
        /// Gets or sets the linked data URI of the party. Synonym for <seealso cref="PartyUri"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("PartyUri")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string PartyUriSerialisable
        {
            get { return (this.PartyUri != null) ? this.PartyUri.ToString() : null; }
            set { this.PartyUri = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.Absolute); }
        }
    }
}
