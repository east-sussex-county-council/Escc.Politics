
namespace Escc.Politics
{
    /// <summary>
    /// A question which a councillor or committee member may answer
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        /// <value>The question id.</value>
        public int? QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the question reference, eg 1, 1a or 2.
        /// </summary>
        /// <value>The question reference.</value>
        public string QuestionReference { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        /// <value>The question text.</value>
        public string QuestionText { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        /// <value>The answer.</value>
        public string Answer { get; set; }
    }
}
