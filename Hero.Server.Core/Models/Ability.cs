﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Models
{
    public class Ability
    {
        public string Name { get; set; }
        public Guid? CharacterId { get; set; }
        /// <summary>
        /// Indicates if the Ability is a passive Ability or one that can be actively used.
        /// </summary>
        public bool IsPassive { get; set; }
        public string Description { get; set; }
        public List<Skill>? Skills { get; set; }
        public Character? Character { get; set; }

        public void Update(Ability ability)
        {
            this.IsPassive= ability.IsPassive;
            this.Description= ability.Description;
        }
    }
}
