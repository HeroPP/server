using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hero.Server.Core.Models
{
    public class Hero
    {
        public Guid Id { get; set; }
        public List<NodeTree> ActiveNodeTrees { get; set; }
        public List<NodeTree> NodeTrees { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HealthPoints { get; set; }
        public int LightPoints { get; set; }
        public double MovementSpeed { get; set; }
        public double Resistance { get; set; }
        public double OpticalRange { get; set; }
        public double MeleeDamageBuff { get; set; }
        public double RangeDamageBuff { get; set; }
        public double LightDamageBuff { get; set; }
        public double DamageBuff { get; set; }
        public double Parry { get; set; }
        public double Dodge { get; set; }
        public List<Ability> Abilities { get; set; }
    }
}
