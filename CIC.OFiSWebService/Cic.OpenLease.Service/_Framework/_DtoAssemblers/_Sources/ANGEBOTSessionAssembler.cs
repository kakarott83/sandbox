// OWNER JJ, 22-04-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Util.ExtensionOld;
    using System;
    using System.Data.Objects;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class AngebotSessionAssembler : IDtoAssembler<AngebotSessionDto, PSESSION>
    {
        #region Private variables
        private System.Collections.Generic.Dictionary<string, string> _Errors;
        private long? _SysPUSER;
        private Cic.OpenLease.Model.DdOw.OwExtendedEntities _context;
        #endregion

        #region Constructors
        public AngebotSessionAssembler(long? sysPUSER, Cic.OpenLease.Model.DdOw.OwExtendedEntities context)
        {
            _SysPUSER = sysPUSER;
            _context = context;
            _Errors = new System.Collections.Generic.Dictionary<string, string>();
        }
        #endregion

        #region IDtoAssembler<AngebotSessionDto, PSESSION> Members (Methods)
        public bool IsValid(AngebotSessionDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            // Otymistic
            bool IsValid = true;


            // Check if _SysPERSONInPEROLE is not null
            if (IsValid && !_SysPUSER.HasValue)
            {
                _Errors.Add(Cic.OpenLease.Model.DdOl.IT.FieldNames.SYSIT.ToString(), "Not exists in sight field. SysPERSONInPEROLE is null.");
                IsValid = false;
            }


            return IsValid;
        }

        public Cic.OpenLease.Model.DdOw.PSESSION CreateOrUpdate(Cic.OpenLease.ServiceAccess.DdOl.AngebotSessionDto angebotSessionDto, bool withGUID)
        {
            //Get saved Session
            Cic.OpenLease.Model.DdOw.PSESSION OriginalSession = null;
            Cic.OpenLease.Model.DdOw.PSESSION ModifiedPSESSION;
            string orgguid = angebotSessionDto.Sid;
            string guid = _SysPUSER +"_"+ angebotSessionDto.Sid;
            angebotSessionDto.Sid = guid;

            var QueryPSESSIONGUID = from session in _context.PSESSION.Include("PUSER")
                                    where (session.PUSER.SYSPUSER == _SysPUSER && session.GUIDCODE == angebotSessionDto.Sid)
                                select session;

            var QueryPSESSION = from session in _context.PSESSION.Include("PUSER")
                                    where (session.PUSER.SYSPUSER == _SysPUSER )
                                    select session;
            if(withGUID)
                OriginalSession = QueryPSESSIONGUID.FirstOrDefault();
            else 
                OriginalSession = QueryPSESSION.FirstOrDefault();


            if (OriginalSession == null) //There is no saved session with session.Guidcode = angebotSessionDto.Sid for this PUSer 
            {
                // Create
                ModifiedPSESSION = Create(angebotSessionDto);
            }
            else
            {
                //Update
                ModifiedPSESSION = UpdateTemp(angebotSessionDto, OriginalSession);
            }
            angebotSessionDto.Sid = orgguid;

            return ModifiedPSESSION;
        }

        public PSESSION Create(AngebotSessionDto dto)
        {
            PSESSION NewPSESSION;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            NewPSESSION = new PSESSION();

            // Map
            MyMap(dto, NewPSESSION);



            //TRANSACTIONS
           // using (System.Transactions.TransactionScope TransactionScope = new System.Transactions.TransactionScope())
            {
                NewPSESSION.PUSERReference.EntityKey = new System.Data.EntityKey("OwEntities.PUSER", "SYSPUSER", _SysPUSER);
                _context.PSESSION.Add(NewPSESSION);

                // Save changes
                _context.SaveChanges(); 

                _context.AcceptAllChanges();
                // Set transaction complete
                //TransactionScope.Complete();
            }


            return NewPSESSION;
        }

        public PSESSION UpdateTemp(AngebotSessionDto dto, PSESSION OriginalPSESSION)
        {
           
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }


            //Load PUSER reference
            if (!OriginalPSESSION.PUSERReference.IsLoaded)
            {
                OriginalPSESSION.PUSERReference.Load();
            }

            _context.Refresh(RefreshMode.ClientWins, OriginalPSESSION);
            // Map
            MyMap(dto, OriginalPSESSION);
            
            _context.SaveChanges();
           

            return OriginalPSESSION;

        }


        public PSESSION Update(AngebotSessionDto dto)
        {
            PSESSION OriginalPSESSION;
            PSESSION ModifiedPSESSION;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }


            //Get PSESSION object to update
            var QueryPSESSION = from session in _context.PSESSION.Include("PUSER")
                                where session.PUSER.SYSPUSER == _SysPUSER
                                select session;

            OriginalPSESSION = QueryPSESSION.FirstOrDefault();

            //Load PUSER reference
            if (!OriginalPSESSION.PUSERReference.IsLoaded)
            {
                OriginalPSESSION.PUSERReference.Load();
            }

            // Map
            MyMap(dto, OriginalPSESSION);

            // Update (with Save changes)
            ModifiedPSESSION = _context.Update< PSESSION>(OriginalPSESSION, null);


            return ModifiedPSESSION;

        }

        public AngebotSessionDto ConvertToDto(PSESSION domain)
        {
            AngebotSessionDto AngebotSessionDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            AngebotSessionDto = new AngebotSessionDto();
            MyMap(domain, AngebotSessionDto);

            return AngebotSessionDto;
        }

        public PSESSION ConvertToDomain(AngebotSessionDto dto)
        {
            PSESSION PSESSION;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            PSESSION = new PSESSION();
            MyMap(dto, PSESSION);

            return PSESSION;
        }
        #endregion

        #region IDtoAssembler<ITDto,IT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get
            {
                return _Errors;
            }
        }
        #endregion

        #region My methods
        private void MyMap(AngebotSessionDto fromngebotSessionDto, PSESSION toPSESSION)
        {
            // Mapping
            toPSESSION.SESSIONCONTENT = MySerializeObject(fromngebotSessionDto);
            toPSESSION.CREATEDATE = DateTime.Now;
            toPSESSION.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
            toPSESSION.GUIDCODE = fromngebotSessionDto.Sid;
        }

        private void MyMap(PSESSION fromPSESSION, AngebotSessionDto toAngebotSessionDto)
        {
            // Mapping
            AngebotSessionDto Temp = (AngebotSessionDto)MyDeserializeObject(fromPSESSION.SESSIONCONTENT);
            toAngebotSessionDto.ANGEBOTDto = Temp.ANGEBOTDto;
            toAngebotSessionDto.Mitantragsteller = Temp.Mitantragsteller;
            toAngebotSessionDto.Step = Temp.Step;
            toAngebotSessionDto.Sid = fromPSESSION.GUIDCODE;
            if (fromPSESSION.GUIDCODE != null && fromPSESSION.GUIDCODE.IndexOf("_") > -1)
            {
                toAngebotSessionDto.Sid = fromPSESSION.GUIDCODE.Substring(fromPSESSION.GUIDCODE.IndexOf("_")+1);
            }
            
        }

        public byte[] MySerializeObject(Object pObject)
        {
            //Serialize object to xml in bytes to write in blob
            System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream();
            System.Text.UTF8Encoding Encoding = new System.Text.UTF8Encoding();
            System.Xml.Serialization.XmlSerializer XmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(AngebotSessionDto));
            System.Xml.XmlTextWriter XmlTextWriter = new System.Xml.XmlTextWriter(MemoryStream, System.Text.Encoding.UTF8);
            XmlSerializer.Serialize(XmlTextWriter, pObject);
            MemoryStream = (System.IO.MemoryStream)XmlTextWriter.BaseStream;

            return MemoryStream.ToArray();
        }

        public Object MyDeserializeObject(byte[] content)
        {
            //Deserialize from blob to object
            System.Xml.Serialization.XmlSerializer XmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(AngebotSessionDto));
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(content);
            System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(memoryStream, System.Text.Encoding.UTF8);

            object Result = XmlSerializer.Deserialize(memoryStream);

            return Result;
        }

        #endregion
    }
}