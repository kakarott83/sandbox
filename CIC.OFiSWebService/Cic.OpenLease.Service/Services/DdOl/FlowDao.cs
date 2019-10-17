using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Data Access Object for BMWFlow
    /// </summary>
    [System.CLSCompliant(true)]
    public class FlowDao
    {
        #region Private variables
        private DdOlExtended _context;

        #endregion

        #region Constructors
        public FlowDao(DdOlExtended context)
        {
            _context = context;
        }
        #endregion


        /// <summary>
        /// delivers all entries for bmwflow
        /// </summary>
        /// <param name="type">Type (ANG,ANT,VTR,VT)</param>
        /// <param name="sysid">Primary key of type-entity</param>
        /// <returns></returns>
        public List<FlowDto> getMessages(string type, long sysid)
        {
            List<FlowDto> resultList = null;

            try
            {


                //Parameters for query
                System.Data.Common.DbParameter[] Parameters =
                        {

                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "p1", Value = sysid},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "p2", Value = 1}

                        };

                String DEFQUERY = @"select 0 DONE,0 SYSFLOW,KOMMENTAR,CATEGORY,USERID,BEFRISTETBIS,RANG from
                            (
                            SELECT 0 DONE,
                              0 SYSFLOW,
                              CASE
                                WHEN d.externcode = 'CW_0'
                                THEN
                                  (SELECT extractvalue(xmltype(CONTENT),'*/documentsAndCollaterals[collateralName = ''CW_0'']/collateralAdditionalText')
                                  FROM ddlkpspos
                                  WHERE sysid =
                                    (SELECT MAX(deoutexec.SYSDEOUTEXEC)
                                    FROM auskunft,
                                      deoutexec
                                    WHERE auskunft.sysauskunft = deoutexec.SYSAUSKUNFT
                                    AND auskunft.sysid         = antrag.sysid
                                    )
                                  AND ddlkpspos.AREA = 'DEOUTEXEC'
                                  )
                                ELSE
                                  CASE
                                    WHEN d.externcode = 'DOK_0'
                                    THEN
                                      (SELECT extractvalue(xmltype(CONTENT),'*/documentsAndCollaterals[collateralName = ''DOK_0'']/collateralAdditionalText')
                                      FROM ddlkpspos
                                      WHERE sysid =
                                        (SELECT MAX(deoutexec.SYSDEOUTEXEC)
                                        FROM auskunft,
                                          deoutexec
                                        WHERE auskunft.sysauskunft = deoutexec.SYSAUSKUNFT
                                        AND auskunft.sysid         = antrag.sysid
                                        )
                                      AND ddlkpspos.AREA = 'DEOUTEXEC'
                                      )
                                    ELSE
                                      (SELECT
                                        CASE
                                          WHEN (SELECT COUNT(1)
                                            FROM DEDEFCONTXT_V dv
                                            WHERE dv.sysdedefcon = d.sysdedefcon
                                            AND dv.SYSCTLANG     = :p2) = 1
                                          THEN
                                            (SELECT DEDEFCONTXT_V.TEXT
                                            FROM DEDEFCONTXT_V
                                            WHERE DEDEFCONTXT_V.SYSDEDEFCON = d.SYSDEDEFCON
                                            AND SYSCTLANG                   = :p2
                                            )
                                          ELSE d.DEFTEXTEXT
                                        END
                                      FROM dual
                                      )
                                  END
                              END                AS KOMMENTAR,
                              ratingauflage.wert AS CATEGORY,
                              CASE
                                WHEN ratingauflage.SYSPERSON = ANTRAG.SYSKD
                                THEN 1
                                ELSE 2
                              END AS USERID,
                              (select ende from antobsich where sysantrag = antrag.sysid and syssichtyp = d.displaytype) AS BEFRISTETBIS,
                              d.rank AS RANG
                            FROM antrag,
                              rating,
                              ratingauflage,
                              dedefcon d
                            WHERE rating.sysrating       = ratingauflage.sysrating
                            AND d.SYSDEDEFCON            = ratingauflage.sysdedefcon
                            AND rating.flag1             = 0
                            AND ratingauflage.FULLFILLED = 0
                            AND ratingauflage.ACTIVEFLAG = 1
                            AND rating.area              = 'ANTRAG'
                            AND rating.sysid             = antrag.sysid
                            AND antrag.sysangebot        = :p1
                            union all
                            select 0,0,'Info:','0',1,null,null from dual
                            union all
                            SELECT 0 AS DONE
                              ,0 AS SYSFLOW
                              ,to_char(notizmemo) AS KOMMENTAR
                              ,'0' AS CATEGORY
                              ,1 AS USERID
                              ,null AS BEFRISTETBIS
                              ,null as RANG
                            FROM wfmmemo,
                            angebot
                            WHERE angebot.sysantrag = wfmmemo.syslease
                            AND wfmmemo.STR01 = 'EXT'
                            AND wfmmemo.SYSWFMTABLE = 117
                            AND wfmmemo.SYSWFMMKAT = 22
                            AND angebot.sysid = :p1
                            )
                            ORDER BY RANG asc";

                String Query = _context.ExecuteStoreQuery<String>("select paramfile from eaipar where code='GETFOAUFLAGEN'", null).FirstOrDefault();
                if (Query == null) Query = DEFQUERY;

                resultList = _context.ExecuteStoreQuery<FlowDto>(Query, Parameters).ToList();
                foreach (FlowDto fd in resultList)
                {
                    if (fd.CATEGORY != null && fd.CATEGORY.Length > 0)
                    {
                        try
                        {
                            fd.CATEGORY = String.Format(new System.Globalization.CultureInfo("de-DE"), "{0:N}", decimal.Parse(fd.CATEGORY, System.Globalization.CultureInfo.InvariantCulture));
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

            }
            catch
            {
                throw;
            }


            return resultList;
        }




    }


}