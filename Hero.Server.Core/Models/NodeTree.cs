using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Models
{
    public class NodeTree
    {
        /// <summary>
        /// Unique ID of the NodeTree
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The Name of the NodeTree
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Available Points in this NodeTree
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// List of all Nodes the Tree has.
        /// </summary>
        public List<Guid> AllNodes { get; set; }
    }
}
