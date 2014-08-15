using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using Escc.AddressAndPersonalDetails;

namespace Escc.Politics
{
    /// <summary>
    /// A councillor on East Sussex County Council
    /// </summary>
    public class Councillor : CommitteeMember
    {
        private Collection<CommitteeMembership> committees = new Collection<CommitteeMembership>();
        private Collection<CouncillorAffiliation> affiliations = new Collection<CouncillorAffiliation>();
        private Collection<string> councilStatus = new Collection<string>();
        private Collection<Question> financialInterests = new Collection<Question>();
        private Collection<Question> financialInterestsOfPartner = new Collection<Question>();
        private Collection<Surgery> surgeries = new Collection<Surgery>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Councillor"/> class.
        /// </summary>
        public Councillor()
        {
            this.Name.Titles.Add("Councillor");
        }

        /// <summary>
        /// Gets or sets the home address.
        /// </summary>
        /// <value>The home address.</value>
        public BS7666Address HomeAddress { get; set; }

        /// <summary>
        /// Gets or sets the work address.
        /// </summary>
        /// <value>The work address.</value>
        public BS7666Address WorkAddress { get; set; }

        /// <summary>
        /// Gets or sets the interests.
        /// </summary>
        /// <value>The interests.</value>
        public string Interests { get; set; }

        /// <summary>
        /// Gets or sets the outisde organisations with which the councillor is affiliated
        /// </summary>
        public Collection<CouncillorAffiliation> Affiliations
        {
            get
            {
                return this.affiliations;
            }
        }

        /// <summary>
        /// Gets or sets the committees with which the councillor is affiliated
        /// </summary>
        public Collection<CommitteeMembership> Committees
        {
            get
            {
                return this.committees;
            }
        }

        /// <summary>
        /// Gets or sets the date the councillor was elected
        /// </summary>
        public DateTime? DateElected { get; set; }

        /// <summary>
        /// Gets or sets the URL for an photo of the councillor
        /// </summary>
        [XmlIgnore]
        public Uri ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for an photo of the councillor. Synonym for <seealso cref="ImageUrl"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("ImageUrl")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string ImageUrlSerialisable
        {
            get { return (this.ImageUrl != null) ? this.ImageUrl.ToString() : null; }
            set { this.ImageUrl = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }

        /// <summary>
        /// Gets the name in the form "Councillor [FirstName] [LastName]"
        /// </summary>
        public string FullName
        {
            get
            {
                if (this.Name.GivenNames.Count > 0)
                {
                    return (this.Name.Titles.ToString() + " " + this.Name.GivenNames[0].ToString() + " " + this.Name.FamilyName + " " + this.Name.Suffixes.ToString()).Trim();
                }
                else
                {
                    return this.Name.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the name in the form "Smith, John Jack"
        /// </summary>
        /// <value>The name reversed.</value>
        public string NameReversed { get { return this.Name.FamilyName + ", " + this.Name.GivenNames.ToString(); } }

        /// <summary>
        /// Gets or sets the political party to which the candidate belongs
        /// </summary>
        public PoliticalParty Party { get; set; }

        /// <summary>
        /// Gets or sets the electoral division the councillor represents
        /// </summary>
        public ElectoralDivision ElectoralDivision { get; set; }

        /// <summary>
        /// Gets the councillor's prominent roles in the County Council cabinet and leadership.
        /// </summary>
        /// <value>The councillor's County Council status.</value>
        public Collection<string> CouncilStatus
        {
            get
            {
                return this.councilStatus;
            }
        }

        /// <summary>
        /// Gets questions and answers about the councillor's financial interests.
        /// </summary>
        /// <value>The questions.</value>
        public Collection<Question> FinancialInterests { get { return this.financialInterests; } }

        /// <summary>
        /// Gets questions and answers about the councillor's spouse or partner's financial interests.
        /// </summary>
        /// <value>The questions.</value>
        public Collection<Question> FinancialInterestsOfPartner { get { return this.financialInterestsOfPartner; } }

        /// <summary>
        /// Gets or sets the URL for the councillor's financial interests
        /// </summary>
        [XmlIgnore]
        public Uri FinancialInterestsUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for the councillor's financial interests. Synonym for <seealso cref="FinancialInterestsUrl"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("FinancialInterestsUrl")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string FinancialInterestsUrlSerialisable
        {
            get { return (this.FinancialInterestsUrl != null) ? this.FinancialInterestsUrl.ToString() : null; }
            set { this.FinancialInterestsUrl = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute); }
        }

        /// <summary>
        /// Gets the dates of surgeries where the councillor's constituents can come to talk
        /// </summary>
        /// <value>The surgery dates.</value>
        public Collection<Surgery> SurgeryDates { get { return this.surgeries; } }
    }
}
