using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOw;
using Microsoft.Exchange.WebServices.Data;
using Cic.OpenOne.Common.Util;

namespace Cic.One.DTO
{
    public class MappingProfileMail : MappingProfileBase
    {
        /// <summary>
        /// Convertiert eine Liste von Emails zu einem Dictionary
        /// </summary>
        public class ListEmailAddressesTypeConverter : ITypeConverter<List<MEmailAddressDto>, EmailAddressDictionary>
        {
            /// <summary>
            /// Implementierung des Interfaces
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public EmailAddressDictionary Convert(ResolutionContext context)
            {
                var v = context.SourceValue as List<MEmailAddressDto>;
                var dict = new EmailAddressDictionary();
                if (v != null)
                {
                    if (v.Count > 0)
                    {
                        dict[EmailAddressKey.EmailAddress1] = Mapper.Map<MEmailAddressDto, EmailAddress>(v[0]);
                        if (v.Count > 1)
                        {
                            dict[EmailAddressKey.EmailAddress2] = Mapper.Map<MEmailAddressDto, EmailAddress>(v[1]);
                            if (v.Count > 2)
                                dict[EmailAddressKey.EmailAddress3] = Mapper.Map<MEmailAddressDto, EmailAddress>(v[2]);
                        }
                    }
                }
                return dict;
            }
        }

        /// <summary>
        /// Erzeugt aus einem string eine Liste an Email-Adressen
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<MEmailAddressDto> StringToEmailList(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new List<MEmailAddressDto>();
            var items = str.Split(';');
            if (items == null)
                return new List<MEmailAddressDto>();

            return items.Select((a) => new MEmailAddressDto() { Address = a }).ToList();
        }

        /// <summary>
        /// Erzeugt aus einer Liste an Email-Adressen einen string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string EmailListToString(List<MEmailAddressDto> items)
        {
            return string.Join(";", items.Select(a => a.Address));
            
        }

        

        /// <summary>
        /// Wandelt einen string in einen MMessageBody um
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public MMessageBodyDto StringToMailBody(string str)
        {
            return new MMessageBodyDto()
            {
                BodyType = MBodyTypeEnum.HTML,
                Text = str
            };
        }

        protected override void Configure()
        {
            base.Configure();

            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            // DTOs zur Datenbank
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------

            Mapper.CreateMap<MImportanceEnum?, int?>();

            //Missing content
            Mapper.CreateMap<MEmailMessageDto, MAILMSG>()
                .ForMember(dest => dest.ITEMID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SENTTO, opt => opt.MapFrom(src => src.ToRecipients))
                .ForMember(dest => dest.SENTTOCC, opt => opt.MapFrom(src => src.CcRecipients))
                .ForMember(dest => dest.SENTTOBCC, opt => opt.MapFrom(src => src.BccRecipients))
                .ForMember(dest => dest.RESPREQFLAG, opt => opt.MapFrom(src => src.IsResponseRequested))
                .ForMember(dest => dest.DELREQFLAG, opt => opt.MapFrom(src => src.IsDeliveryReceiptRequested))
                .ForMember(dest => dest.READRECPTREQFLAG, opt => opt.MapFrom(src => src.IsReadReceiptRequested))
                .ForMember(dest => dest.PRIORITY, opt => opt.MapFrom(src => src.Importance))
                .ForMember(dest => dest.READFLAG, opt => opt.MapFrom(src => src.IsRead))
                .ForMember(dest => dest.RECVDATE, opt => opt.MapFrom(src => src.DateTimeReceived))
                .ForMember(dest => dest.RECVTIME, opt => opt.MapFrom(src => DateTimeHelper.DateTimeNullableToClarionTime(src.DateTimeReceived)))
                .ForMember(dest => dest.RECVFROM, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.SENTDATE, opt => opt.MapFrom(src => src.DateTimeSent))
                .ForMember(dest => dest.SENTTIME, opt => opt.MapFrom(src => DateTimeHelper.DateTimeNullableToClarionTime(src.DateTimeSent)))
                ;

            //Missing privateFlag
            Mapper.CreateMap<MTaskDto, PTASK>()

                //.ForMember(dest => dest.sub, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.COMPLETEFLAG, opt => opt.MapFrom(src => src.IsComplete))
                .ForMember(dest => dest.DUEDATE, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.DUETIME, opt => opt.MapFrom(src => DateTimeHelper.DateTimeNullableToClarionTime(src.DueDate)))
                .ForMember(dest => dest.ITEMID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PRIORITY, opt => opt.MapFrom(src => src.Importance))
                .ForMember(dest => dest.PROZESSSTATUS, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.STARTDATE, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.STARTTIME, opt => opt.MapFrom(src => DateTimeHelper.DateTimeNullableToClarionTime(src.StartDate)))
                .ForMember(dest => dest.TASKMODE, opt => opt.MapFrom(src => src.Mode))
                .ForMember(dest => dest.TEAMFLAG, opt => opt.MapFrom(src => src.IsTeamTask))
                ;

            //Missing ShowAs, privateFlag, content
            Mapper.CreateMap<MAppointmentDto, APPTMT>()
                .ForMember(dest => dest.ALLDAYFLAG, opt => opt.MapFrom(src => src.IsAllDayEvent))
                .ForMember(dest => dest.ENDDATE, opt => opt.MapFrom(src => src.End))
                .ForMember(dest => dest.ENDTIME, opt => opt.MapFrom(src => DateTimeHelper.DateTimeToClarionTime(src.End)))
                .ForMember(dest => dest.ITEMID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LOCATION, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.PRIORITY, opt => opt.MapFrom(src => src.Importance))
                .ForMember(dest => dest.STARTDATE, opt => opt.MapFrom(src => src.Start))
                .ForMember(dest => dest.STARTTIME, opt => opt.MapFrom(src => DateTimeHelper.DateTimeNullableToClarionTime(src.Start)))
                ;

            Mapper.CreateMap<MMessageBodyDto, string>().ConstructUsing((a) => a.Text);

            //TODO
            Mapper.CreateMap<MEmailAddressDto, string>().ConstructUsing((a) => a.Address);

            Mapper.CreateMap<List<MEmailAddressDto>, string>().ConvertUsing(EmailListToString);

            Mapper.CreateMap<MFileAttachement, FILEATT>()
                .ForMember(dest => dest.CONTID, opt => opt.MapFrom(src => src.ContentId))
                .ForMember(dest => dest.FILELOCATION, opt => opt.MapFrom(src => src.ContentLocation))
                .ForMember(dest => dest.ATTID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FILESIZE, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.FILENAME, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ACTIVEFLAG, opt => opt.UseValue(new Nullable<int>(1))); //set Active as default

            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            // Datenbank zu DTOs
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------

            Mapper.CreateMap<MAILMSG, MEmailMessageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ITEMID))
                .ForMember(dest => dest.ToRecipients, opt => opt.MapFrom(src => src.SENTTO))
                .ForMember(dest => dest.CcRecipients, opt => opt.MapFrom(src => src.SENTTOCC))
                .ForMember(dest => dest.BccRecipients, opt => opt.MapFrom(src => src.SENTTOBCC))
                .ForMember(dest => dest.IsResponseRequested, opt => opt.MapFrom(src => src.RESPREQFLAG))
                .ForMember(dest => dest.IsDeliveryReceiptRequested, opt => opt.MapFrom(src => src.DELREQFLAG))
                .ForMember(dest => dest.IsReadReceiptRequested, opt => opt.MapFrom(src => src.READRECPTREQFLAG))
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => src.PRIORITY))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.SUBJECT))
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.READFLAG))
                .ForMember(dest => dest.Sensitivity, opt => opt.MapFrom(src => (src.PRIVATEFLAG == 1) ? Sensitivity.Private : Sensitivity.Normal))

                //.ForMember(dest => dest.From, opt => opt.MapFrom(src => src.RECVFROM))
                    ;

            Mapper.CreateMap<PTASK, MTaskDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ITEMID))

                //.ForMember(dest => dest.Mode, opt => opt.MapFrom(src => src.TASKMODE))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTimeHelper.CreateDate(src.STARTDATE, src.STARTTIME)))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => DateTimeHelper.CreateDate(src.DUEDATE, src.DUETIME)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PROZESSSTATUS))

                //.ForMember(dest => dest.IsTeamTask, opt => opt.MapFrom(src => src.TEAMFLAG))
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => src.PRIORITY))
                .ForMember(dest => dest.Sensitivity, opt => opt.MapFrom(src => (src.PRIVATEFLAG == 1) ? Sensitivity.Private : Sensitivity.Normal))
                .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.COMPLETEFLAG))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    {
                        if (src.COMPLETEFLAG != null && src.COMPLETEFLAG.Value == 1)
                            return MTaskStatusEnum.Completed;
                        else
                            return MTaskStatusEnum.InProgress;
                    }));
                ;

            Mapper.CreateMap<APPTMT, MAppointmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ITEMID))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => DateTimeHelper.CreateDate(src.STARTDATE, src.STARTTIME)))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => DateTimeHelper.CreateDate(src.ENDDATE, src.ENDTIME)))
                .ForMember(dest => dest.IsAllDayEvent, opt => opt.MapFrom(src => src.ALLDAYFLAG))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.LOCATION))
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => src.PRIORITY))
                .ForMember(dest => dest.Sensitivity, opt => opt.MapFrom(src => (src.PRIVATEFLAG == 1) ? Sensitivity.Private : Sensitivity.Normal))
                .ForMember(dest => dest.LegacyFreeBusyStatus, opt => opt.MapFrom(src => src.SHOWAS))
                ;

            Mapper.CreateMap<int, Nullable<MImportanceEnum>>().ConstructUsing((a) => new Nullable<MImportanceEnum>((MImportanceEnum)a));

            //TODO vielleicht den a-1 zu a machen.
            Mapper.CreateMap<int, Nullable<MLegacyFreeBusyStatus>>().ConstructUsing((a) => new Nullable<MLegacyFreeBusyStatus>((MLegacyFreeBusyStatus)(a)));

            //TODO welches Format hat die From
            Mapper.CreateMap<string, MEmailAddressDto>().ConstructUsing((a) => new MEmailAddressDto() { Address = a });

            Mapper.CreateMap<string, MMessageBodyDto>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src));

            //.ConvertUsing(StringToMailBody);

            Mapper.CreateMap<FILEATT, MFileAttachement>()
                .ForMember(dest => dest.ContentId, opt => opt.MapFrom(src => src.CONTID))
                .ForMember(dest => dest.ContentLocation, opt => opt.MapFrom(src => src.FILELOCATION))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ATTID))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.FILESIZE))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FILENAME));

            Mapper.CreateMap<RECURR, MRecurrence>()
                .ForMember(dest => dest.NumberOfOccurrences, opt => opt.MapFrom(src => src.NUMBERRECURR))
                ;

            Mapper.CreateMap<string, List<MEmailAddressDto>>().ConvertUsing(StringToEmailList);

            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            // DTOs zu Exchange
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------

            Mapper.CreateMap<MItemDto, Item>()
                .Include<MEmailMessageDto, EmailMessage>()
                .Include<MContactDto, Contact>()
                .Include<MAppointmentDto, Appointment>()
                .Include<MContactGroupDto, ContactGroup>()
                .Include<MTaskDto, Task>()

                //.ForMember(dest=>dest.Categories,opt=>opt.MapFrom(src=>src.Categories))
                ;

            Mapper.CreateMap<MEmailAddressDto, EmailAddress>()
                .Include<MAttendeeDto, Attendee>();

            Mapper.CreateMap<MEmailMessageDto, EmailMessage>()
                .ForMember(dest => dest.IsReadReceiptRequested, opt => opt.Condition(new Func<MEmailMessageDto, bool>((a) => a.IsReadReceiptRequested != null)))
                .ForMember(dest => dest.IsDeliveryReceiptRequested, opt => opt.Condition(new Func<MEmailMessageDto, bool>((a) => a.IsDeliveryReceiptRequested != null)));

            Mapper.CreateMap<MContactDto, Contact>();
            Mapper.CreateMap<MAppointmentDto, Appointment>();
            Mapper.CreateMap<MContactGroupDto, ContactGroup>();
            Mapper.CreateMap<MMessageBodyDto, MessageBody>();
            Mapper.CreateMap<MAttendeeDto, Attendee>();
            Mapper.CreateMap<MTaskDto, Task>();

            Mapper.CreateMap<MRecurrence, Recurrence>().ConvertUsing(MToRecurrence);

            Mapper.CreateMap<List<string>, StringList>().ConstructUsing((a) => new StringList(a));

            Mapper.CreateMap<List<MEmailAddressDto>, EmailAddressDictionary>()
                .ConvertUsing<ListEmailAddressesTypeConverter>();

            Mapper.CreateMap<MItemChangeDto, ItemChange>();
            Mapper.CreateMap<MChangeDto, Change>();

            Mapper.CreateMap<MNameResolutionDto, NameResolution>();

            Mapper.CreateMap<MFileAttachement, FileAttachment>();
            Mapper.CreateMap<MFileAttachement, Attachment>();

            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            // Exchange zu DTOs
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------

            Mapper.CreateMap<Item, MItemDto>()
                .Include<EmailMessage, MEmailMessageDto>()
                .Include<Contact, MContactDto>()
                .Include<Appointment, MAppointmentDto>()
                .Include<ContactGroup, MContactGroupDto>()
                .Include<Task, MTaskDto>();

            Mapper.CreateMap<EmailAddress, MEmailAddressDto>()
                .Include<Attendee, MAttendeeDto>();

            Mapper.CreateMap<EmailMessage, MEmailMessageDto>()
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => { try { return new Nullable<Importance>(src.Importance); } catch { return null; } }))
                .ForMember(dest => dest.Sensitivity, opt => opt.MapFrom(src => { try { return new Nullable<Sensitivity>(src.Sensitivity); } catch { return null; } }))
                .ForMember(dest => dest.DateTimeReceived, opt => opt.MapFrom(src => { try { return new Nullable<DateTime>(src.DateTimeReceived); } catch { return null; } }))
                .ForMember(dest => dest.DateTimeSent, opt => opt.MapFrom(src => { try { return new Nullable<DateTime>(src.DateTimeSent); } catch { return null; } }))
                .ForMember(dest => dest.IsReadReceiptRequested, opt => opt.MapFrom(src => { try { return new Nullable<bool>(src.IsReadReceiptRequested); } catch { return null; } }))
                .ForMember(dest => dest.IsDeliveryReceiptRequested, opt => opt.MapFrom(src => { try { return new Nullable<bool>(src.IsDeliveryReceiptRequested); } catch { return null; } }));

            Mapper.CreateMap<Contact, MContactDto>()
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => { try { return new Nullable<Importance>(src.Importance); } catch { return null; } }))
                .ForMember(dest => dest.Sensitivity, opt => opt.MapFrom(src => { try { return new Nullable<Sensitivity>(src.Sensitivity); } catch { return null; } }))
                .ForMember(dest => dest.DateTimeReceived, opt => opt.MapFrom(src => { try { return new Nullable<DateTime>(src.DateTimeReceived); } catch { return null; } }))
                .ForMember(dest => dest.DateTimeSent, opt => opt.MapFrom(src => { try { return new Nullable<DateTime>(src.DateTimeSent); } catch { return null; } }))
                .ForMember(dest => dest.ContactSource, opt => opt.MapFrom(src => { try { return src.ContactSource; } catch { return null; } }));

            Mapper.CreateMap<Appointment, MAppointmentDto>();
            Mapper.CreateMap<ContactGroup, MContactGroupDto>()
                .ForMember(dest => dest.Members, opt => opt.Ignore());

            Mapper.CreateMap<MessageBody, MMessageBodyDto>();
            Mapper.CreateMap<Attendee, MAttendeeDto>();
            Mapper.CreateMap<Task, MTaskDto>();
            Mapper.CreateMap<ExpandGroupResults, MContactGroupDto>();

            Mapper.CreateMap<EmailAddressDictionary, List<MEmailAddressDto>>().ConvertUsing(EmailAddressDictionaryToList);
            Mapper.CreateMap<PhoneNumberDictionary, Dictionary<string, string>>().ConvertUsing(PhoneNumberDictionaryToDictionary);
            Mapper.CreateMap<PhysicalAddressDictionary, Dictionary<string, MPhysicalAddress>>().ConvertUsing(PhysicalAddressDictionaryToDictionary);
            Mapper.CreateMap<PhysicalAddressEntry, MPhysicalAddress>();

            Mapper.CreateMap<Recurrence, MRecurrence>().ConvertUsing(RecurrenceToM);

            Mapper.CreateMap<ItemChange, MItemChangeDto>();
            Mapper.CreateMap<Change, MChangeDto>();

            Mapper.CreateMap<NameResolution, MNameResolutionDto>();

            Mapper.CreateMap<Attachment, MFileAttachement>().ConvertUsing((a) =>
                {
                    if (a is FileAttachment)
                        return Mapper.Map(a as FileAttachment, new MFileAttachement());
                    else
                        return null;// new MFileAttachement();
                })

                //.Include<FileAttachment, MFileAttachement>()
                //.ForMember(dest => dest.Size, opt => opt.MapFrom(src => { try { return src.Size; } catch { return 0; } }))
                ;

            Mapper.CreateMap<FileAttachment, MFileAttachement>()
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => { try { return src.Content.Length; } catch { return 0; } }))
                ;
        }

        public MRecurrence RecurrenceToM(Recurrence rec)
        {
            if (rec is Recurrence.DailyPattern)
            {
                var item = rec as Recurrence.DailyPattern;
                return new MRecurrence()
                {
                    Intervall = item.Interval,
                    Period = 1,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    NumberOfOccurrences = item.NumberOfOccurrences
                };
            }
            else if (rec is Recurrence.WeeklyPattern)
            {
                var item = rec as Recurrence.WeeklyPattern;
                if (item.DaysOfTheWeek.Count == 1 && item.DaysOfTheWeek.Contains(DayOfTheWeek.Weekday))
                {
                    return new MRecurrence()
                    {
                        Intervall = item.Interval,
                        Period = 1,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        NumberOfOccurrences = item.NumberOfOccurrences,
                        OnWorkdays = true,
                    };
                }
                else
                    return new MRecurrence()
                    {
                        Intervall = item.Interval,
                        Period = 2,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        NumberOfOccurrences = item.NumberOfOccurrences,
                        OnFridays = item.DaysOfTheWeek.Contains(DayOfTheWeek.Friday),
                        OnMondays = item.DaysOfTheWeek.Contains(DayOfTheWeek.Monday),
                        OnSaturdays = item.DaysOfTheWeek.Contains(DayOfTheWeek.Saturday),
                        OnSundays = item.DaysOfTheWeek.Contains(DayOfTheWeek.Sunday),
                        OnThursdays = item.DaysOfTheWeek.Contains(DayOfTheWeek.Thursday),
                        OnTuesdays = item.DaysOfTheWeek.Contains(DayOfTheWeek.Tuesday),
                        OnWednesdays = item.DaysOfTheWeek.Contains(DayOfTheWeek.Wednesday),
                        OnWorkdays = item.DaysOfTheWeek.Contains(DayOfTheWeek.Weekday),
                    };
            }
            else if (rec is Recurrence.RelativeMonthlyPattern)
            {
                var item = rec as Recurrence.RelativeMonthlyPattern;
                return new MRecurrence()
                {
                    Intervall = item.Interval,
                    Period = 3,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    NumberOfOccurrences = item.NumberOfOccurrences,
                    DayWeekIndex = ((int)item.DayOfTheWeekIndex + 1),
                    OnFridays = item.DayOfTheWeek==(DayOfTheWeek.Friday),
                    OnMondays = item.DayOfTheWeek==(DayOfTheWeek.Monday),
                    OnSaturdays = item.DayOfTheWeek==(DayOfTheWeek.Saturday),
                    OnSundays = item.DayOfTheWeek==(DayOfTheWeek.Sunday),
                    OnThursdays = item.DayOfTheWeek==(DayOfTheWeek.Thursday),
                    OnTuesdays = item.DayOfTheWeek==(DayOfTheWeek.Tuesday),
                    OnWednesdays = item.DayOfTheWeek==(DayOfTheWeek.Wednesday),
                    OnWorkdays = item.DayOfTheWeek==(DayOfTheWeek.Weekday),
                };
            }
            else if (rec is Recurrence.MonthlyPattern)
            {
                var item = rec as Recurrence.MonthlyPattern;
                return new MRecurrence()
                {
                    Intervall = item.Interval,
                    Period = 3,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    NumberOfOccurrences = item.NumberOfOccurrences,
                    DayIndex = item.DayOfMonth,
                };
            }
            else if (rec is Recurrence.RelativeYearlyPattern)
            {
                var item = rec as Recurrence.RelativeYearlyPattern;
                return new MRecurrence()
                {
                    Period = 3,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    NumberOfOccurrences = item.NumberOfOccurrences,
                    DayWeekIndex = ((int)item.DayOfTheWeekIndex + 1),
                    OnFridays = item.DayOfTheWeek == (DayOfTheWeek.Friday),
                    OnMondays = item.DayOfTheWeek == (DayOfTheWeek.Monday),
                    OnSaturdays = item.DayOfTheWeek == (DayOfTheWeek.Saturday),
                    OnSundays = item.DayOfTheWeek == (DayOfTheWeek.Sunday),
                    OnThursdays = item.DayOfTheWeek == (DayOfTheWeek.Thursday),
                    OnTuesdays = item.DayOfTheWeek == (DayOfTheWeek.Tuesday),
                    OnWednesdays = item.DayOfTheWeek == (DayOfTheWeek.Wednesday),
                    OnWorkdays = item.DayOfTheWeek == (DayOfTheWeek.Weekday),
                    MonthIndex = (int)item.Month,
                };
            }
            else if (rec is Recurrence.YearlyPattern)
            {
                var item = rec as Recurrence.YearlyPattern;
                return new MRecurrence()
                {
                    Period = 3,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    NumberOfOccurrences = item.NumberOfOccurrences,
                    MonthIndex = (int)item.Month,
                    DayIndex = item.DayOfMonth
                };
            }
            return new MRecurrence();
        }

        public Recurrence MToRecurrence(MRecurrence rec)
        {
            if (rec == null)
                return null;

            Recurrence r = null;
            switch (rec.Period)
            {
                case 1:
                    if (//rec.OnFridays || rec.OnMondays|| rec.OnSaturdays|| rec.OnSundays|| rec.OnThursdays|| rec.OnTuesdays|| rec.OnWednesdays||
                        rec.OnWorkdays)
                    {
                        r = new Recurrence.WeeklyPattern(rec.StartDate, 1, GetDays(rec)) { NumberOfOccurrences = rec.NumberOfOccurrences };
                        break;
                    }
                    r = new Recurrence.DailyPattern(rec.StartDate, rec.Intervall) {  NumberOfOccurrences = rec.NumberOfOccurrences };
                    break;
                case 2:
                    r = new Recurrence.WeeklyPattern(rec.StartDate, rec.Intervall, GetDays(rec)) {  NumberOfOccurrences = rec.NumberOfOccurrences };
                    break;
                case 3:
                    if (rec.DayWeekIndex.HasValue)
                    {
                        r = new Recurrence.RelativeMonthlyPattern(rec.StartDate, rec.Intervall, GetDays(rec).FirstOrDefault(), (DayOfTheWeekIndex)(rec.DayWeekIndex.Value - 1)) {  NumberOfOccurrences = rec.NumberOfOccurrences };
                        break;
                    }
                    r = new Recurrence.MonthlyPattern(rec.StartDate, rec.Intervall, rec.DayIndex) {  NumberOfOccurrences = rec.NumberOfOccurrences };
                    break;
                case 4:
                    if (rec.DayWeekIndex.HasValue)
                    {
                        r = new Recurrence.RelativeYearlyPattern(rec.StartDate, (Month)rec.MonthIndex, GetDays(rec).FirstOrDefault(), (DayOfTheWeekIndex)(rec.DayWeekIndex.Value - 1)) {   NumberOfOccurrences = rec.NumberOfOccurrences };
                        break;
                    }
                    r = new Recurrence.YearlyPattern(rec.StartDate, (Month)rec.MonthIndex, rec.DayIndex) {  NumberOfOccurrences = rec.NumberOfOccurrences };
                    break;
            }
            if (rec.EndDate.HasValue)
            {
                r.EndDate = rec.EndDate.Value;
            }
            return r;
        }

        private DayOfTheWeek[] GetDays(MRecurrence rec)
        {
            List<DayOfTheWeek> days = new List<DayOfTheWeek>();
            if (rec.OnMondays)
                days.Add(DayOfTheWeek.Monday);
            if (rec.OnTuesdays)
                days.Add(DayOfTheWeek.Tuesday);
            if (rec.OnWednesdays)
                days.Add(DayOfTheWeek.Wednesday);
            if (rec.OnThursdays)
                days.Add(DayOfTheWeek.Thursday);
            if (rec.OnFridays)
                days.Add(DayOfTheWeek.Friday);
            if (rec.OnSaturdays)
                days.Add(DayOfTheWeek.Saturday);
            if (rec.OnSundays)
                days.Add(DayOfTheWeek.Sunday);
            if (rec.OnWorkdays)
                days.Add(DayOfTheWeek.Weekday);
            return days.ToArray();
        }

        public Dictionary<string, MPhysicalAddress> PhysicalAddressDictionaryToDictionary(PhysicalAddressDictionary dict)
        {
            var res = new Dictionary<string, MPhysicalAddress>();
            foreach (PhysicalAddressKey key in Enum.GetValues(typeof(PhysicalAddressKey)))
            {
                PhysicalAddressEntry item;
                if (dict.TryGetValue(key, out item))
                {
                    res.Add(key.ToString(), Mapper.Map(item, new MPhysicalAddress()));
                }
            }
            return res;
        }

        public Dictionary<string, string> PhoneNumberDictionaryToDictionary(PhoneNumberDictionary dict)
        {
            var res = new Dictionary<string, string>();
            foreach (PhoneNumberKey key in Enum.GetValues(typeof(PhoneNumberKey)))
            {
                string item;
                if (dict.TryGetValue(key, out item))
                {
                    res.Add(key.ToString(), item);
                }
            }
            return res;
        }

        public Dictionary<string, MEmailAddressDto> EmailAddressDictionaryToDictionary(EmailAddressDictionary dict)
        {
            var res = new Dictionary<string, MEmailAddressDto>();
            foreach (EmailAddressKey key in Enum.GetValues(typeof(EmailAddressKey)))
            {
                EmailAddress item;
                if (dict.TryGetValue(key, out item))
                {
                    res.Add(key.ToString(), Mapper.Map(item, new MEmailAddressDto()));
                }
            }
            return res;
        }

        public List<MEmailAddressDto> EmailAddressDictionaryToList(EmailAddressDictionary dict)
        {
            var res = new List<MEmailAddressDto>();
            foreach (EmailAddressKey key in Enum.GetValues(typeof(EmailAddressKey)))
            {
                EmailAddress item;
                if (dict.TryGetValue(key, out item))
                {
                    res.Add(Mapper.Map(item, new MEmailAddressDto()));
                }
            }
            return res;
        }
    }
}