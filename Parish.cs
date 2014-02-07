using System;
using System.Collections.Generic;
using System.Text;

namespace Escc.Politics
{
    /// <summary>
    /// A territorial subdivision of a diocese
    /// </summary>
    public class Parish
    {
        /// <summary>
        /// Gets or sets the name of the parish.
        /// </summary>
        /// <value>The name of the parish.</value>
        public string ParishName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parish is partially represented by a particular councillor.
        /// </summary>
        /// <value><c>true</c> if partially represented; otherwise, <c>false</c>.</value>
        public bool? PartiallyRepresented { get; set; }
    }
}
