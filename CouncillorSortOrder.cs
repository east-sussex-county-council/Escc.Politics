namespace Escc.Politics
{
    /// <summary>
    /// Data to sort councillors by
    /// </summary>
    public enum CouncillorSortOrder
    {
        /// <summary>
        /// Sort councillors alphabetically by the name of their electoral division
        /// </summary>
        ElectoralDivision = 0,

        /// <summary>
        /// Sort councillors alphabetically by their surname
        /// </summary>
        CouncillorName = 1,

        /// <summary>
        /// Sort councillors alphabetically by their party
        /// </summary>
        Party = 2
    }
}
