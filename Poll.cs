
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
namespace Escc.Politics
{
    /// <summary>
    /// A poll to select winning candidate(s) in one electorial division, usually as part of a wider election
    /// </summary>
    public class Poll
    {
        private List<ElectionCandidate> candidates = new List<ElectionCandidate>();
        private int byElectionSeats = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Poll"/> class.
        /// </summary>
        /// <param name="division">The electoral division.</param>
        public Poll(ElectoralDivision division)
        {
            this.ElectoralDivision = division;
            this.PossibleVotes = -1;
            this.ActualVotes = -1;
        }

        /// <summary>
        /// Gets or sets the number of seats available for a division's by-election
        /// </summary>
        public int ByElectionSeats
        {
            get
            {
                return this.byElectionSeats;
            }
            set
            {
                this.byElectionSeats = value;
                if (this.byElectionSeats > 2) throw new ArgumentOutOfRangeException("value", "No division has more that 2 seats");
            }
        }

        /// <summary>
        /// Gets or sets the electoral division in which the poll takes place.
        /// </summary>
        /// <value>The electoral division.</value>
        public ElectoralDivision ElectoralDivision { get; set; }

        /// <summary>
        /// Gets or sets the election date.
        /// </summary>
        /// <value>The election date.</value>
        public DateTime ElectionDate { get; set; }

        /// <summary>
        /// Gets or sets the linked data URI of the poll taking place in the division
        /// </summary>
        [XmlIgnore]
        public Uri PollUri { get; set; }

        /// <summary>
        /// Gets or sets the linked data URI of the poll taking place in the division. Synonym for <seealso cref="PollUri"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("PollUri")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string PollUriSerialisable
        {
            get { return (this.PollUri != null) ? this.PollUri.ToString() : null; }
            set { this.PollUri = String.IsNullOrEmpty(value) ? null : new Uri(value, UriKind.Absolute); }
        }

        /// <summary>
        /// Gets candidates standing for election
        /// </summary>
        public IList<ElectionCandidate> Candidates
        {
            get
            {
                return this.candidates;
            }
        }

        /// <summary>
        /// Gets or sets the number of registered voters
        /// </summary>
        public int PossibleVotes { get; set; }

        /// <summary>
        /// Gets or sets the number of votes cast
        /// </summary>
        public int ActualVotes { get; set; }

        /// <summary>
        /// Gets the turnout as a percentage of the electorate
        /// </summary>
        /// <returns>Turnout rounded to one decimal place</returns>
        public double CalculateTurnout()
        {
            double percent = ((double)this.ActualVotes / (double)this.PossibleVotes) * 100;
            double rounded = Math.Round(percent, 1);
            return rounded;
        }



        /// <summary>
        /// Works out the winning candidate from all the specified candidates
        /// </summary>
        public void CalculateWinner()
        {
            ElectionCandidate[] winnersSoFar = new ElectionCandidate[this.ByElectionSeats > 0 ? this.ByElectionSeats : this.ElectoralDivision.Seats];

            foreach (ElectionCandidate cand in this.Candidates)
            {
                // HACK: this doesn't handle a tie...

                bool inserted = false;

                // look for an empty slot
                for (short i = 0; i < winnersSoFar.Length; i++)
                {
                    if (winnersSoFar[i] == null)
                    {
                        winnersSoFar[i] = cand;
                        inserted = true;
                        break;
                    }
                }

                if (inserted) continue;

                // all spaces taken - look for the lowest score
                int lowest = -1;
                int indexOfLowest = -1;
                for (short i = 0; i < winnersSoFar.Length; i++)
                {
                    if (lowest == -1 || winnersSoFar[i].Votes < lowest)
                    {
                        lowest = winnersSoFar[i].Votes;
                        indexOfLowest = i;
                    }
                }

                // replace lowest with current candidate, if they did better
                if (lowest > -1 && cand.Votes > lowest)
                {
                    winnersSoFar[indexOfLowest] = cand;
                }

                // reset winner status
                cand.WonSeat = false;
            }

            // Give all winners their seats
            foreach (ElectionCandidate cand in winnersSoFar)
            {
                if (cand != null) cand.WonSeat = true;
            }
        }

        /// <summary>
        /// Gets text saying which party(ies) won the division, and how multiple seats were distributed
        /// </summary>
        /// <returns>(party name) win (two seats).</returns>
        public string WinningPartyNames
        {
            get
            {
                List<int> partiesDone = new List<int>();
                StringBuilder sb = new StringBuilder();
                foreach (ElectionCandidate cand in this.candidates)
                {
                    if (cand.WonSeat && !partiesDone.Contains(cand.Party.Id))
                    {
                        if (sb.Length > 0) sb.Append("/");
                        sb.Append(cand.Party.Name);
                        partiesDone.Add(cand.Party.Id);
                    }
                }

                sb.Append(" win");

                if (this.ByElectionSeats > 0)
                {
                    if (this.ByElectionSeats > 1)
                    {
                        if (partiesDone.Count == 1)
                        {
                            sb.Append(" (two seats)");
                        }
                        else
                        {
                            sb.Append(" (one seat each)");
                        }
                    }
                }
                else if (this.ElectoralDivision.Seats > 1)
                {
                    if (partiesDone.Count == 1)
                    {
                        sb.Append(" (two seats)");
                    }
                    else
                    {
                        sb.Append(" (one seat each)");
                    }
                }

                sb.Append(".");

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets or sets when the information on this poll was last updated.
        /// </summary>
        /// <value>The last updated date.</value>
        public DateTime LastUpdated { get; set; }
    }
}
