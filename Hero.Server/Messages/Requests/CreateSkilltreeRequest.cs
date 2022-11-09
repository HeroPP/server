﻿using System.ComponentModel.DataAnnotations;

namespace Hero.Server.Messages.Requests
{
    public class CreateSkilltreeRequest
    {
        public Guid? CharacterId { get; set; }
        [Required]
        public string Name { get; set; }
        public int Points { get; set; }
        public List<SkilltreeNodeRequest> Nodes { get; set; }
    }
}
