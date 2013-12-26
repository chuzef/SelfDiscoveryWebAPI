using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class BusinessModel1
    {
        [DataMember]
        public string Property1 { get; private set; }

        public BusinessModel1(string prop1)
        {
            this.Property1 = prop1;
        }
    }
}
