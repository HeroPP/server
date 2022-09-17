using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Models
{
    public class Node
    {
        /// <summary>
        /// Unique ID of the Node
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Importance of the Node, shows the Node differently later on.
        /// Decides about the difference between 4 Edge Nodes, 5 Edge Nodes, 6 Edge Nodes...
        /// </summary>
        public int Importance { get; set; }
        /// <summary>
        /// The Skill which will be aquired if the Node is unlocked
        /// </summary>
        public Skill Skill { get; set; }
        /// <summary>
        /// The Cost of Points to unlock this node
        /// </summary>
        public int Cost { get; set; }
        /// <summary>
        /// Position of the node on the x-Axis on which it shall be renderd in the NodeTree
        /// </summary>
        public double XPos { get; set; }
        /// <summary>
        /// Position of the node on the y-Axis on which it shall be renderd in the NodeTree
        /// </summary>
        public double YPos { get; set; }
        /// <summary>
        /// Backgroundcolor of the Node. Helps to group the Nodes semanticly
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Indicates if the Node has been unlocked yet
        /// </summary>
        public bool IsUnlocked  { get; set; }
        /// <summary>
        /// If the Node is easy reachable, only one of its precessors is needed to unlock it.
        /// If it aint all Precessors have to be unlocked to unlock this node
        /// </summary>
        public bool IsEasyReachable { get; set; }
        /// <summary>
        /// All Nodes that are directly pointing to this Node
        /// </summary>
        public List<Guid> Precessors { get; set; }
        /// <summary>
        /// All Nodes to which this Node points
        /// </summary>
        public List<Guid> Successors { get; set; } 
    }
}
