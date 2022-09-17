﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Models
{
    public class Skill
    {
        public Guid Id { get; set; }
        public Guid? AbilityId { get; set; }
        public string IconUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HealthPointsBoost { get; set; }
        public int LightPointsBoost { get; set; }
        public double MovementSpeedBoost { get; set; }
        public double ResistanceBoost { get; set; }
        public double OpticalRangeBoost { get; set; }
        public double MeleeDamageBoost { get; set; }
        public double RangeDamageBoost { get; set; }
        public double LightDamageBoost { get; set; }
        public double DamageBoost { get; set; }
        public double ParryBoost { get; set; }
        public double DodgeBoost { get; set; }
        public Ability Ability { get; set; }

        public List<Node>? InNodes { get; set; }
        public void Update(Skill skill)
        {
            this.IconUrl= skill.IconUrl;
            this.Name= skill.Name;
            this.Description= skill.Description;    
            this.HealthPointsBoost= skill.HealthPointsBoost;
            this.LightPointsBoost= skill.LightPointsBoost;
            this.MeleeDamageBoost= skill.MeleeDamageBoost;
            this.RangeDamageBoost= skill.RangeDamageBoost;
            this.LightDamageBoost= skill.LightDamageBoost;
            this.DamageBoost = skill.DamageBoost;
            this.ParryBoost = skill.ParryBoost;
            this.DodgeBoost = skill.DodgeBoost;
        }

    }
}
