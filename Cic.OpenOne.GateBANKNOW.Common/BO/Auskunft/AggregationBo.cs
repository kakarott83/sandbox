using System;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Aggregation Business Object
    /// </summary>
    public class AggregationBo : AbstractAggregationBo
    {
        private const double Default = 0.0;

        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Aggregation Business Object
        /// </summary>
        /// <param name="aggregationDao">Aggregation DAO</param>
        /// <param name="auskunftdao"></param>
        public AggregationBo(IAggregationDao aggregationDao, IAuskunftDao auskunftdao)
            : base(aggregationDao, auskunftdao)
        {
            
        }

        /// <summary>
        /// Fills in_Value array with values from InDto c
        /// AUSKUNFT, AggregationINP, Aggregation, AggregationOUT will be created
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>AggregationDto, filled with values returned by Aggregation Webservice</returns>
        public override AuskunftDto callByValues(AggregationInDto inDto)
        {
            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.AggregationCallByValues);

            // Save AggregationInp
            aggregationDao.SaveAggregationInDto(sysAuskunft, inDto);

            // Die Id des Antragstellers aus der RatingAuskunft holen
            aggregationDao.GetAntragstellerInfo(sysAuskunft, inDto.AntragID);

            AggregationOutDto outDto = new AggregationOutDto();
            
            // Aggregation "web service" Call
            outDto.aggOLOutDto = aggregationDao.GetOLDatenBySysAntrag(sysAuskunft, inDto.AntragID);
            outDto.aggVPOutDto = aggregationDao.GetVPDatenBySysAntrag(sysAuskunft, inDto.AntragID);
            if (inDto.DeltaVistaID > 0)
            {
                outDto.aggDVOutDto = aggregationDao.GetDVDatenBySysAntrag(sysAuskunft, inDto.AntragID);
            }

            outDto.aggZEKOutDto = aggregationDao.GetZEKDatenBySysAntrag(sysAuskunft, inDto.AntragID);

            outDto.ReturnCode = 0;
            
            // Update Auskunft
            auskunftDao.UpdateAuskunft(sysAuskunft, outDto.ReturnCode);

            // Save AggregationOut
            aggregationDao.SaveAggregationOutDto(outDto, sysAuskunft);

            AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
            auskunftDto.AggregationOutDto = outDto;
            auskunftDto.requestXML =  XMLSerializer.SerializeUTF8WithoutNamespace(inDto);
            auskunftDto.responseXML = XMLSerializer.SerializeUTF8WithoutNamespace(outDto);
            return auskunftDto;
        }

        /// <summary>
        /// Fills in_Value array with values from InDto and calls AggregationWSDao method CallAggregationByValues()
        /// AUSKUNFT, AggregationINP, Aggregation already exist, only a Auskunft and Aggregation will be updated
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AggregationDto, filled with values returned by Aggregation Webservice</returns>
        public override AuskunftDto callByValues(long sysAuskunft)
        {
            DateTime startTime = DateTime.Now;
            
            
            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
            _log.Info("AGGSQL AuskunftDao.FindBySysId Query duration : " + (TimeSpan)(DateTime.Now - startTime));
            startTime = DateTime.Now;

            // Get AggregationInDto
            AggregationInDto inDto = aggregationDao.FindBySysId(sysAuskunft);
            auskunftdto.AggregationInDto = inDto;
            _log.Info("AGGSQL AggregationDao.FindBySysId Query duration : " + (TimeSpan)(DateTime.Now - startTime));

            // Die AntragstellerInfo holen
            aggregationDao.GetAntragstellerInfo(sysAuskunft, inDto.AntragID);

            AggregationOutDto outDto = new AggregationOutDto();

            // Wenn InDto null ist, muss trotzdem eine leere Outputstruktur (AGGOUT, AGGOUTOL,...) angelegt werden, 
            // denn sonst funktionieren Clarion-Scripte nicht richtig
            if (inDto != null)
            {
                // Aggregation "web service" Call
                startTime = DateTime.Now;
                outDto.aggOLOutDto = aggregationDao.GetOLDatenBySysAntrag(sysAuskunft, inDto.AntragID);
                _log.Info("aggregationDao.GetOLDatenBySysAntrag Call duration : " + (TimeSpan)(DateTime.Now - startTime));

                startTime = DateTime.Now;
                outDto.aggVPOutDto = aggregationDao.GetVPDatenBySysAntrag(sysAuskunft, inDto.AntragID);
                _log.Info("aggregationDao.GetVPDatenBySysAntrag Call duration : " + (TimeSpan)(DateTime.Now - startTime));

                if (inDto.DeltaVistaID > 0)
                {
                    startTime = DateTime.Now;
                    outDto.aggDVOutDto = aggregationDao.GetDVDatenBySysAntrag(sysAuskunft, inDto.AntragID);
                    _log.Info("aggregationDao.GetDVDatenBySysAntrag Call duration : " + (TimeSpan)(DateTime.Now - startTime));
                }
                
                
                startTime = DateTime.Now;
                outDto.aggZEKOutDto = aggregationDao.GetZEKDatenBySysAntrag(sysAuskunft, inDto.AntragID);
                _log.Info("aggregationDao.GetZEKDatenBySysAntrag Call duration : " + (TimeSpan)(DateTime.Now - startTime));

                outDto.ReturnCode = 0;
            }

            auskunftdto.AggregationOutDto = outDto;

            // Save AggregationOut
            aggregationDao.SaveAggregationOutDto(outDto, sysAuskunft);

            // Update Auskunft
            auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, outDto.ReturnCode);
            auskunftdto.requestXML = XMLSerializer.SerializeUTF8WithoutNamespace(inDto);
            auskunftdto.responseXML = XMLSerializer.SerializeUTF8WithoutNamespace(outDto);
            return auskunftdto;
        }

        #region Private methods
        #endregion    
	}
    
}
