using System.Globalization;
using Models;
using Models.Attributes;

namespace Business
{
    [Exposed(Name = "patients", Description = "Container for patient-related methods.")]
    public static class BusinessLogic1
    {
        [Exposed(Name = "encounters", Description = "Last three patient encounters")]
        public static BusinessModel3 Method1([Exposed(Name = "patientID", Description = "Patient's PHN")]int par1, int someIntParameter)
        {
            var result = new BusinessModel3(par1);
            return result;
        }

        [Exposed(Name = "documents", Description = "Patient documents")]
        public static BusinessModel2 Method3([Exposed(Name = "patientID", Description = "Patient's Paris MRUN")]string par1)
        {
            var result = new BusinessModel2(par1);
            return result;
        }

        public static BusinessModel3 Method2([Exposed(Name = "par1")]int par1)
        {
            return new BusinessModel3(par1);
        }
    }
}
