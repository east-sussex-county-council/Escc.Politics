namespace Escc.Politics
{
    /// <summary>
    /// Committees are sometimes split into three groups
    /// </summary>
    public enum CommitteeType
    {
        /// <summary>
        /// Any committee which is not a panel, board, forum or scrutiny committee
        /// </summary>
        Other = 0,

        /// <summary>
        /// A scrutiny committee
        /// </summary>
        Scrutiny = 1,

        /// <summary>
        /// A panel, board or forum
        /// </summary>
        PanelBoardOrForum = 2,


        /// <summary>
        /// Use all types of committee
        /// </summary>
        AllTypes = 3
    }
}