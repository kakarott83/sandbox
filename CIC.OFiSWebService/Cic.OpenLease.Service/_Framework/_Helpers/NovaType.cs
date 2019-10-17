namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Hilfsklasse zur Berechnung der Nova/Novazuschlag-Anteile aus Brutto oder Netto
    /// </summary>
    public class NovaType
    {
        public decimal netto { get; set; }
        /// <summary>
        /// inkl. nova und novazuschlag
        /// </summary>
        public decimal bruttoInklNova { get; set; }
        /// <summary>
        /// ohne nova und ohne nova zuschlag
        /// </summary>
        public decimal bruttoExklNova { get; set; }
        /// <summary>
        /// including nova and novazuschlag
        /// </summary>
        public decimal nettoInklNova { get; set; }
        public decimal novaZuschlag { get; set; }
        public decimal novaInklZuschlag { get; set; }
        //Nova without Zuschlag
        public decimal nova { get; set; }
        public decimal ust { get; set; }
        public decimal bonusmalusexklaufschlag { get; set; }
        /// <summary>
        /// Percentages
        /// </summary>
        public decimal novaPercent { get; set; }
        public decimal novaZuschlagPercent { get; set; }
        public decimal taxPercent { get; set; }
        public decimal sonderminderung { get; set; }

        /// <summary>
        /// //NOVANEU
        /// </summary>
        /// <param name="taxPercent">0-100</param>
        /// <param name="novaPercent">0-100</param>
        /// <param name="novaZuschlag">0-100</param>
        /// <param name="sonderminderung">negative for bonus, positive for malus</param>
        public NovaType(decimal taxPercent, decimal novaPercent, decimal novaZuschlag, decimal sonderminderung)
        {
            this.novaPercent = novaPercent;
            this.taxPercent = taxPercent;
            this.novaZuschlagPercent = novaZuschlag;
            this.sonderminderung = sonderminderung;
        }

      
        /// <summary>
        /// Delivers the NovaZuschlag Percent
        /// </summary>
        /// <returns></returns>
        public static decimal fetchNovaQuote()
        {

            return (decimal)new QUOTEDao().getQuote( QUOTEDao.QUOTE_NOVA_ZUSCHLAG);
          
        }

        public void setNetto(decimal value)
        {
            this.netto = value;            
            this.nova = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromNetValue(this.netto, this.novaPercent);
            this.nova += sonderminderung;
            this.novaZuschlag = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromNetValue(this.nova, this.novaZuschlagPercent);
            this.ust = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromNetValue(this.netto, this.taxPercent);
            this.bruttoInklNova = this.netto + this.ust + this.nova + this.novaZuschlag;
            this.bruttoExklNova = this.netto + this.ust;
            this.nettoInklNova = this.netto + this.nova + this.novaZuschlag;
            this.novaInklZuschlag = this.nova + this.novaZuschlag;
        }

        public decimal getSteuervorteil()
        {
            return this.nova /1.2M * 0.2M;
        }

        /// <summary>
        /// Sets Brutto inkl. Nova and incl. Sonderminderung
        /// </summary>
        /// <param name="value"></param>
        public void setBruttoInklNova(decimal value)
        {
            setNetto((value - sonderminderung) / (1 + novaPercent / 100M + novaPercent / 100M * novaZuschlagPercent / 100M + taxPercent / 100M));
        }

        public void addBonusMalus(decimal bonusmalusexklzuschlag)
        {
            this.bonusmalusexklaufschlag = bonusmalusexklzuschlag;
            decimal rwnovabonusmalusaufschlag = bonusmalusexklzuschlag * this.novaZuschlagPercent / 100;
            this.bruttoInklNova += bonusmalusexklzuschlag + rwnovabonusmalusaufschlag;
            this.bruttoExklNova += bonusmalusexklzuschlag;
        }
       

        public NovaType getWithNachlass(decimal nachlass)
        {
            NovaType rval = new NovaType(taxPercent, novaPercent, novaZuschlag, sonderminderung);
            rval.setBruttoInklNova(this.bruttoInklNova - nachlass);
            return rval;
        }

        public NovaType getWithNachlassPercent(decimal nachlassProzent)
        {
            NovaType rval = new NovaType(taxPercent, novaPercent, novaZuschlag, sonderminderung);
            rval.setBruttoInklNova(this.bruttoInklNova - this.bruttoInklNova * nachlassProzent / 100M);
            return rval;
        }
    }
}