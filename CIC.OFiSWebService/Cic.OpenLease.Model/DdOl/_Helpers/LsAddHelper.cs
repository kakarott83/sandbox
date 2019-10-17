namespace Cic.OpenLease.Model.DdOl
{
    public static class LsAddHelper
    {
        public static decimal DeliverUst(Cic.OpenLease.Model.DdOl.OlExtendedEntities context)
        {
            return 16.9M;
        }

        public static decimal DeliverNova(Cic.OpenLease.Model.DdOl.OlExtendedEntities context)
        {
            return 0M;
        }

        public static decimal DeliverNetto(decimal bruttoValue)
        {
            return bruttoValue - ((bruttoValue * DeliverUst(null)) /100);
        }

        public static decimal DeliverBrutto(decimal nettoValue)
        {
            return nettoValue + ((nettoValue * DeliverUst(null)) / 100);
        }

        public static decimal DeliverUst(decimal bruttoValue)
        {
            return (bruttoValue * DeliverUst(null)) / 100;
        }

        public static decimal DeliverRestwertvorschlagBrutto(Cic.OpenLease.Model.DdOl.OlExtendedEntities context)
        {
            return 10000M;
        }
    }
}
