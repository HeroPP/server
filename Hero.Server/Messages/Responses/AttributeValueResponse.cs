﻿namespace Hero.Server.Messages.Responses
{
    public class AttributeValueResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public double StepSize { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double Value { get; set; }
    }
}