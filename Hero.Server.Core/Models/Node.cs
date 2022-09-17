﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Models
{
    public class Node
    {

        public Guid Id { get; set; }
        //SkillID for DB
        public Guid? SkillId { get; set; }
        public Guid? NodeTreeId { get; set; }
        /// <summary>
        /// Importance of the Node, shows the Node differently later on.
        /// Decides about the difference between 4 Edge Nodes, 5 Edge Nodes, 6 Edge Nodes...
        /// </summary>
        public int Importance { get; set; }
        public Skill Skill { get; set; }
        public int Cost { get; set; }
        public double XPos { get; set; }
        public double YPos { get; set; }
        public string Color { get; set; }
        public bool IsUnlocked  { get; set; }
        /// <summary>
        /// If the Node is easy reachable, only one of its precessors is needed to unlock it.
        /// If it aint all Precessors have to be unlocked to unlock this node
        /// </summary>
        public bool IsEasyReachable { get; set; }
        public List<Guid> Precessors { get; set; }
        public List<Guid> Successors { get; set; }
        public NodeTree? NodeTree { get; set; }

        public void Update(Node node)
        {
            this.Importance = node.Importance;
            this.Cost = node.Cost;
            this.Color = node.Color;
            this.IsUnlocked = node.IsUnlocked;
            this.IsEasyReachable = node.IsEasyReachable;
        }
    }
}
