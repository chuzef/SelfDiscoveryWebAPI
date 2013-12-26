using System;

namespace Models.Attributes
{
    public class ExposedAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Roles { get; set; }
    }
}
