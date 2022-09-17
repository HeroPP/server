using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Models
{
    public class Ability
    {
        /// <summary>
        /// Unique ID of the Ability
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Indicates if the Ability is a passive Ability or one that can be actively used.
        /// </summary>
        public bool IsPassive { get; set; }
        /// <summary>
        /// Name of the Ability
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Describes the Effekt of the specific Ability. Under which conditions it can be used etc.
        /// </summary>
        public string Description { get; set; }

    }
}
