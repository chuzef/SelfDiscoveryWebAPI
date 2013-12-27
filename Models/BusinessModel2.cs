using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class BusinessModel2
    {
        [DataMember(Name = "fn")]
        public string FirstName { get; set; }

        [DataMember(Name = "ln")]
        public string LastName { get; set; }
    }
}
