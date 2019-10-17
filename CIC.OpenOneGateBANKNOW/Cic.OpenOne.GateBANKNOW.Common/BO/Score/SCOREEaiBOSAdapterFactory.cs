using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Behaviour;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.ScoreCaseMgmt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Score
{
    /// <summary>
    /// Adapts Score-Service to be called from EAI
    /// </summary>
    public class SCOREEaiBOSAdapterFactory : IEaiBOSAdapterFactory
	{
		private static readonly ILog _log = Log.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);

		public IEaiBOSAdapter getEaiBOSAdapter (String method)
		{
			// make it case-insensitive
			String imethod = method.ToUpper ();
			switch (imethod)
			{
				case ("SCORE_CREATE_COL_CASE"):
				return new CreateColCaseAdapter ();
				case ("SCORE_UPDATE_COL_CASE"):
				return new UpdateColCaseAdapter ();
			}
			return null;
		}

		/// <summary>
		/// splitt into data rows "SEGMENTING"
		/// </summary>
		/// <param name="queue"></param>
		/// <returns></returns>
		internal static List<CreateColCaseInDto> getColCaseInDtos (List<EaiqinDto> queue)
		{
			

			List<CreateColCaseInDto> rval = new List<CreateColCaseInDto> ();


			var props = typeof (CreateColCaseInDto).GetProperties ();

			for (int idx = 0; idx < queue.Count; idx++)
			{
				EaiqinDto start = queue[idx];
				if (start.F01.Equals ("DATENSATZ"))
				{
					int eidx = queue.Count - 1;
					int idx2 = 0;
					for (idx2 = idx + 1; idx2 < queue.Count; idx2++)
					{
						EaiqinDto end = queue[idx2];
						if (end.F01.Equals ("DATENSATZ"))
						{
							eidx = idx2 - 1;
							break;
						}
					}
					CreateColCaseInDto data = new CreateColCaseInDto ();
					List<EaiqinDto> dataqueue = queue.GetRange (idx + 1, eidx - idx);

					foreach (var prop in props)
					{
						// Trim column F01 (var names) to eliminate leading and trailing spaces in EaiQIn (mb/rh 20180123)
						String value = (from t in dataqueue
										where t.F01.Trim().Equals (prop.Name)
										select t.F02).FirstOrDefault ();

						prop.SetValue (data, trimString (value));
					}
					data.DataType = start.F02;
					data.DataId = start.F03;
					data.DataAlias = start.F04;
					rval.Add (data);

				}
			}



			return rval;
		}


		/// <summary>
		/// 
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private static String stripString (String s)
		{
			if (s == null)
			{
				return s;
			}
			if (s.Length > 255)
			{
				return s.Substring (0, 255);
			}
			return s;
		}

		private static String trimString (String s)
		{
			if (s == null)
			{
				return null;
			}
			return s.Trim ();
		}

		private static decimal getDecimal (String data)
		{
			if (data == null)
				return 0;
			return Decimal.Parse (data, System.Globalization.CultureInfo.InvariantCulture);
		}

		private static DateTime? getDate (String date, String format, IFormatProvider provider)
		{
			if (date == null)
				throw new Exception ("Date empty");
			date = date.Trim ();
			if ("0000-00-00".Equals (date.Substring (0, 10)) || "0111-11-11".Equals (date.Substring (0, 10)))
			{
				throw new Exception ("Date empty");
			}
			try
			{
				return DateTime.ParseExact (date, format, provider);
			}
			catch (Exception)
			{
				throw new Exception ("Date invalid: " + date);
			}
		}

		/// <summary>
		/// Adapter for calling CREATECOLCASE from eaihot
		/// </summary>
		private class CreateColCaseAdapter : IEaiBOSAdapter
		{
			public void processEaiHot (IEaihotDao dao, EaihotDto eaihot)
			{
				processEaiHot (dao, eaihot, false);
			}
			public void processEaiHot (IEaihotDao dao, EaihotDto eaihot, bool update)
			{
				try
				{
					RetailContractType retailContract = new RetailContractType ();
					List<EaiqinDto> queue = dao.listEaiqinForEaihot (eaihot.SYSEAIHOT);

					// HINT: ATTENTION!: MULTI Invoices possible!
					//// WITH counter 
					List<Cic.OpenOne.GateBANKNOW.Common.DTO.CreateColCaseInDto> inputdataList = SCOREEaiBOSAdapterFactory.getColCaseInDtos (queue);

					using (PrismaExtended context = new PrismaExtended ())
					{
						//Format of date in db-string
						// String dformat = "yyyy-MM-dd\\THH:mm:ss";		// ALT / DEFEKT   (rh 20171115)
						// MULTI format would be possible too ON MIXED Date AND DateTime-fields (rh 20171115)
						// String[] dformatList = { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss" };	 
						// DateTime.ParseExact (dateDummy.Trim (), dformatList, CultureInfo.InvariantCulture, DateTimeStyles.None);
						String dformat = "yyyy-MM-dd";                      // NEW: Date ONLY (rh 20171115) 

						//————————————————————————————————————————————————————
						// CONTRACT
						//————————————————————————————————————————————————————
						CreateColCaseInDto contractData = (from t in inputdataList
														   where t.DataType.Equals ("VT")
														   select t).FirstOrDefault ();
						if (contractData == null)
							throw new Exception ("EMPTY contract Data");	// EXIT on null data


						ColCaseVTDto vtdata = context.ExecuteStoreQuery<ColCaseVTDto> ("select depot deposit,vertrag ContractReference,vt.syskd,vtoption.flag03 isSpecialCase from vt,kalk,ob,vtoption where vtoption.SYSID=vt.sysid and kalk.sysob=ob.sysob and ob.sysvt=vt.sysid and vt.sysid=" + contractData.DataId, null).FirstOrDefault ();
						retailContract.number = vtdata.ContractReference;

						retailContract.collateral = new CollateralType[1];
						retailContract.collateral[0] = new DepositType ();
						retailContract.collateral[0].amount = new MonetaryValueType ();
						retailContract.collateral[0].amount.value = vtdata.Deposit;
						retailContract.collateral[0].amount.valueSpecified = true;

						retailContract.collectionCase = new CollectionCaseType[1];
						retailContract.collectionCase[0] = new CollectionCaseType ();
						retailContract.collectionCase[0].collectionData = new CollectionDataType[1];
						retailContract.collectionCase[0].collectionData[0] = new CollectionDataType ();
						retailContract.collectionCase[0].collectionData[0].arrearsInterestAmount = new MonetaryValueType ();
						retailContract.collectionCase[0].collectionData[0].arrearsInterestAmount.value = getDecimal (contractData.Interest);
						retailContract.collectionCase[0].collectionData[0].arrearsInterestAmount.valueSpecified = true;
						retailContract.collectionCase[0].collectionData[0].arrears = new MonetaryValueType ();
						retailContract.collectionCase[0].collectionData[0].arrears.value = getDecimal (contractData.ArrearsBalance);
						retailContract.collectionCase[0].collectionData[0].arrears.valueSpecified = true;

						int notecount = 0;

						if ("1".Equals (contractData.FuturePtPFlag))
							notecount++;
						if ("1".Equals (contractData.FuturePayArrFlag))
							notecount++;

						retailContract.collectionCase[0].status = contractData.FuturePayArrFlag;

						if ("1".Equals (contractData.FuturePayArrFlag))
						{
							retailContract.note = new NoteType[notecount];
							retailContract.note[0] = new NoteType ();
							retailContract.note[0].note = contractData.DetailFuturePayArr;

							retailContract.collectionCase[0].gracePeriod = new AbsoluteTimePeriodType ();

							if (contractData.FuturePayArrStartDate != null)     // new 20180111
							{
								try
								{
									//	retailContract.collectionCase[0].gracePeriod.startDate = wert8values.FuturePayArrStartDate.Value;
									retailContract.collectionCase[0].gracePeriod.startDate = getDate (contractData.FuturePayArrStartDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
									retailContract.collectionCase[0].gracePeriod.startDateSpecified = true;
								}
								catch (Exception) { _log.Warn ("Date for FuturePayArrStartDate not valid or empty"); }
							}

							if (contractData.FuturePayArrEndDate != null)		// new 20180111
							{
								try
								{
									// retailContract.collectionCase[0].gracePeriod.endDate = wert8values.FuturePayArrEndDate.Value;
									retailContract.collectionCase[0].gracePeriod.endDate = getDate (contractData.FuturePayArrEndDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
									retailContract.collectionCase[0].gracePeriod.endDateSpecified = true;
								}
								catch (Exception) { _log.Warn ("Date for FuturePayArrEndDate not valid or empty"); }
							}
						}

						// if (wert9values!=null && "1".Equals(wert9values.FuturePtPFlag))
						if ("1".Equals (contractData.FuturePtPFlag))
						{
							if (retailContract.note == null)
								retailContract.note = new NoteType[1];

							retailContract.note[notecount - 1] = new NoteType ();
							retailContract.note[notecount - 1].note = contractData.DetailFuturePtP;

							retailContract.collectionCase[0].paymentBond = new PaymentBondType ();
							retailContract.collectionCase[0].paymentBond.typeOfBond = "FuturePtP";
							retailContract.collectionCase[0].paymentBond.validityPeriod = new AbsoluteTimePeriodType ();
							if (contractData.FuturePtPStartDate != null)     // new 20180111
							{
								try
								{
									// retailContract.collectionCase[0].paymentBond.validityPeriod.startDate = wert9values.FuturePtPStartDate.Value;
									retailContract.collectionCase[0].paymentBond.validityPeriod.startDate = getDate (contractData.FuturePtPStartDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
									retailContract.collectionCase[0].paymentBond.validityPeriod.startDateSpecified = true;
								}
								catch (Exception) { _log.Warn ("Date for FuturePtPStartDate not valid or empty"); }
							}
							if (contractData.FuturePtPEndDate != null)      // new 20180111
							{
								try
								{
									// retailContract.collectionCase[0].paymentBond.validityPeriod.endDate = wert9values.FuturePtPEndDate.Value;
									retailContract.collectionCase[0].paymentBond.validityPeriod.endDate = getDate (contractData.FuturePtPEndDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
									retailContract.collectionCase[0].paymentBond.validityPeriod.endDateSpecified = true;
								}
								catch (Exception) { _log.Warn ("Date for FuturePtPEndDate not valid or empty"); }
							}
						}

						retailContract.collectionCase[0].dunningActivities = new DunningActivitiesType[1];
						retailContract.collectionCase[0].dunningActivities[0] = new DunningActivitiesType ();
						if (contractData.DunningActivityDate != null)
						{
							try
							{
								retailContract.collectionCase[0].dunningActivities[0].lastAction = getDate (contractData.DunningActivityDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
								retailContract.collectionCase[0].dunningActivities[0].lastActionSpecified = true;
							}
							catch (Exception) { _log.Warn ("Date for lastAction not valid or empty"); }
						}

						retailContract.collectionCase[0].retailRecovery = new RetailRecoveryType ();
						retailContract.collectionCase[0].retailRecovery.estimatedRecoveryValue = new MonetaryValueType ();
						retailContract.collectionCase[0].retailRecovery.estimatedRecoveryValue.value = getDecimal (contractData.EstimatedSalesPriceVehicle);
						retailContract.collectionCase[0].retailRecovery.estimatedRecoveryValue.valueSpecified = true;

						retailContract.writeOff = new WriteOffCaseType ();
						if (contractData.WriteOffDate != null)
						{
							try
							{
								retailContract.writeOff.writeOffDate = getDate (contractData.WriteOffDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
								retailContract.writeOff.writeOffDateSpecified = true;
							}
							catch (Exception) { _log.Warn ("Date for writeOffDate not valid or empty"); }
						}
						retailContract.writeOff.totalSum = new MonetaryValueType ();
						retailContract.writeOff.totalSum.value = getDecimal (contractData.WriteOffAmount);
						retailContract.writeOff.totalSum.valueSpecified = true;
						if (contractData.WriteOffFollowupDate != null)
						{
							try
							{
								retailContract.writeOff.writeOffFollowUp = getDate (contractData.WriteOffFollowupDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
								retailContract.writeOff.writeOffFollowUpSpecified = true;
							}
							catch (Exception) { _log.Warn ("Date for writeOffFollowUp not valid or empty"); }
						}

						retailContract.collectionCase[0].lawyerHandling = new LawyerHandoverType ();
						bool solicitorFlag = (contractData.SolicitorFlag != null && (contractData.SolicitorFlag.Equals ("1") || contractData.SolicitorFlag.Equals ("true"))) ? true : false;
						retailContract.collectionCase[0].lawyerHandling.caseAtLawyer = solicitorFlag;
						retailContract.collectionCase[0].lawyerHandling.caseAtLawyerSpecified = true;
						if (contractData.SolicitorHandoverDate != null)
						{
							try
							{
								retailContract.collectionCase[0].lawyerHandling.handoverDate = getDate (contractData.SolicitorHandoverDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
								retailContract.collectionCase[0].lawyerHandling.handoverDateSpecified = true;
							}
							catch (Exception) { _log.Warn ("Date for lawyerHandling.handoverDate not valid or empty"); }
						}

						if (vtdata.isSpecialCase > 0)
						{
							retailContract.collectionCase[0].isSpecialCase = true;
							retailContract.collectionCase[0].isSpecialCaseSpecified = true;
						}
						if (contractData.SolicitorFollowupDate != null)
						{
							try
							{
								retailContract.collectionCase[0].lawyerHandling.followUpDate = getDate (contractData.SolicitorFollowupDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
								retailContract.collectionCase[0].lawyerHandling.followUpDateSpecified = true;
							}
							catch (Exception) { _log.Warn ("Date for lawyerHandling.followUpDate not valid or empty"); }
						}

						List<CreateColCaseInDto> hdData = (from t in inputdataList
														   where t.DataType.Equals ("PERSON") && t.DataAlias.Equals ("HD")
														   select t).ToList ();
						List<CreateColCaseInDto> mhData = (from t in inputdataList
														   where t.DataType.Equals ("PERSON") && t.DataAlias.Equals ("MH")
														   select t).ToList ();
						int crcount = 1;
						if (hdData != null && hdData.Count > 0)
							crcount++;
						if (mhData != null && mhData.Count > 0)
							crcount++;

						// CustomerReference
						retailContract.partnerRoleRelationship = new BIPartnerRoleRelationshipType[crcount];
						retailContract.partnerRoleRelationship[0] = new BIPartnerRoleRelationshipType ();
						retailContract.partnerRoleRelationship[0].involvedPartnerRole = new CustomerType ();// PartnerRoleType();
						retailContract.partnerRoleRelationship[0].involvedPartnerRole.partner = new PartnerType ();
						retailContract.partnerRoleRelationship[0].involvedPartnerRole.partner.number = "" + vtdata.syskd;
						// CustomerRelationshipType
						retailContract.partnerRoleRelationship[0].type = "Customer";

						int mhidx = 1;
						if (hdData != null && hdData.Count > 0)
						{
							retailContract.partnerRoleRelationship[mhidx] = new BIPartnerRoleRelationshipType ();
							retailContract.partnerRoleRelationship[mhidx].involvedPartnerRole = new CustomerType ();
							retailContract.partnerRoleRelationship[mhidx].involvedPartnerRole.partner = new PartnerType ();
							retailContract.partnerRoleRelationship[mhidx].involvedPartnerRole.partner.number = hdData[0].DataId;
							retailContract.partnerRoleRelationship[mhidx].type = "Dealer";
							mhidx++;
						}
						if (mhData != null && mhData.Count > 0)
						{

							retailContract.partnerRoleRelationship[mhidx] = new BIPartnerRoleRelationshipType ();
							retailContract.partnerRoleRelationship[mhidx].involvedPartnerRole = new CustomerType ();
							retailContract.partnerRoleRelationship[mhidx].involvedPartnerRole.partner = new PartnerType ();
							retailContract.partnerRoleRelationship[mhidx].involvedPartnerRole.partner.number = mhData[0].DataId;
							retailContract.partnerRoleRelationship[mhidx].type = "Co-Debtor";
							mhidx++;
						}

						//————————————————————————————————————————————————————
						// INVOICE(S) ACHTUNG!: könnten MEHRERE Invoices SEIN!
						//————————————————————————————————————————————————————
						List<CreateColCaseInDto> rnData = (from t in inputdataList
														   where t.DataType.Equals ("RN")
														   select t).ToList ();

						bool orderFlag = (contractData.RepossessionOrderCMS != null && (contractData.RepossessionOrderCMS.Equals ("1") || contractData.RepossessionOrderCMS.Equals ("true"))) ? true : false;

						bool thirdPartyFlag = (contractData.PurchaseFlagCustomer != null && (contractData.PurchaseFlagCustomer.Equals ("1") || contractData.PurchaseFlagCustomer.Equals ("true"))) ? true : false;

						bool repossessionFlag = false;
						try
						{
							if (getDate (contractData.VehicleRepossessed, dformat, CultureInfo.InvariantCulture).HasValue)
								repossessionFlag = true;//date valid, so set the flag
						}
						catch (Exception)
						{
							//date empty or invalid
						}

						int offset = 0;
						int invCount = rnData.Count;
						if (orderFlag || repossessionFlag || thirdPartyFlag)
						{
							invCount++;
							offset++;
						}
						retailContract.retailInvoice = new RetailInvoiceType[invCount];

						if (orderFlag || repossessionFlag || thirdPartyFlag)    // Settlement-data, only when solicitorFlag
						{
							//Settlement data
							SettlementInvoiceType sit = new SettlementInvoiceType ();
							retailContract.retailInvoice[0] = sit;
							SettlementType st = new SettlementType ();
							sit.settlement = st;

							//——————————————————————————————————————————————————————————————————————————————
							// rh 20171115: 
							//——————————————————————————————————————————————————————————————————————————————
							//	InvoiceNumber is undetectable here as we do NOT have an invoice (rn)  
							//	as we are related to the contract itself here
							//——————————————————————————————————————————————————————————————————————————————
							//////// SET InvoiceID and InvoiceNumber (new rh 20171115)
							//////// InvoiceID
							//////sit.document = new DocumentType[1];
							//////sit.document[0] = new DocumentType ();
							//////sit.document[0].documentIdentifier = ccdata.InvoiceID;
							//////// InvoiceNumber
							//////sit.number = ccdata.InvoiceNumber;
							//——————————————————————————————————————————————————————————————————————————————

							// InvoiceType
							st.thirdPartyPurchasingOption = new ThirdPartyPurchasingOptionType ();
							st.thirdPartyPurchasingOption.isOptionChoosen = thirdPartyFlag;
							st.thirdPartyPurchasingOption.isOptionChoosenSpecified = true;

							st.detailedReturnInfo = new DetailedReturnInfoType ();
							st.detailedReturnInfo.order = orderFlag;
							st.detailedReturnInfo.orderSpecified = true;
							st.detailedReturnInfo.isRepossessed = repossessionFlag;
							st.detailedReturnInfo.isRepossessedSpecified = true;

							st.finalAssetSettlementPurchasePrice = new MonetaryValueType ();
							st.finalAssetSettlementPurchasePrice.value = getDecimal (contractData.RemainingBalance);
							st.finalAssetSettlementPurchasePrice.valueSpecified = true;

							try
							{
								st.detailedReturnInfo.followUpDate = getDate (contractData.RepossessionFollowupDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
								st.detailedReturnInfo.followUpDateSpecified = true;
							}
							catch (Exception) { _log.Warn ("Date for detailedReturnInfo.followUpDate not valid or empty"); }
						}

						int ic = offset;
						int rncounter = 0;
						retailContract.postings = new PostingType[rnData.Count];

						foreach (CreateColCaseInDto rn in rnData)
						{
							ColCaseRNDto ccdata = context.ExecuteStoreQuery<ColCaseRNDto> ("select sysperson customerreference,sysid invoiceid,rechnung invoicenumber,CASE WHEN (ART > 0) THEN (gbetrag + gsteuer) * (-1) ELSE (gbetrag + gsteuer) END invoiceoriginalamount, CASE WHEN (ART > 0) THEN (gbetrag + gsteuer + mahnbetrag + zinsen + gebuehr - (teilzahlung + anzahlung + skonto + storno)) * (-1) ELSE (gbetrag + gsteuer + mahnbetrag + zinsen + gebuehr - (teilzahlung + anzahlung + skonto + storno)) END invoiceopenamount, zinsen lateinterestamount,text invoicetext,beleg2 invoicenumberalphabet, valutadatum InvoicePostingDate,case when (weinzugab <= to_date('01.01.0111','dd.mm.yyyy') OR weinzugab IS NULL) then valutadatum else weinzugab end InvoiceDueDate,esr ddreturnreasoncode,zinsdatum lateinterestupdatedate from rn where sysid=" + rn.DataId, null).FirstOrDefault ();
							ccdata.InvoiceNumber = trimString (ccdata.InvoiceNumber);
							ccdata.InvoiceText = trimString (ccdata.InvoiceText);

							//————————————————————————————————————————————————————————————————————————————————————————————————————
							// rh 20171121:			
							// old path: RetailContract.collectionCase.collectionData.arrearsCalculationDate
							// new path: RetailContract.<retailInvoice.RetailContract.>collectionCase.collectionData.arrearsCalculationDate
							//————————————————————————————————————————————————————————————————————————————————————————————————————
							// do *NOT* send to old path: 
							//————————————————————————————————————————————————————————————————————————————————————————————————————
							//if (ccdata.lateinterestupdatedate != null && ccdata.lateinterestupdatedate.Year > 1900)
							//{
							//	 retailContract.collectionCase[0].collectionData[0].arrearsCalculationDate = ccdata.lateinterestupdatedate;
							//   retailContract.collectionCase[0].collectionData[0].arrearsCalculationDateSpecified=true;
							//}
							//————————————————————————————————————————————————————————————————————————————————————————————————————

							retailContract.retailInvoice[ic] = new RetailInvoiceType ();
							if (rn.InvoiceType == null || !rn.InvoiceType.Equals ("RATE"))
							{
								retailContract.retailInvoice[ic] = new BulkInvoiceType ();
								((BulkInvoiceType) retailContract.retailInvoice[ic]).bulkInvoiceId = ccdata.InvoiceNumberAlphabet;
							}

							RetailInvoiceType invoice = retailContract.retailInvoice[ic];

							// InvoiceID
							invoice.document = new DocumentType[1];
							invoice.document[0] = new DocumentType ();
							invoice.document[0].documentIdentifier = ccdata.InvoiceID;
							// InvoiceNumber
							invoice.number = ccdata.InvoiceNumber;

							// InvoiceOriginalAmount
							invoice.totalAmount = new MonetaryValueType ();
							invoice.totalAmount.value = ccdata.InvoiceOriginalAmount;
							invoice.totalAmount.valueSpecified = true;
							// InvoiceOpenAmount
							invoice.retailContract = new RetailContractType ();
							invoice.retailContract.number = vtdata.ContractReference;
							invoice.retailContract.outstandingAmount = new MonetaryValueType ();
							invoice.retailContract.outstandingAmount.value = ccdata.InvoiceOpenAmount;
							invoice.retailContract.outstandingAmount.valueSpecified = true;

							// ONLY FOR postingType.amount: IN QUESTION IF rncounter indexing IS STILL NEEDED (rh: email Zeitler 20171121)
							retailContract.postings[rncounter] = new PostingType ();

							retailContract.postings[rncounter].amount = new MonetaryValueType ();
							retailContract.postings[rncounter].amount.value = ccdata.InvoiceOpenAmount;
							retailContract.postings[rncounter].amount.valueSpecified = true;

							//————————————————————————————————————————————————————————————————————————————————————————————————————
							// rh 20171121: 
							// postingType in other location, too (mb+rh 20171113)
							// old path: retailContract.postings[rncounter].postingType
							// new path: retailContract.<retailInvoice.retailContract.>postings[0].postingType
							//————————————————————————————————————————————————————————————————————————————————————————————————————
							// do *NOT* send to old path: 
							// retailContract.postings[rncounter].postingType = rn.InvoiceType;
							//————————————————————————————————————————————————————————————————————————————————————————————————————

							// new path: retailContract.<retailInvoice.retailContract.>postings[0].postingType
							invoice.retailContract.postings = new PostingType[1];
							invoice.retailContract.postings[0] = new PostingType ();
							invoice.retailContract.postings[0].postingType = rn.InvoiceType;
							//————————————————————————————————————————————————————————————————————————————————————————————————————

							// InvoiceDueDate
							invoice.dueDate = ccdata.InvoiceDueDate;
							invoice.dueDateSpecified = true;
							invoice.description = ccdata.InvoiceText;
							// InvoicePostingDate
							invoice.date = ccdata.InvoicePostingDate;
							invoice.dateSpecified = true;


							invoice.position = new InvoicePositionType[1];
							invoice.position[0] = new InvoicePositionType ();
							invoice.position[0].amount = new MonetaryValueType ();
							invoice.position[0].amount.value = ccdata.LateInterestAmount;
							invoice.position[0].amount.valueSpecified = true;

							//————————————————————————————————————————————————————————————————————————————————————————————————————
							// DDReturnReasonCode
							//————————————————————————————————————————————————————————————————————————————————————————————————————
							// old path: retailContract.transaction[0].DDReturnCode
							// new path: retailContract.<retailInvoice.retailContract.>transaction[0].DDReturnCode
							//————————————————————————————————————————————————————————————————————————————————————————————————————
							// do *NOT* send to old path: 
							//retailContract.transaction = new PaymentType[1];
							//retailContract.transaction[0] = new PaymentType();
							//retailContract.transaction[0].DDReturnCode = ccdata.DDReturnReasonCode;

							// DDReturnCode: new path: (mb+rh 20171113)
							invoice.retailContract.transaction = new PaymentType[1];
							invoice.retailContract.transaction[0] = new PaymentType ();
							invoice.retailContract.transaction[0].DDReturnCode = ccdata.DDReturnReasonCode;
							//————————————————————————————————————————————————————————————————————————————————————————————————————


							if (ccdata.lateinterestupdatedate != null && ccdata.lateinterestupdatedate.Year > 1900)
							{
								// rh 20171121: 
								// old path: RetailContract.collectionCase.collectionData.arrearsCalculationDate
								// new path: RetailContract.<retailInvoice.RetailContract.>collectionCase.collectionData.arrearsCalculationDate

								// new path:
								invoice.retailContract.collectionCase = new CollectionCaseType[1];
								invoice.retailContract.collectionCase[0] = new CollectionCaseType ();
								invoice.retailContract.collectionCase[0].collectionData = new CollectionDataType[1];
								invoice.retailContract.collectionCase[0].collectionData[0] = new CollectionDataType ();

								invoice.retailContract.collectionCase[0].collectionData[0].arrearsCalculationDate = ccdata.lateinterestupdatedate;
								invoice.retailContract.collectionCase[0].collectionData[0].arrearsCalculationDateSpecified = true;
							}

							ic++;
							rncounter++;
						}

						//—————————————————————————————————————————————————————————————————————
						// VEHICLE
						//—————————————————————————————————————————————————————————————————————
						/*CreateColCaseInDto vehicleData = (from t in inputdataList
														  where t.DataType.Equals("OB")
														  select t).FirstOrDefault();
						if (vehicleData != null)*/
						{
							ColCaseOBDto obdata = context.ExecuteStoreQuery<ColCaseOBDto> ("select trim(ob.hersteller) carbrand,trim(ob.fabrikat) carmodel,trim( ob.serie) carvin, obini.erstzul car1stregistrationdate,trim(ob.kennzeichen) carlicenceplate,trim(obini.farbe_a) carcolor from ob,obini where obini.SYSOBINI=ob.sysob and ob.sysvt=" + contractData.DataId, null).FirstOrDefault ();
							if (obdata != null)
							{
								// CarBrand
								retailContract.bmwSFBrand = new BMWSFBrandType ();
								retailContract.bmwSFBrand.brandName = obdata.CarBrand;

								// VehicleType data
								retailContract.financedAsset = new VehicleType ();
								VehicleType vehicle = (VehicleType) retailContract.financedAsset;
								vehicle.dateOfFirstRegistration = obdata.Car1stRegistrationDate;
								vehicle.dateOfFirstRegistrationSpecified = true;

								vehicle.estimatedCurrentTradeValue = new EstimatedCurrentTradeValueType[1];
								vehicle.estimatedCurrentTradeValue[0] = new EstimatedCurrentTradeValueType ();
								vehicle.estimatedCurrentTradeValue[0].estimatedValue = new MonetaryValueType ();
								//vehicle.estimatedCurrentTradeValue[0].estimatedValue.value = getDecimal (vehicleData.CurrentCarValue);
								vehicle.estimatedCurrentTradeValue[0].estimatedValue.valueSpecified = false;
								vehicle.vehicleAppraisal = new ShortVehicleAppraisalType ();
								if (contractData.VehicleExpertiseFollowupDate != null)
								{
									try
									{
										vehicle.vehicleAppraisal.followUpDate = getDate (contractData.VehicleExpertiseFollowupDate.Trim (), dformat, CultureInfo.InvariantCulture).Value;
										vehicle.vehicleAppraisal.followUpDateSpecified = true;
									}
									catch (Exception) { _log.Warn ("Date vehicleAppraisal.followUpDate not valid or empty"); }
								}
								vehicle.vehicleAppraisal.status = contractData.VehicleExpertiseOrder;
								if (contractData.DateVehicleExpertiseCompleted != null)
								{
									vehicle.vehicleAppraisal.document = new DocumentType ();
									{
										try
										{
											vehicle.vehicleAppraisal.document.creationDate = getDate (contractData.DateVehicleExpertiseCompleted.Trim (), dformat, CultureInfo.InvariantCulture).Value;
											vehicle.vehicleAppraisal.document.creationDateSpecified = true;
										}
										catch (Exception) { _log.Warn ("Date vehicleAppraisal.creationDate not valid or empty"); }
									}
								}
								vehicle.usedVehicleSalesPricing = new UsedVehicleSalesPricingType ();
								if (contractData.DateOfSale != null)
								{
									try
									{
										vehicle.usedVehicleSalesPricing.dateOfSale = getDate (contractData.DateOfSale.Trim (), dformat, CultureInfo.InvariantCulture).Value;
										vehicle.usedVehicleSalesPricing.dateOfSaleSpecified = true;
									}
									catch (Exception) { _log.Warn ("Date usedVehicleSalesPricing.dateOfSale not valid or empty"); }
								}
								vehicle.make = new VehicleMakeType ();
								vehicle.make.name = new I18nStringType[1];
								vehicle.make.name[0] = new I18nStringType ();
								vehicle.make.name[0].name = obdata.CarBrand;

								// CarModel
								vehicle.model = new VehicleModelType ();
								vehicle.model.name = new I18nStringType[1];
								vehicle.model.name[0] = new I18nStringType ();
								vehicle.model.name[0].name = obdata.CarModel;
								// CarVIN
								vehicle.vIN = obdata.CarVIN;
								// CarLicencePlate
								vehicle.licencePlate = new LicencePlateType[1];
								vehicle.licencePlate[0] = new LicencePlateType ();
								vehicle.licencePlate[0].licence = obdata.CarLicencePlate;
								// CarColor
								vehicle.exteriorColour = new ExteriorColourType[1];
								vehicle.exteriorColour[0] = new ExteriorColourType ();
								vehicle.exteriorColour[0].colour = new I18nStringType ();
								vehicle.exteriorColour[0].colour.name = obdata.CarColor;
							}
						}
					}

					//—————————————————————————
					// createColCase
					//—————————————————————————
					ColCaseMgmtClient client = new ColCaseMgmtClient ();
					client.Endpoint.EndpointBehaviors.Add (new SOAPLogging ());
					MessageHeaderType messageHeader = new MessageHeaderType ();
					messageHeader.individualUserIdentification = new IdentificationType ();
					client.ClientCredentials.UserName.UserName = AppConfig.getValueFromDb ("SETUP", "SCORE", "USERNAME", "QQISAT3");
					client.ClientCredentials.UserName.Password = AppConfig.getValueFromDb ("SETUP", "SCORE", "PASSWORD", "QQISAT3");

					messageHeader.correlationID = AppConfig.getValueFromDb ("SETUP", "SCORE", "CORRELATIONID", "1424141");
					messageHeader.globalCorrelationID = AppConfig.getValueFromDb ("SETUP", "SCORE", "GLOBALCORRELATIONID", "");

					using (OperationContextScope scope = new OperationContextScope (client.InnerChannel))
					{
						var httpRequestProperty = new HttpRequestMessageProperty ();
						httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Basic " +
						  Convert.ToBase64String (Encoding.ASCII.GetBytes (client.ClientCredentials.UserName.UserName + ":" + client.ClientCredentials.UserName.Password));

						OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

						if (update)
							client.updateColCase (ref messageHeader, retailContract);   // MB: da void, haben wir KEINE MÖGLICHKEIT für Status-Handling 
						else
							client.createColCase (ref messageHeader, retailContract);   // MB: da void, haben wir KEINE MÖGLICHKEIT für Status-Handling 
					}
					/*  eaihot.OUTPUTPARAMETER1 = "OK";			// MB: OUTPUTPARAMETER-Sample: wenn wir bis hierher gekommen sind, dann 
					  eaihot.OUTPUTPARAMETER2 = "";
					  eaihot.OUTPUTPARAMETER3 = "";*/
				}
				catch (Exception e)
				{
                    eaihot.PROZESSSTATUS = 3;
                    eaihot.OUTPUTPARAMETER1 = "FEHLER";
					eaihot.OUTPUTPARAMETER3 = stripString (e.Message);
					String prefix = "Create";
					if (update)
						prefix = "Update";
					_log.Error ("Error in " + prefix + "ColCaseAdapter", e);
				}
				//—————————————————————————
				// UPDATE EAIHOT 
				//—————————————————————————
				dao.updateEaihot (eaihot);
			}
		}

		/// <summary>
		/// Adapter for calling UPDATECOLCASE from eaihot
		/// </summary>
		private class UpdateColCaseAdapter : IEaiBOSAdapter
		{
			public void processEaiHot (IEaihotDao dao, EaihotDto eaihot)
			{
				CreateColCaseAdapter mapperToCreateColCase = new CreateColCaseAdapter ();
				mapperToCreateColCase.processEaiHot (dao, eaihot, true);
			}
		}
	}
}
