using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstrakte Klasse von KundeBo
    /// </summary>
    public abstract class AbstractKundeBo : IKundeBo
    {
                /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IKundeDao kundeDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="kundeDao"></param>
        public AbstractKundeBo(IKundeDao kundeDao)
        {
            this.kundeDao = kundeDao;
        }

        /// <summary>
        /// createOrUpdateKunde Methode
        /// </summary>
        /// <param name="kunde">KundeDto</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto</returns>
        public abstract KundeDto createOrUpdateKunde(KundeDto kunde, long sysperole);

        /// <summary>
        /// createOrUpdateKunde Methode
        /// </summary>
        /// <param name="kunde">KundeDto</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto</returns>
        public abstract KundeDto createOrUpdateKundePerson(KundeDto kunde, long sysperole);

        /// <summary>
        /// Adresse via ID auslesen
        /// </summary>
        /// <param name="sysit">Primary key</param>
        /// <returns>Adressen Datenstruktur</returns>
        public abstract AdresseDto[] getAdressen(long sysit);

        /// <summary>
        /// Kontendaten via ID auslesen
        /// </summary>
        /// <param name="sysit">Primary key</param>
        /// <returns>Konto Datenstuktur</returns>
        public abstract KontoDto[] getKonten(long sysit);

        /// <summary>
        /// Kunde via ID auslesen
        /// </summary>
        /// <param name="sysit">Primary key</param>
        /// <returns>Kunden Datenstruktur</returns>
        public abstract KundeDto getKunde(long sysit);

        /// <summary>
        /// getKunde ruft das KundeDAO auf und führt die getKunde Methode aus
        /// </summary>
        /// <param name="sysit">SYSIT des Kunden</param>
        /// <param name="sysantrag">Antrag ID</param>
        /// <returns>Liefert ein KundeDto mit der gleichen SYSIT zurück</returns>
        public abstract KundeDto getKundeViaAntragID(long sysit, long sysantrag);

        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ for the antrag, assuming the pkz/ukz is already there
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        public abstract void transferITPKZUKZToPERSON(long syskd, long sysit, long sysantrag);


        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ for the antrag, assuming the pkz/ukz is already there
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        public abstract void transferITPKZUKZToPKZUKZ(long syskd, long sysit, long sysantrag);

        /// <summary>
        /// aktuellsten Mitantragsteller laden
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns>mitantragsteller</returns>
        public abstract long getMitantragsteller(long sysit);
    }
}
