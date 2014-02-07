using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace Escc.Politics
{
    /// <summary>
    /// An electoral division for electing councillors to East Sussex County Council
    /// </summary>
    public class ElectoralDivision
    {
        private int seats = 1;

        /// <summary>
        /// Creates a new electoral division
        /// </summary>
        public ElectoralDivision()
        {
        }

        /// <summary>
        /// Creates a new electoral division
        /// </summary>
        /// <param name="divisionName">Name of the electoral division</param>
        public ElectoralDivision(string divisionName)
        {
            this.Name = divisionName;
        }

        /// <summary>
        /// Gets the division name formatted for use as an XHTML id attribute value
        /// </summary>
        /// <returns>A lowercase string of a-z characters</returns>
        public string GenerateXhtmlId()
        {
            return this.Name.ToLower(CultureInfo.CurrentCulture).Replace(" ", "").Replace(",", "").Replace(".", "").Replace("'", "").Replace("&amp;", "").Replace("&", "");
        }

        /// <summary>
        /// Gets or sets the unique database identifier for the division
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the electoral division
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the linked data URI of the division
        /// </summary>
        [XmlIgnore]
        public Uri DivisionUri { get; set; }

        /// <summary>
        /// Gets or sets the name of the folder containing information about this division
        /// </summary>
        /// <value>The name of the folder.</value>
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the linked data URI of the division. Synonym for <seealso cref="DivisionUri"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("DivisionUri")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string DivisionUriSerialisable
        {
            get { return (this.DivisionUri != null) ? this.DivisionUri.ToString() : null; }
            set { this.DivisionUri = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.Absolute); }
        }

        /// <summary>
        /// Gets or sets the name of the district or borough
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// Gets or sets the URL of the district or borough website
        /// </summary>
        [XmlIgnore]
        public Uri DistrictUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of the district or borough website. Synonym for <seealso cref="DistrictUrl"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("DistrictUrl")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string DistrictUrlSerialisable
        {
            get { return (this.DistrictUrl != null) ? this.DistrictUrl.ToString() : null; }
            set { this.DistrictUrl = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }

        /// <summary>
        /// Gets or sets the number of seats in the division
        /// </summary>
        public int Seats
        {
            get
            {
                return this.seats;
            }
            set
            {
                this.seats = value;
                if (this.seats > 2) throw new ArgumentOutOfRangeException("value", "No division has more that 2 seats");
            }
        }
    }
}
