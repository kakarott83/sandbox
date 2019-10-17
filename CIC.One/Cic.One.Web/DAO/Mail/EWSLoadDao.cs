using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;

namespace Cic.One.Web.DAO.Mail
{
    /// <summary>
    /// Ist für das Laden der Eigenschaften für Exchange 2007 zuständig.
    /// </summary>
    public class EWSLoadDao
    {
        /// <summary>
        /// Lädt alle benötigten Eigenschaften für alle Items
        /// </summary>
        /// <param name="results">Ergebnis aus der Suche</param>
        /// <param name="service">Offene ExchangeService Instanz</param>
        public static void LoadProperties(IEnumerable<Item> results, ExchangeService service)
        {
            if (results == null)
                return;

            var contacts = new List<Contact>();
            var contactgroups = new List<ContactGroup>();
            var emails = new List<EmailMessage>();
            var appointments = new List<Appointment>();
            var tasks = new List<Task>();

            foreach (Item i in results)
            {
                TryAdd(i, contacts);
                TryAdd(i, contactgroups);
                TryAdd(i, emails);
                TryAdd(i, appointments);
                TryAdd(i, tasks);
            }

            TryLoad(contacts, ContactSet, service);
            TryLoad(contactgroups, ContactGroupSet, service);
            TryLoad(emails, EmailMessageSet, service);
            TryLoad(appointments, AppointmentSet, service);
            TryLoad(tasks, TaskSet, service);
        }

        private static void TryLoad<T>(List<T> items, PropertySet itemSet, ExchangeService service)
            where T : Item
        {
            if (items != null && items.Count > 0)
            {
                service.LoadPropertiesForItems(items, itemSet);

                //foreach (var item in items)
                //{
                //    //foreach (var attachement in item.Attachments)
                //    //    attachement.Load();
                //}
            }
        }

        private static void TryAdd<T>(Item i, List<T> contacts) where T : class
        {
            if (i is T)
            {
                contacts.Add(i as T);
            }
        }

        public static readonly PropertySet AppointmentSet = new PropertySet(
                ItemSchema.Subject,
                ItemSchema.Body,
                ItemSchema.Categories,
                ItemSchema.Importance,
                ItemSchema.Sensitivity,
                ItemSchema.DateTimeSent,
                ItemSchema.DateTimeReceived,
                ItemSchema.Attachments,

                AppointmentSchema.RequiredAttendees,
                AppointmentSchema.OptionalAttendees,
                AppointmentSchema.Resources,
                AppointmentSchema.End,
                AppointmentSchema.Start,
                AppointmentSchema.Location,
                AppointmentSchema.IsAllDayEvent,
                AppointmentSchema.Recurrence
                );

        public static readonly PropertySet EmailMessageSet = new PropertySet(
                ItemSchema.Subject,
                ItemSchema.Body,
                ItemSchema.Categories,
                ItemSchema.Importance,
                ItemSchema.Sensitivity,
                ItemSchema.DateTimeSent,
                ItemSchema.DateTimeReceived,
                ItemSchema.Attachments,

                EmailMessageSchema.ToRecipients,
                EmailMessageSchema.CcRecipients,
                EmailMessageSchema.BccRecipients,
                EmailMessageSchema.IsRead,
                EmailMessageSchema.From,
                EmailMessageSchema.IsResponseRequested,
                EmailMessageSchema.IsDeliveryReceiptRequested,
                EmailMessageSchema.IsReadReceiptRequested,
                EmailMessageSchema.IsDraft
                );

        public static readonly PropertySet ContactGroupSet = new PropertySet(
                ItemSchema.Subject,
                ItemSchema.Body,
                ItemSchema.Categories,
                ItemSchema.Importance,
                ItemSchema.Sensitivity,
                ItemSchema.DateTimeSent,
                ItemSchema.DateTimeReceived,
                ItemSchema.Attachments,

                ContactGroupSchema.DisplayName
                );

        public static readonly PropertySet ContactSet = new PropertySet(
                ItemSchema.Subject,
                ItemSchema.Body,
                ItemSchema.Categories,
                ItemSchema.Importance,
                ItemSchema.Sensitivity,
                ItemSchema.DateTimeSent,
                ItemSchema.DateTimeReceived,
                ItemSchema.Attachments,

                ContactSchema.DisplayName,
                ContactSchema.GivenName,
                ContactSchema.EmailAddress1,
                ContactSchema.EmailAddress2,
                ContactSchema.EmailAddress3,
                ContactSchema.MobilePhone,
                ContactSchema.HomePhone,
                ContactSchema.HomePhone2,
                ContactSchema.BusinessPhone,
                ContactSchema.BusinessPhone2,
                ContactSchema.PrimaryPhone,
                ContactSchema.CompanyMainPhone,
                ContactSchema.AssistantPhone,
                ContactSchema.CarPhone,
                ContactSchema.BusinessFax,
                ContactSchema.HomeFax,
                ContactSchema.OtherFax,
                ContactSchema.Isdn,
                ContactSchema.OtherTelephone,
                ContactSchema.Pager,
                ContactSchema.RadioPhone,
                ContactSchema.Initials,
                ContactSchema.CompanyName,
                ContactSchema.Department,
                ContactSchema.Surname
                );

        public static readonly PropertySet TaskSet = new PropertySet(
                ItemSchema.Subject,
                ItemSchema.Body,
                ItemSchema.Categories,
                ItemSchema.Importance,
                ItemSchema.Sensitivity,
                ItemSchema.DateTimeSent,
                ItemSchema.DateTimeReceived,
                ItemSchema.Attachments,

                TaskSchema.Mode,
                TaskSchema.StartDate,
                TaskSchema.DueDate,
                TaskSchema.Status,
                TaskSchema.IsComplete,
                TaskSchema.IsTeamTask,
                TaskSchema.CompleteDate,
                TaskSchema.Recurrence
                );
    }
}