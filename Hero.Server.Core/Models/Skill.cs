using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Models
{
    public class Skill
    {
        /// <summary>
        /// Unique ID of the Skill
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// URL used to show the Icon of the Skill
        /// </summary>
        public string IconUrl { get; set; }
        /// <summary>
        /// Name of the Skill. Shown in the Nodetree and the Charaktersheet
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Describes the Skill, basic Effekts and Modifiers
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Additive Boost on Heroes HealthPoints
        /// </summary>
        public int HealthPointsBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes LightPoints
        /// </summary>
        public int LightPointsBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Movementspeed
        /// </summary>
        public double MovementSpeedBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Damage Resistance
        /// </summary>
        public double ResistanceBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Vision Range
        /// </summary>
        public double OpticalRangeBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Melee Damage
        /// </summary>
        public double MeleeDamageBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Range Damage
        /// </summary>
        public double RangeDamageBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Light Damage
        /// </summary>
        public double LightDamageBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Base Damage
        /// </summary>
        public double DamageBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Parries
        /// </summary>
        public double ParryBoost { get; set; }
        /// <summary>
        /// Additive Boost on Heroes Dodges
        /// </summary>
        public double DodgeBoost { get; set; }
        /// <summary>
        /// List of all Abilities granted through this Skill.
        /// </summary>
        public List<Ability> Abilities { get; set; }

    }
}
