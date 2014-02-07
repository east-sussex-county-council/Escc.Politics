using System;

namespace Escc.Politics
{
    /// <summary>
    /// A councillor's declared affiliation with an organisation other than East Sussex County Council
    /// </summary>
    public class CouncillorAffiliation
    {
        /// <summary>
        /// Gets or sets the councillor.
        /// </summary>
        /// <value>The councillor.</value>
        public Councillor Councillor { get; set; }

        /// <summary>
        /// Gets or sets text describing the role the councillor performs with the organisation
        /// </summary>
        public string CouncillorRole { get; set; }

        /// <summary>
        /// Gets or sets the name of the organisation
        /// </summary>
        public string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the unique database identifier for the organisation
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Gets the organisation name with the role appended in brackets
        /// </summary>
        /// <returns></returns>
        public string AffiliationText
        {
            get
            {
                string text = this.OrganisationName;
                if (!String.IsNullOrEmpty(this.CouncillorRole)) text += " (" + this.CouncillorRole + ")";
                return text;
            }
        }
    }
}
