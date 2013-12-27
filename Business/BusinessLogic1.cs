using Models;
using Models.Attributes;

namespace Business
{
    [Exposed(Name = "patients", Description = "Container for patient-related methods.")]
    public static class BusinessLogic1
    {
        [Exposed(Name = "encounters", Description = "Last three patient encounters.", Roles = new[] { "Emergency", "BookingClerk" })]
        public static BusinessModel3 Method1([Exposed(Name = "phn", Description = "Patient's PHN")]int par1, int someIntParameter)
        {
            var result = new BusinessModel3(par1);
            return result;
        }

        [Exposed(Name = "search", Description = "Patient search by a set of fixed criteria. Request example: {'fn':'Cindy', 'ln' : 'Black'}", Roles = new[] { "Emergency", "BookingClerk" })]
        public static BusinessModel1 PatientSearch([Exposed(Name = "searchRequest")]BusinessModel2 par1)
        {
            return new BusinessModel1(string.Format("Congrats! We have found: {1}, {0}", par1.FirstName, par1.LastName));
        }

        [Exposed(Name = "documents", Description = "Patient documents")]
        public static BusinessModel2 Method3([Exposed(Name = "patientID", Description = "Patient's Paris MRUN")]string par1)
        {
            var result = new BusinessModel2 { FirstName = par1 };
            return result;
        }
    }
}
