using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EQUIFAX
{
    public class InterConnectInputDto
    {
        public InterConnectInputDto()
        {
        }
        public InterConnectInputDto(String idType, String idCode) { 
            applicants = new Applicant();
            applicants.primaryConsumer = new Consumer();
            applicants.primaryConsumer.personalInformation = new PersonalInformation();
             applicants.primaryConsumer.personalInformation.idType = idType;
            applicants.primaryConsumer.personalInformation.idCode = idCode;

        }

        public Applicant applicants { get; set; }

        #region Response-Data
        public String interactionId { get; set; }
        public String transactionState { get; set; }
        public String transactionId { get; set; }
        public String timestamp { get; set; }
        public List<EquifaxError> errors { get; set; }
        #endregion
    }

    public class EquifaxError
    {
        public String code { get; set; }
        public String message { get; set; }
    }
    public class Applicant
    {
        public Consumer primaryConsumer { get; set; }
    }

    public class Consumer
    {
        public PersonalInformation personalInformation { get; set; }
        #region Response-Data
        public DataSourceResponse dataSourceResponses { get; set; }
        #endregion
    }
    public class DataSourceResponse
    {
        public EIPG EIPG { get; set; }
        public List<EquifaxError> errors { get; set; }
    }
    public class EIPG
    {
        public RISK RISK { get; set; }
    }
    public class RISK
    {
        public String identifier { get; set; }
        public String idCode { get; set; }
        /// <summary>
        /// Service return code. It gets ‘000’ when Asnef Risk Attributes(A.R.A)  was properly calculated
        /// 000 Response correctly generated
        ///102 DNI not informed
        ///103 DNI Incorrect
        ///150 Request Type incorrect
        ///171 Address fields not informed
        ///172 Postal Code not informed
        ///173 Province Code not found
        ///174 Birth Date not informed or incorrect
        ///175 Younger
        ///176 Internal Error
        ///177 Service Error
        ///500 LOGON/LOGOFF already active
        ///501 Request executed without LOGON
        ///502 Entity Code Doesn’t exist
        ///503 User used in the query doesn’t exist
        ///504 Data not authorized for Entity 
        ///801 Service unavailable.Communication Problems
        ///802 Service unavailable. Application Problems
        ///803 Billing user doesn’t exist
        ///999 Security Error
        /// </summary>
        public String returnCode { get; set; }
        /// <summary>
        /// Found in Asnef File:
        ///00 –present in Asnef
        ///01 –not present in Asnef
        /// </summary>
        public String present { get; set; }
        /// <summary>
        /// Risk Score Individuals 
        /// output classification.
        /// Possible values: 
        /// (*):
        /// 1 (Very high)
        /// 2 (High)
        /// 3 (Medium high)
        /// 4 (Medium)
        /// 5 (Medium low)
        /// 6 (Low) (*)
        ///     These values could vary
        /// in case of customization
        /// </summary>
        public String rating { get; set; }
        public ARAAttribute araAttributes{ get; set; }
    }
    /// <summary>
    /// Used for saving all results into one table
    /// </summary>
    public class EQUIFAXRISKOUT : ARAAttribute
    {
        public String rating { get; set; }
        public String present { get; set; }
        public String returnCode { get; set; }
        public String identifier { get; set; }
        public String idCode { get; set; }
        public long sysauskunft { get; set; }
        public String transactionId { get; set; }
        public String transactionState { get; set; }
        public String timestamp { get; set; }
    }
   
    public class ARAAttribute
    {
        public int totalNumberOfOperations { get; set; }
        public int numberOfConsumerCreditOperations { get; set; }
        public int numberOfConsumerCreditOps { get { return numberOfConsumerCreditOperations; }  }
        public int numberOfMortgageOperations { get; set; }
        public int numberOfPersonalLoanOperations { get; set; }
        public int numberOfCreditCardOperations { get; set; }
        public int numberOfTelcoOperations { get; set; }
        public int totalNumberOfOtherUnpaid { get; set; }
        public double totalUnpaidBalance { get; set; }
        public double unpaidBalanceOwnEntity { get; set; }
        public double unpaidBalanceOfOtherEntities { get; set; }
        public double unpaidBalanceOfConsumerCredit { get; set; }
        public double unpaidBalanceOfMortgage { get; set; }
        public double unpaidBalanceOfPersonalLoan { get; set; }
        public double unpaidBalanceOfCreditCard { get; set; }
        public double unpaidBalanceOfTelco { get; set; }
        public double unpaidBalanceOfOtherProducts { get; set; }
        public double worstUnpaidBalance { get; set; }
        /// <summary>
        /// Worst situation
        /// Values:
        /// ‘01’: Amicable
        /// ‘02’: Damaging
        /// ‘03’: Judicial
        /// ‘04’: Default
        /// ‘05’: Receivership
        /// ‘06’: Bankruptcy
        /// ‘08’: Assets under 
        /// Regularised Receivership
        /// ‘09’: Voluntary contest
        /// ‘10’: Necessary contest
        /// ‘99’: Others
        /// </summary>
        public String worstSituationCode { get; set; }
        public int numberOfDaysOfWorstSituation { get; set; }
        public int numberOfCreditors { get; set; }
        public int delincuencyDays {get;set;}
}
    public class PersonalInformation
    {
        public String idType { get; set; }
        public String idCode { get; set; }
        public List<Address> addresses { get; set; }

        #region Response-Data
        public String idCountryCode { get; set; }
        public String postalCode { get; set; }
        /// <summary>
        /// Format YYYYMMDD
        /// </summary>
        public String dateOfBirth { get; set; }
        #endregion
    }
    public class Address
    {
        public String postalCode { get; set; }
        /// <summary>
        /// "Date of Birth sent in the Request Format YYYYMMDD
        /// </summary>
        public String dateOfBirth { get; set; }
    }
}
