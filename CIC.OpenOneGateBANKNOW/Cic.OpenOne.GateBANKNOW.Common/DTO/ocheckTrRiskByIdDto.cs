using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{

    public class ocheckTrRiskByIdDto
    {

        [AttributeLabel("ANTRAGSDATEN")]
        public ELInDto AntragsDaten
        {
            get;
            set;
        }


        public FaktorenDto Faktoren
        {
            get;
            set;
        }

        public Cic.OpenOne.GateBANKNOW.Common.DTO.VariantenParam VarianteTSParam
        {

            get;
            set;
        }

        [AttributeLabel("Explos für Ursprungskalkulation")]
        public Cic.OpenOne.GateBANKNOW.Common.DTO.Ursprungskalkulation Ursprungskalkulation
        {
            get;
            set;
        }

        public Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI0ANZ KOMBI0ANZ
        {
            get;
            set;
        }


        public Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI0 KOMBI0
        {
            get;
            set;
        }


        public Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI1 KOMBI1
        {
            get;
            set;
        }

        [AttributeLabel("Explos für variante1")]
        public Cic.OpenOne.GateBANKNOW.Common.DTO.Variante1 variante1
        {
            get;
            set;
        }

        [AttributeLabel("Explos für variante2")]
        public Cic.OpenOne.GateBANKNOW.Common.DTO.Variante2 variante2
        {
            get;
            set;
        }

        [AttributeLabel("Explos für variante2")]
        public Cic.OpenOne.GateBANKNOW.Common.DTO.Freivariante freivariante
        {
            get;
            set;
        }

        [AttributeLabel("Cluster Parameter")]
        public VClusterParamDto clusterParam
        {
            get;
            set;
        }

        /// <summary>
        /// Frontid results
        /// </summary>
        public string frontid
        {
            get;
            set;



        }
    }

}
