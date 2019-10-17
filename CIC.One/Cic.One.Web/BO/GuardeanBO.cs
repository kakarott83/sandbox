using Cic.One.DTO;
using Cic.One.Web.GateGuardean;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Cic.One.Web.BO
{
    public class GuardeanBo : IGuardeanBo
    {
        private String proxyuser = "guardeanClient/guardean";
        private String proxypwd = "guardean";
        private WorkflowEnginePortTypeClient we2;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public GuardeanBo()
        {

        }
        public ogetGuardeanDetailDto getAuskunft(EntityContainer entities)
        {
            ogetGuardeanDetailDto rval = new ogetGuardeanDetailDto();
            rval.gview = new GviewDto();
            rval.gview.fields = new List<Viewfield>();


            executeWorkflowRequestType wr = new executeWorkflowRequestType();
            wr.data = new workflowExecutionDataType();
            wr.data.workflowName = "Wkf_main";
            wr.data.detailLevel = new detailLevelType();
            wr.data.detailLevel.returnRequestStruct = false;
            wr.data.detailLevel.returnResponseStruct = true;
            wr.data.requestStruct = new structType();

            structType contractDetails = addStructToStruct(wr.data.requestStruct, "contractDetails");
            addLongToStruct(contractDetails, "applicationNumber", 0);
            addLongToStruct(contractDetails, "contractDuration", entities.angkalk.lz.HasValue?(long)entities.angkalk.lz.Value:0);
            addLongToStruct(contractDetails, "mileagePerYear", 0);
            addLongToStruct(contractDetails, "vehicleState", 1);
            addStringToStruct(contractDetails, "contractType", "V");
            addStringToStruct(contractDetails, "IBAN",entities.it.iban!=null?entities.it.iban:"");// "000000000000110");
            addStringToStruct(contractDetails, "BIC", entities.it.bic != null ? entities.it.bic : "");// "GED01");
            addStringToStruct(contractDetails, "accountOwner", "Test User");

            addDateTimeToStruct(contractDetails, "dateFirstOffer",DateTime.Now);
            addDateTimeToStruct(contractDetails, "contractStartDate", DateTime.Now);


            arrayType customer = addArrayToStruct(wr.data.requestStruct, "customer");
            structType customer1 = addStructToArray(customer, null);
            structType address1 = addStructToStruct(customer1, "address");
            addStringToStruct(address1, "countryCode", "DE");

            structType budget = addStructToStruct(customer1, "budgetCalculation");
            addDoubleToStruct(budget, "additionalGrossIncome", entities.it.nebeneinkNetto>0? entities.it.nebeneinkNetto:1);
            addDoubleToStruct(budget, "aliMony", entities.it.unterhalt > 0 ? entities.it.unterhalt : 1);
            addDoubleToStruct(budget, "amountExistingCredit", entities.it.auslagen > 0 ? entities.it.auslagen : 1);
            addDoubleToStruct(budget, "grossIncome", entities.it.einkNetto > 0 ? entities.it.einkNetto : 1);
            addLongToStruct(budget, "numberPersonDomesticHome", entities.it.kinderimhaus);

            addStringToStruct(customer1, "lastName", entities.it.name);
            long bpid = 0;
            try {
                bpid = long.Parse(entities.it.matchcode);
            }catch(Exception ){}
            addLongToStruct(customer1, "businessPartnerId", bpid);
            addLongToStruct(customer1, "businessPartnerPersonType", 0);
            addLongToStruct(customer1, "customerContractUsageType", 1);
            if("99".Equals(entities.it.rechtsformCode))
                addLongToStruct(customer1, "customerType", 1);
            else 
                addLongToStruct(customer1, "customerType", 2);
            addLongToStruct(customer1, "gender", 1);
            addLongToStruct(customer1, "housingConditionType", 2);

            addDoubleToStruct(customer1, "turnOver", entities.it.jahresumsatz > 0 ? entities.it.jahresumsatz : 0);

            addDoubleToStruct(customer1, "creditLimit", entities.angebot.bruttokredit>0?entities.angebot.bruttokredit:1);//TODO?
            addStringToStruct(customer1, "role", "");
            addDateTimeToStruct(customer1, "dateOfBirth", entities.it.gebdatum.HasValue?entities.it.gebdatum.Value:DateTime.Now);
            addDateTimeToStruct(customer1, "foundingDate", entities.it.gruendung.HasValue ? entities.it.gruendung.Value : DateTime.Now);


            structType dealer = addStructToStruct(wr.data.requestStruct, "dealer");
            structType financialData = addStructToStruct(wr.data.requestStruct, "financialData");
            addDoubleToStruct(financialData, "basePrice", entities.angkalk.ahk > 0 ? entities.angkalk.ahk : 0);
            addDoubleToStruct(financialData, "finalPayment", 0);
            addDoubleToStruct(financialData, "residualValue", entities.angkalk.rw > 0 ? entities.angkalk.rw : 0);
            addDoubleToStruct(financialData, "interestRate", entities.angkalk.zins);
            addLongToStruct(financialData, "interestRateType", 2);

            addDoubleToStruct(financialData, "downpayment", entities.angkalk.sz);
            addDoubleToStruct(financialData, "financedAmount", entities.angkalk.bgintern>0?entities.angkalk.bgintern:0);
            addDoubleToStruct(financialData, "installment", entities.angkalk.rate > 0 ? entities.angkalk.rate : 0);
            structType vehicle = addStructToStruct(wr.data.requestStruct, "vehicle");

            DateTime start = DateTime.Now;
            workflowExecutionType resp = executeWorkflow(wr);
            long duration = (long)(DateTime.Now - start).TotalMilliseconds;
            _log.Debug("Guardean call time: " + duration);

            structType rv = resp.rootPath.responseStruct;
            if(rv.attribute==null || rv.attribute.Length==0)
            {
                return rval;
            }
            structType decision = getStruct(rv, "decision");
            //structType cra = getStruct(decision, "cra");
            structType scoring = getStruct(decision, "scoring");
            structType policyRules = getStruct(decision, "policyRules");

            stringType decisionResult = getString(decision, "decision");
            stringType appScore = getString(scoring, "applicationScore");
            stringType rating = getString(scoring, "rating");

            addViewfieldHeadline(rval.gview, "Ergebnis");
            
            addViewfield(rval.gview, "automatische Entscheidung", decisionResult.value);
            addViewfield(rval.gview, "Score", appScore.value);
            addViewfield(rval.gview, "Rating", rating.value);

            
            
            if(policyRules!=null)
            {
                Dictionary<String, List<Viewfield>> group = new Dictionary<string, List<Viewfield>>();

                arrayType policies = getArray(policyRules, "policyRule");
                foreach(abstractValueType pol in policies.element)
                {
                    structType s = (structType)pol;
                    stringType ps = getString(s, "policyRuleSegment");
                    stringType pd = getString(s, "policyRuleDescription");
                    stringType pc = getString(s, "policyRuleCode");
                    if (ps == null || pd == null || pc == null) continue;

                    /*"0 = information
                    1 = muw
                    2 = rejection"	Regeltyp
                    */
                    longType ampel = getLong(s, "policyRuleType");
                    long rtype = ampel.value.Value;

                    /*0 = ok
                    1 = nok
                    2 = not evaluated"	Regelergebnis*/
                    stringType ergebnis = getString(s, "policyRuleResult");
                    int rres = 0;
                    if (ergebnis != null)
                    {
                        try
                        {
                            rres = int.Parse(ergebnis.value);
                        }
                        catch (Exception ) { }

                    }
                    long result = 0;
                    if (rres == 0)//OK GRÜN
                        result = 0;
                    else if (rres == 2)//NOT EVALUATED GRAU FRAGEZEICHEN
                        result = 3;
                    else if(rres==1)
                    {
                        if (rtype == 0)
                            result = 4;//Grau I
                        else if (rtype == 1)
                            result = 1;//Orange
                        else if (rtype == 2)
                            result = 2;//ROT
                    }

                    Viewfield vf = createViewfield(rval.gview, pd.value, pc.value,result, 0);
                    if(!group.ContainsKey(ps.value))
                    {
                        group[ps.value] = new List<Viewfield>();
                    }
                    group[ps.value].Add(vf);
                }
                addViewfieldHeadline(rval.gview, "Automatische Entscheidung");
                //add all policy Rules
                foreach(String segment in group.Keys)
                {
                    addViewfieldSeparator(rval.gview, segment);
                    foreach(Viewfield vf in group[segment])
                    {
                        rval.gview.fields.Add(vf);
                    }
                }
            }
            arrayType cras = getArray(decision, "cra");
            if(cras!=null)
            { 
            foreach (abstractValueType cri in cras.element)
            {
                structType cra = (structType)cri;
                if (cra != null)
                {
                    stringType cradecision = getString(cra, "decision");
                    stringType craProvider = getString(cra, "craProvider");
                    String provider = "";
                    if (craProvider != null && craProvider.value != null)
                        provider = craProvider.value;
                    binaryType bin = getBinary(cra, "orginalCRAReport");

                    addViewfieldHeadline(rval.gview, "Auskunft " + provider);
                    addViewfieldSeparator(rval.gview, "Ergebnis " + provider);
                    addViewfield(rval.gview, "Decision", cradecision.value);
                    addViewfieldSeparator(rval.gview, "Anhänge " + provider);
                    addViewfieldData(rval.gview, "PDF", getBinaryData(bin));
                }
            }
        }
            return rval;
        }
        
        private void addViewfieldSeparator(GviewDto g, String name)
        {
            
            Viewfield vf = new Viewfield();
            vf.id = "" + g.fields.Count;
            vf.attr = new ViewFieldAttributes();
            vf.attr.field = name;
            vf.attr.label = name;
            vf.attr.type = "String";
            vf.attr.ro = 1;
            vf.attr.viewtype = "separator";
            g.fields.Add(vf);
        }
        private void addViewfieldHeadline(GviewDto g, String name)
        {

            Viewfield vf = new Viewfield();
            vf.id = "" + g.fields.Count;
            vf.attr = new ViewFieldAttributes();
            vf.attr.field = name;
            vf.attr.label = name;
            vf.attr.type = "String";
            vf.attr.ro = 1;
            vf.attr.viewtype = "headline";
            g.fields.Add(vf);
        }
        private void addViewfield(GviewDto g, String name, String value)
        {
            
            Viewfield vf = new Viewfield();
            vf.id = "" + g.fields.Count;
            vf.attr = new ViewFieldAttributes();
            vf.attr.field = name;
            vf.attr.label = name;
            vf.attr.type = "String";
           
            vf.attr.ro = 1;
            vf.attr.viewtype = "text";
            vf.value = new ViewValue();
            vf.value.s = value;
            g.fields.Add(vf);
        }
        private void addViewfieldData(GviewDto g, String name, byte[] value)
        {

            Viewfield vf = new Viewfield();
            vf.id = "" + g.fields.Count;
            vf.attr = new ViewFieldAttributes();
            vf.attr.field = name;
            vf.attr.label = name;
            vf.attr.type = "Byte";

            vf.attr.ro = 1;
            vf.attr.viewtype = "pdf";
            vf.value = new ViewValue();
            vf.value.data = value;
            g.fields.Add(vf);
        }
        private Viewfield createViewfield(GviewDto g,String name, String code, long l, int chk)
        {
            
            Viewfield vf = new Viewfield();
            vf.id = "" + g.fields.Count;
            vf.attr = new ViewFieldAttributes();
            vf.attr.field = name;
            vf.attr.label = name;
            vf.attr.type = "Long";
            vf.attr.code = code;
            vf.attr.ro = 1;
            vf.attr.viewtype = "text";
            vf.value = new ViewValue();
            vf.value.s = ""+l;
            vf.value.l = l;
            vf.value.i = chk;
            return vf;
        }


        private arrayType getArray(structType st, String name)
        {
            return (arrayType)(from t in st.attribute
                                where t.name.Equals(name)
                                select t).FirstOrDefault();
        }
        private structType getStruct(structType st,String name)
        {
            return (structType)(from t in st.attribute
                    where t.name.Equals(name)
                    select t).FirstOrDefault();
        }
        private stringType getString(structType st, String name)
        {
            return (stringType)(from t in st.attribute
                    where t.name.Equals(name)
                    select t).FirstOrDefault();
        }
        private longType getLong(structType st, String name)
        {
            return (longType)(from t in st.attribute
                                where t.name.Equals(name)
                                select t).FirstOrDefault();
        }
        private binaryType getBinary(structType st, String name)
        {
            return (binaryType)(from t in st.attribute
                                where t.name.Equals(name)
                                select t).FirstOrDefault();
        }
        public void test()
        {
            executeWorkflowRequestType wr = new executeWorkflowRequestType();
            wr.data = new workflowExecutionDataType();
            wr.data.workflowName = "ShowCaseWorkflow";
            wr.data.externalIdentifier = "SOAP-UI_Test_wehrenbj";
            wr.data.detailLevel = new detailLevelType();
            wr.data.detailLevel.returnRequestStruct = false;
            wr.data.detailLevel.returnResponseStruct = true;
            wr.data.requestStruct = new structType();
            arrayType at = new arrayType();
            structType cust1 = addStructToArray(at, null);

            structType bc = addStructToStruct(wr.data.requestStruct, "BusinessCustomer");
            bc = addStructToStruct(bc, "BusinessReport");
            structType cont = addStructToStruct(bc, "contractInformation");
            
            addStringToStruct(bc, "identificationnumber", "03453452013836");
            addStringToStruct(bc, "legitimateinterest", "Kreditentscheidung");

            addDoubleToStruct(cont, "carValue", 75000);
            addLongToStruct(cont, "contractDuration", 48);
            addDoubleToStruct(cont, "monthlyExpenses", 40000);
            addDoubleToStruct(cont, "monthlyTurnover", 500000);
            executeWorkflow(wr);
        }

        private byte[] getBinaryData(binaryType b)
        {
            if (b == null || b.value == null) return null;

            String base64Encoded = System.Text.ASCIIEncoding.ASCII.GetString(b.value);

            try
            {
                byte[] data = System.Convert.FromBase64String(base64Encoded);
                return data;
            }catch(Exception )
            {
                return b.value;
            }
            //base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            //FileUtils.saveFile("C:\\temp\\kremo.pdf", data);
           
        }

        /// <summary>
        /// Executes the Workflow on Guardean
        /// </summary>
        /// <param name="wr"></param>
        /// <returns></returns>
        private workflowExecutionType executeWorkflow(executeWorkflowRequestType wr)
        {
            if(we2==null)
            { 
                we2 = new WorkflowEnginePortTypeClient();
           
                we2.ClientCredentials.UserName.UserName = proxyuser;
                we2.ClientCredentials.UserName.Password = proxypwd;
                we2.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SOAPLoggingBehaviour());
            }
            using (OperationContextScope scope = new OperationContextScope(we2.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                  "Basic " +
                  Convert.ToBase64String(Encoding.ASCII.GetBytes(
                         we2.ClientCredentials.UserName.UserName + ":" +
                         we2.ClientCredentials.UserName.Password));

                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                            httpRequestProperty;

                executeWorkflowResponseType re = we2.executeWorkflow(wr);
                return re.workflowExecution;
            }

        }

        /// <summary>
        /// Extends the arrayType with one Element
        /// </summary>
        /// <param name="strct"></param>
        /// <returns></returns>
        private arrayType extendArray(arrayType arr)
        {
            
            if (arr.element == null)
                arr.element = new abstractValueType[1];
            else
            {
                abstractValueType[] atts = arr.element;
                Array.Resize(ref atts, atts.Length + 1);
                arr.element = atts;
            }
            return arr;
        }

        private doubleType addDoubleToStruct(abstractValueType strct, String name, double value)
        {
            structType st = extendStructArray(strct);
            doubleType att = new doubleType();
            att.name = name;
            att.value = value;
            st.attribute[st.attribute.Length - 1] = att;
            return att;
        }
        /// <summary>
        /// Extends the attribute-Array of the given structType
        /// </summary>
        /// <param name="strct"></param>
        /// <returns></returns>
        private structType extendStructArray(abstractValueType strct)
        {
            structType st = (structType)strct;
            if (st.attribute == null)
                st.attribute = new abstractValueType[1];
            else
            {
                abstractValueType[] atts = st.attribute;
                Array.Resize(ref atts, atts.Length + 1);
                st.attribute = atts;
            }
            return st;
        }
        private arrayType addArrayToStruct(abstractValueType strct, String name)
        {
            structType st = extendStructArray(strct);
            arrayType att = new arrayType();
            att.name = name;
           
            st.attribute[st.attribute.Length - 1] = att;
            return att;
        }
        private dateTimeType addDateTimeToStruct(abstractValueType strct, String name, DateTime? value)
        {
            structType st = extendStructArray(strct);
            dateTimeType att = new dateTimeType();
            att.name = name;
            att.value = value;
            st.attribute[st.attribute.Length - 1] = att;
            return att;
        }
        private longType addLongToStruct(abstractValueType strct, String name, long value)
        {
            structType st = extendStructArray(strct);
            longType att = new longType();
            att.name = name;
            att.value = value;
            st.attribute[st.attribute.Length - 1] = att;
            return att;
        }
        private stringType addStringToStruct(abstractValueType strct, String name, String value)
        {
            structType st = extendStructArray(strct);
            stringType att = new stringType();
            att.name = name;
            att.value = value;
            st.attribute[st.attribute.Length - 1] = att;

            return att;
        }
        private structType addStructToStruct(abstractValueType strct, String name)
        {
            structType st = extendStructArray(strct);
            structType att = new structType();
            att.name = name;
            st.attribute[st.attribute.Length - 1] = att;
            return att;
        }
        /// <summary>
        /// adds a named struct to the given arrayType
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private structType addStructToArray(arrayType arr, String name)
        {
            arr = extendArray(arr);
            structType att = new structType();
            att.name = name;
            arr.element[arr.element.Length - 1] = att;
            return att;
        }
    }
}
