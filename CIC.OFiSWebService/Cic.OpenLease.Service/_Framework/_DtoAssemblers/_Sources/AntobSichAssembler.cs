namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System.Linq;
    #endregion

    public class AntobSichAssembler
    {
        #region Methods
        public AntObSichDto ConvertToDto(ANTOBSICH fromAntObSich, DdOlExtended context)
        {
            return MyMap(fromAntObSich, context);
        }
        #endregion

        #region My methods
        private AntObSichDto MyMap(ANTOBSICH fromAntObSich, DdOlExtended context)
        {
            // Query PERSON
            var CurrentPerson = (from Person in context.PERSON
                                 where Person.SYSPERSON == fromAntObSich.SYSPERSON
                                 select Person).FirstOrDefault();


            AntObSichDto Result = new AntObSichDto();

            if (CurrentPerson != null)
            {
                Result.AntobSichSysMh = fromAntObSich.SYSMH;
                Result.PersonNameVornameGebDatum = CurrentPerson.NAME + " " + CurrentPerson.VORNAME + " " + CurrentPerson.GEBDATUM.GetValueOrDefault().ToShortDateString();
            }

            return Result;
        }
        #endregion
    }
}