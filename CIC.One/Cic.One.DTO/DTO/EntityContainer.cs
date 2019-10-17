using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;
using Cic.One.DTO.BN;

namespace Cic.One.DTO
{
    /// <summary>
    /// Container holding all Entities supported by the application 
    /// All this entities are acessible inside a workflow
    /// 
    /// The name of the variables declared must be the lower-case complete string before Dto, e.g. OpportunityDto leads to variable name opportunity
    /// </summary>
    public class EntityContainer
    {
        public StaffelpositionstypDto staffelpositionstyp { get; set; }
        public StaffeltypDto staffeltyp { get; set; }
        public RolleDto rolle { get; set; }
        public RollentypDto rollentyp { get; set; }
        public HandelsgruppeDto handelsgruppe { get; set; }
        public VertriebskanalDto vertriebskanal { get; set; }
        public BrandDto brand { get; set; }
        public RechnungDto rechnung { get; set; }
        public AngobbriefDto angobbrief { get; set; }
		public ZahlungsplanDto zahlungsplan { get; set; }
		public KreditlinieDto kreditlinie { get; set; }
		public FahrzeugbriefDto fahrzeugbrief { get; set; }
        public KalkDto kalk { get; set; }
        public PersonDto person { get; set; }
        public AccountDto account { get; set; }
        //need this fields else a relation-config with same entityalias account would always restore from account
        public AccountDto ma { get; set; }
        public AccountDto vp { get; set; }
        public AccountDto vm { get; set; }
        public WktaccountDto wktaccount { get; set; }

    //{ get { if (_account == null) _account = new AccountDto(); return _account; } set { _account = value; } }
        //private AccountDto _account;

        public AdresseDto adresse{ get; set; }
//{ get { if (_adresse == null) _adresse = new AdresseDto(); return _adresse; } set { _adresse = value; } }
        //private AdresseDto _adresse;
        public AngebotDto angebot { get; set; }
        public AngkalkDto angkalk { get; set; }
        public AngobDto angob { get; set; }
        public AngobslDto angobsl { get; set; }
        public AngobslposDto angobslpos { get; set; }
        public AngvarDto angvar { get; set; }
        public OppotaskDto oppotask { get; set; }
        public AntobDto antob { get; set; }
        public AntobslDto antobsl { get; set; }
        public AntobslposDto antobslpos { get; set; }
        public ApptmtDto apptmt { get; set; }
        public PtrelateDto ptrelate { get; set; }
        public BeteiligterDto beteiligter { get; set; }
        public CampDto camp { get; set; }
        public ContactDto contact { get; set; }
        public KontoDto konto { get; set; }
        public MailmsgDto mailmsg { get; set; }
        public MemoDto memo { get; set; }
        public ObDto ob { get; set; }
        public OpportunityDto opportunity { get; set; }
        public PartnerDto partner { get; set; }
        public PrproductDto prproduct { get; set; }
        public PtaskDto ptask { get; set; }
        public RecurrDto recurr { get; set; }
        public ReminderDto reminder { get; set; }
        public VorgangDto vorgang { get; set; }
        public VtobslposDto vtobslpos { get; set; }
        public VtobslDto vtobsl { get; set; }
        public WfuserDto wfuser { get; set; }
        public StickynoteDto stickynote { get; set; }
        public NotizDto notiz { get; set; }
        public PrkgroupDto prkgroup { get; set; }
        public VertragDto vertrag {get;set;}
        public RahmenDto rahmen {get;set;}
        public ObjektDto objekt { get; set; }
        public ItDto it { get; set; }
        public MycalcDto mycalc { get; set; }
        public MycalcfsDto mycalcfs { get; set; }
        public RecalcDto recalc { get; set; }
        public ObtypDto obtyp { get; set; }
        public FinanzierungDto finanzierung { get; set; }
        public GviewDto gview { get; set; }
        public ZekDto zek { get; set; }
        public BNAngebotDto BNAngebot { get; set; }
        public BNAntragDto BNAntrag { get; set; }
        public EaihotDto eaihot { get; set; }

		public PreadDto pread { get; set; }
	}
}
