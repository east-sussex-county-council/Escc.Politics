using System;
using EsccWebTeam.Gdsc;

namespace Escc.Politics
{
    /// <summary>
    /// A candidate standing in an election
    /// </summary>
    public class ElectionCandidate : Person
    {
        /// <summary>
        /// Gets or sets the political party to which the candidate belongs
        /// </summary>
        public PoliticalParty Party { get; set; }

        /// <summary>
        /// Gets or sets whether the candidate has won his/her seat
        /// </summary>
        public bool WonSeat { get; set; }

        /// <summary>
        /// Gets or sets the number of votes cast for the candidate
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// Creates a candidate who is standing for election
        /// </summary>
        public ElectionCandidate()
        {
            this.Votes = -1;
        }

        /// <summary>
        /// Gets the percentage of the vote received by this candidate
        /// </summary>
        /// <param name="actualVotesForDivision">The number of votes cast in the candidate's electoral division</param>
        /// <returns>A percentage, rounded to one decimal place</returns>
        public double GetPercentOfVotes(int actualVotesForDivision)
        {
            if (actualVotesForDivision > 0)
            {
                double percent = ((double)this.Votes / (double)actualVotesForDivision) * 100;
                double rounded = Math.Round(percent, 1);
                return rounded;
            }
            throw new ArgumentException("Votes for division must be greater than 0", "actualVotesForDivision");
        }
    }
}
