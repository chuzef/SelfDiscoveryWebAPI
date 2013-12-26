using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class BusinessModel2
    {
        [DataMember]
        public string Property1 { get; private set; }

        public BusinessModel2(string prop1)
        {
            this.Property1 = prop1;
        }
    }
}
