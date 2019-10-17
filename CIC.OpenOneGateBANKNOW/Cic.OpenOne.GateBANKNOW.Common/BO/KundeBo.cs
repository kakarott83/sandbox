using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Das KundeBo liefert und empfängt das KundeDto um dies ans DAO zum Speichern oder neu Erstellen weiter zu leiten.
    /// </summary>
    public class KundeBo : AbstractKundeBo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao"></param>
        public KundeBo(IKundeDao pDao)
            : base(pDao)
        {
        }

        /// <summary>
        /// createOrUpdateKunde erhält ein KundeDto welches nur eine SYSIT beinhaltet. Anhand der SYSIT wird differenziert zwischen Neuanlage oder Update.
        /// </summary>
        /// <param name="kunde">KundeDto welches eine SYSIT beinhalten muss</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto mit mindestens einer SYSIT</returns>
        public override KundeDto createOrUpdateKunde(KundeDto kunde, long sysperole)
        {
            if (kunde.sysit == 0)
            {
                return createKunde(kunde, sysperole);

            }
            else
            {
                return updateKunde(kunde);
            }
        }

        /// <summary>
        /// createOrUpdateKunde erhält ein KundeDto welches nur eine syskd beinhaltet. Anhand der syskd wird differenziert zwischen Neuanlage oder Update.
        /// </summary>
        /// <param name="kunde">KundeDto welches eine syskd beinhalten muss</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto mit mindestens einer syskd</returns>
        public override KundeDto createOrUpdateKundePerson(KundeDto kunde, long sysperole)
        {
            if (kunde.syskd == 0)
            {
                return createKundePerson(kunde, sysperole);

            }
            else
            {
                return updateKundePerson(kunde);
            }
        }

        /// <summary>
        /// createKunde ruft das KundeDAO auf und führt die createKunde Methode aus
        /// </summary>
        /// <param name="kundeInput">KundenDto mit einer SYSIT = 0</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>ein KundeDto mit einer SYSIT vom der Datenbank (rest leer)</returns>
        public KundeDto createKunde(KundeDto kundeInput, long sysperole)
        {
            return this.kundeDao.createKunde(kundeInput, sysperole);
        }

        /// <summary>
        /// updateKunde ruft das KundeDAO auf und führt die updateKunde Methode aus
        /// </summary>
        /// <param name="kunde">KundeDto mit einer SYSIT > 0</param>
        /// <returns>KundeDto mit gefüllten Werten und der gleichen SYSIT</returns>
        public KundeDto updateKunde(KundeDto kunde)
        {
            return kundeDao.updateKunde(kunde);
        }

        /// <summary>
        /// createKunde ruft das KundeDAO auf und führt die createKunde Methode aus
        /// </summary>
        /// <param name="kundeInput">KundenDto mit einer syskd = 0</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>ein KundeDto mit einer syskd vom der Datenbank (rest leer)</returns>
        public KundeDto createKundePerson(KundeDto kundeInput, long sysperole)
        {
            return this.kundeDao.createKundePerson(kundeInput, sysperole);
        }

        /// <summary>
        /// updateKunde ruft das KundeDAO auf und führt die updateKunde Methode aus
        /// </summary>
        /// <param name="kunde">KundeDto mit einer syskd > 0</param>
        /// <returns>KundeDto mit gefüllten Werten und der gleichen syskd</returns>
        public KundeDto updateKundePerson(KundeDto kunde)
        {
            return kundeDao.updateKundePerson(kunde);
        }

        /// <summary>
        /// Adresse via Interessenten ID holen
        /// </summary>
        /// <param name="sysit">Interessenten ID</param>
        /// <returns>Daten</returns>
        public override AdresseDto[] getAdressen(long sysit)
        {
            return kundeDao.getAdressen(sysit);
        }

        /// <summary>
        /// Kontendaten via Interessenten ID holen
        /// </summary>
        /// <param name="sysit">Interessenten ID</param>
        /// <returns>Daten</returns>
        public override KontoDto[] getKonten(long sysit)
        {
            return kundeDao.getKonten(sysit);
        }

        /// <summary>
        /// getKunde ruft das KundeDAO auf und führt die getKunde Methode aus
        /// </summary>
        /// <param name="sysit">SYSIT des Kunden</param>
        /// <returns>Liefert ein KundeDto mit der gleichen SYSIT zurück</returns>
        public override KundeDto getKunde(long sysit)
        {
            return kundeDao.getKunde(sysit);
        }

        /// <summary>
        /// getKunde ruft das KundeDAO auf und führt die getKunde Methode aus
        /// </summary>
        /// <param name="sysit">SYSIT des Kunden</param>
        /// <param name="sysantrag">Antrag ID</param>
        /// <returns>Liefert ein KundeDto mit der gleichen SYSIT zurück</returns>
        public override KundeDto getKundeViaAntragID(long sysit, long sysantrag)
        {
            return kundeDao.getKundeViaAntragID(sysit, sysantrag);
        }

        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ for the antrag, assuming the pkz/ukz is already there
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        public override void transferITPKZUKZToPERSON(long syskd, long sysit, long sysantrag)
        {
            this.kundeDao.transferITPKZUKZToPERSON(syskd, sysit, sysantrag);
        }


        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ for the antrag, assuming the pkz/ukz is already there
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        public override void transferITPKZUKZToPKZUKZ(long syskd, long sysit, long sysantrag)
        {
            this.kundeDao.transferITPKZUKZToPKZUKZ(syskd, sysit, sysantrag);
        }

        /// <summary>
        /// aktuellsten Mitantragsteller laden
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public override long getMitantragsteller(long sysit)
        {
            return this.kundeDao.getMitantragsteller(sysit);
        }

        
    }
}
