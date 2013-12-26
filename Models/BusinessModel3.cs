using System.Runtime.Serialization;

namespace Models
{
    [DataContract(Name = "model3")]
    public class BusinessModel3
    {
        [DataMember(Name="prop1")]
        public int Property1 { get; private set; }

        public BusinessModel3(int prop1)
        {
            this.Property1 = prop1;
        }
    }
}
