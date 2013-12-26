namespace SelfDiscoveryWebAPI.Models
{
    public class ExposedMethodInfo
    {
        public string UrlTemplate { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Description { get; set; }
        public ExposedParameterInfo[] Parameters { get; set; }
        public string ReturnType { get; set; }

        public class ExposedParameterInfo
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
        }
    }
}