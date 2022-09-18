namespace Hero.Server.Core.Models
{
    public class Ability
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public Guid? CharacterId { get; set; }
        /// <summary>
        /// Indicates if the Ability is a passive Ability or one that can be actively used.
        /// </summary>
        public bool IsPassive { get; set; }
        public string Description { get; set; }

        public void Update(Ability ability)
        {
            this.IsPassive= ability.IsPassive;
            this.Description= ability.Description;
        }
    }
}
