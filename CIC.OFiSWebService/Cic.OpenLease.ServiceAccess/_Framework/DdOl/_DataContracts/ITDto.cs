// OWNER BK, 15-06-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Interessenten
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
	public class ITDto
	{
		#region Constructors
		// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
		/// <summary>
		/// Konstruktor, ohne Bestandteile
		/// </summary>
		public ITDto()
		{
		}

		// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
		/// <summary>
		/// Konstruktor, Grundbestandteil
		/// </summary>
		public ITDto(Cic.OpenLease.Model.DdOl.IT it)
			: this(it, null, null, null, null, null, null, null, null, null, null, null, null, null)
		{
		}

		// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
		/// <summary>
		/// Konstruktor, alle Bestandteile
		/// </summary>
		public ITDto(Cic.OpenLease.Model.DdOl.IT it, Cic.OpenLease.Model.DdOl.PERSON person, Cic.OpenLease.Model.DdOl.BRANCHE branche, Cic.OpenLease.Model.DdOw.CTLANG ctlang, Cic.OpenLease.Model.DdOl.LAND land, Cic.OpenLease.Model.DdOl.STAAT staat, Cic.OpenLease.Model.DdOl.LAND landNAT, Cic.OpenLease.Model.DdOw.CTLANG ctlangKONT, Cic.OpenLease.Model.DdOl.LAND landAG1, Cic.OpenLease.Model.DdOl.STAAT staatAG1, Cic.OpenLease.Model.DdOl.LAND landAG2, Cic.OpenLease.Model.DdOl.STAAT staatAG2, Cic.OpenLease.Model.DdOl.LAND land2, Cic.OpenLease.Model.DdOl.STAAT staat2)
		{
			this.IT = it;
			this.PERSON = person;
			this.BRANCHE = branche;
			this.CTLANG = ctlang;
			this.LAND = land;
			this.STAAT = staat;
			this.LANDNAT = landNAT;
			this.CTLANGKONT = ctlangKONT;
			this.LANDAG1 = landAG1;
			this.STAATAG1 = staatAG1;
			this.LANDAG2 = landAG2;
			this.STAATAG2 = staatAG2;
            this.LAND2 = land2;
            this.STAAT2 = staat2;
		}
		#endregion

		#region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.IT"/>.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.IT IT
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.PERSON"/>.
        /// Personenverknüpfung
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.PERSON PERSON
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil<see cref="Cic.OpenLease.Model.DdOl.BRANCHE"/>.
		/// Branche
        /// Für die Suche bei Speichern wird BRANCHE.BEZEICHNUNG benutzt.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.BRANCHE BRANCHE
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOw.CTLANG"/>.
        /// Sprache
        /// Für die Suche bei Speichern wird CTLANG.LANGUAGENAME benutzt.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOw.CTLANG CTLANG
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.LAND"/>.
        /// Staat
        /// Für die Suche bei Speichern wird LAND.COUNTRYNAME benutzt.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.LAND LAND
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.LAND"/>.
        /// Staat 2
        /// Für die Suche bei Speichern wird LAND.COUNTRYNAME benutzt.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.LAND LAND2
        {
            // TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
            // TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
            // TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
            get;
            // TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
            // TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
            set;
        }

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.STAAT"/>.
		/// Bundesland, Kanton
        /// Für die Suche bei Speichern wird STAAT.STAAT1 benutzt.
        /// Feld STAAT.STAAT1 ist die Staat Name.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.STAAT STAAT
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

            /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.STAAT"/>.
		/// Bundesland, Kanton 2
        /// Für die Suche bei Speichern wird STAAT.STAAT1 benutzt.
        /// Feld STAAT.STAAT1 ist die Staat Name.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.STAAT STAAT2
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.LAND"/>.
        /// Nationalität
        /// Für die Suche bei Speichern wird LAND.COUNTRYNAME benutzt.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.LAND LANDNAT
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOw.CTLANG"/>.
        /// Ansprechpartner, Sprache
        /// Für die Suche bei Speichern wird CTLANG.LANGUAGENAME benutzt.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOw.CTLANG CTLANGKONT
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.LAND"/>.
		/// Ersten Arbeitgeber, Staat
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.LAND LANDAG1
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.STAAT"/>.
		/// Ersten Arbeitgeber, Bundesland, Kanton
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.STAAT STAATAG1
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.LAND"/>.
		/// Zweiter Arbeitgeber, Staat
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.LAND LANDAG2
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.STAAT"/>.
		/// Zweiter Arbeitgeber, Bundesland, Kanton
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.STAAT STAATAG2
		{
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithAllParameters
			get;
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithoutParameters
			// TESTED BY ITDtoTestFixture.CheckPropertiesConstructorWithOneParameter
			set;
		}
		#endregion
	}
}
