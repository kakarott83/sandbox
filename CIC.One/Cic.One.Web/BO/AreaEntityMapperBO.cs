using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;

namespace Cic.One.Web.BO
{
    public class AreaEntityMapperBO
    {
        private static AreaEntityMapperBO _instance;
        private Dictionary<Type, String> areaMap = new Dictionary<Type, String>();//maps BOS Class types to OL Area
        private Dictionary<String, String> luceneOLMap = new Dictionary<String, String>();//maps Lucene Area to OL Area
        private Dictionary<String, String> luceneGuiMap = new Dictionary<String, String>();//maps gui ids to lucene ids
        private Dictionary<String, String> olGuiMap = new Dictionary<String, String>();//maps ol ids to gui ids


        private AreaEntityMapperBO()
        {
            areaMap[typeof(AccountDto)]="PERSON";
            areaMap[typeof(ItDto)] = "IT";
            areaMap[typeof(PartnerDto)] = "PERSON";
            areaMap[typeof(VertragDto)] = "VT";
            areaMap[typeof(AngebotDto)] = "ANGEBOT";
            areaMap[typeof(AntragDto)] = "ANTRAG";

            luceneOLMap["INT"] = "IT";
            luceneOLMap["VERTRAG"] = "VT";
            luceneOLMap["WKTACCOUNT"] = "PERSON";


            //used when clicking on a search result to show detail
            luceneGuiMap["INT"]= "It";
            luceneGuiMap["VERTRAG"]= "Vertrag";
            luceneGuiMap["PERSON"]= "Account";
            luceneGuiMap["ANGEBOT"]= "Angebot";
            luceneGuiMap["WKTACCOUNT"]= "Wktaccount";

            olGuiMap["PERSON"]="Account";
            olGuiMap["VT"]="Vertrag";
        }

        public static AreaEntityMapperBO getInstance()
        {
            if (_instance == null)
                _instance = new AreaEntityMapperBO();

            return _instance;
        }

        /// <summary>
        /// returns the Java Entity Name (Klassname without Dto) from the Lucene Area
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public String getGuiEntityFromLucene(String entity)
        {
            if (luceneGuiMap.ContainsKey(entity))
            {
                entity = luceneGuiMap[entity];
            }
            return getEntityFromOL(entity);
        }

        /// <summary>
        /// returns the Java Entity Name (Klassname without Dto) from the OL Area
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public String getEntityFromOL(String entity)
        {
            if (olGuiMap.ContainsKey(entity))
                return olGuiMap[entity];
            return entity.Substring(0, 1).ToUpper() + entity.Substring(1).ToLower();
        }

        /// <summary>
        /// delivers the OL Area for the LUCENE Entity ID
        /// if not mapped the luceneArea is assumed to be an ol area and just returned
        /// </summary>
        /// <param name="luceneArea"></param>
        /// <returns></returns>
        public String getAreaFromLucene(String luceneArea)
        {
            if (!luceneOLMap.ContainsKey(luceneArea))
            {
                return luceneArea;
                
            }
            return luceneOLMap[luceneArea];
        }

        /// <summary>
        /// delivers the OL Area for the BOS class type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public String getArea(Type t)
        {
            if (!areaMap.ContainsKey(t))
            {

                throw new Exception("Type " + t + " has no AREA-Mapping yet");
            }
            return areaMap[t];
        }


    }
}