using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
    /// <summary>
    /// StatusEPOSBo-Klasse
    /// </summary>
    public class StatusEPOSBo
    {
        /// <summary>
        /// setStatusEPOS
        /// </summary>
        /// <param name="angebot"></param>
        public static void setStatusEPOS(AngAntDto angebot)
        {
            if (angebot.zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Neu)) && angebot.attribut.Equals(AngebotAttribut.NeuGueltig.ToString()))
                angebot.zustand = Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Gueltig);
            else
            {
                if (angebot.zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Neu)) && angebot.attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Abgelaufen)))
                    angebot.zustand = Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Abgelaufen);
                else
                {
                    if (angebot.zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Gedruckt)) && angebot.attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.GedrucktGueltig)))
                        angebot.zustand = Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Gedruckt);
                    else
                    {
                        if (angebot.zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Gedruckt)) && angebot.attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Abgelaufen)))
                            angebot.zustand = Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Abgelaufen);
                        else
                        {
                            if (angebot.zustand.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Abgeschlossen)) && angebot.attribut.Equals(Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Antrageingereicht)))
                                angebot.zustand = Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Antrageingereicht);
                        }
                    }
                }
            }
        }
    }
}