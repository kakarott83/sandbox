
namespace XmlConfiguratorBase.DTO
{
    public class ConstantsDto
    {
        public const bool DEBUGGING = true;
        public const string ENCODING = "UTF-8";
        public const string MAX_VERSION = "999999";
        public const string QUERY_SELECT_WFV_WFVCOMMAND = "select coderfu,codermo,wfv.syscode,wfv.befehlszeile,wfv.einrichtung,entrytype from wfv left outer join wfvcommand on wfv.befehlszeile=wfvcommand.befehlszeile where wfv.typ=1 and (wfvcommand.befehlszeile is null or (wfvcommand.webflag=1 and wfvcommand.revisionfrom<=:vers and (wfvcommand.revisionuntil>=:vers or wfvcommand.revisionuntil is null)))";
        public const string SHORT_DESC_FOR_DATABASE_INSERTION = "Angelegt durch XML Editor";
    }
}
