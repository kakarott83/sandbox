using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Cic.One.Web.BO.Search;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.One.Web.BO;

namespace Cic.One.Web.DAO
{
    public class RecurrDao : Cic.One.Web.DAO.IRecurrDao
    {
        /// <summary>
        /// Sucht Appointments inklusive den Recurrences
        /// </summary>
        /// <param name="input">Parameter</param>
        /// <returns></returns>
        public List<ApptmtDto> searchApptmts(iSearchApptmtsWithRecurrDto input, long sysPerole)
        {
            DateTime endDate = input.EndDate;
            DateTime startDate = input.StartDate;

            SearchBo<RecurrDto> recurrbo = new SearchBo<RecurrDto>(SearchQueryFactoryFactory.getInstance(), sysPerole);
            SearchBo<ApptmtDto> apptmtbo = new SearchBo<ApptmtDto>(SearchQueryFactoryFactory.getInstance(), sysPerole);

            List<Filter> filtersRecurr = new List<Filter>();
            List<Filter> filtersApptmt = new List<Filter>();
            if (input.SearchParameter.filters != null)
            {
                filtersRecurr = input.SearchParameter.filters.ToList();
                filtersApptmt = input.SearchParameter.filters.ToList();
            }

            //Falls kein Ownerfilter vorhanden ist
            if (filtersRecurr.Where((a) => a.filterType == FilterType.OWNER).FirstOrDefault() == null)
            {
                filtersRecurr.Add(new Filter()
                    {
                        fieldname = "PEROLECACHE.SYSPARENT",
                        filterType = FilterType.Equal,
                        value = sysPerole.ToString()
                    });

                //Dann kann auch der Apptmtfilter keinen haben
                filtersApptmt.Add(new Filter()
                {
                    fieldname = "PEROLECACHE.SYSPARENT",
                    filterType = FilterType.Equal,
                    value = sysPerole.ToString()
                });
            }

            //Filter, dass die Recurrs im Intervall liegen
            filtersRecurr.Add(new Filter()
            {
                fieldname = "RECURR.STARTDATE",
                filterType = FilterType.DateLE,
                value = string.Format("{0:yyyy-MM-dd}", endDate)
            });
            filtersRecurr.Add(new Filter()
            {
                fieldname = "",
                filterType = FilterType.SQL,
                value = " RECURR.ENDDATE is null or trunc(RECURR.ENDDATE) >= to_date('" + string.Format("{0:yyyy-MM-dd}", startDate) + "', 'yyyy-mm-dd') "
            });

            //Restliche Apptmts-Filter
            //Filter dass die Apptmts im Intervall liegen
            filtersApptmt.Add(new Filter()
            {
                fieldname = "APPTMT.STARTDATE",
                filterType = FilterType.DateLE,
                value = string.Format("{0:yyyy-MM-dd}", endDate)
            });
            filtersApptmt.Add(new Filter()
            {
                fieldname = "APPTMT.ENDDATE",
                filterType = FilterType.DateGE,
                value = string.Format("{0:yyyy-MM-dd}", startDate)
            });
            //Filter, dass es keine Recurrence hat
            filtersApptmt.Add(new Filter()
            {
                fieldname = "",
                filterType = FilterType.SQL,
                value = "NOT EXISTS(select R.SYSAPPTMT FROM RECURR R where R.SYSAPPTMT=APPTMT.SYSAPPTMT)"
            });
            using (DdOwExtended ctx = new DdOwExtended())
            {


                input.SearchParameter.searchType = SearchType.Partial;
                input.SearchParameter.pageSize = 1000;
                DateTime start = DateTime.Now;
                //Suche alle möglichen Recurrs
                input.SearchParameter.filters = filtersRecurr.ToArray();
                var recurrs = recurrbo.search(input.SearchParameter);
                var recurrsIds = recurrs.results.Select((a) => a.sysApptmt);

                //Lädt alle apptmts
                var apptmts = Mapper.Map<List<APPTMT>, List<ApptmtDto>>((from app in ctx.APPTMT
                                                                         where recurrsIds.Contains(app.SYSAPPTMT)
                                                                         select app).ToList());
                //Erzeugt alle Recurring Apptmts zu einer Recurrence
                var recurringApptmts = from app in apptmts
                                       from rec in recurrs.results
                                       where rec.sysApptmt == app.sysApptmt
                                       select CreateApptmts(rec, Mapper.Map(app, new ApptmtDto()), startDate, endDate);

                //Wäre schöner so: (Geht aber leider nicht, weil man Entities mit normalen Klassen mischt)
                //Erzeuge alle Reccurring Apptmts anhand von der Recurrence und dem zugehörigen Apptmt
                //var recurringApptmts = from app in ctx.APPTMT
                //             from rec in recurrs.results
                //             where rec.sysApptmt == app.SYSAPPTMT
                //             select CreateApptmts(rec, Mapper.Map(app, new ApptmtDto()), startDate, endDate);

                //List<List<..>> wird abgeflacht zu List<..>
                var result = recurringApptmts.SelectMany(a => a).ToList();

                //Restliche Apptmts, welche keine Recurr haben und im Intervall liegen werden gesucht
                input.SearchParameter.filters = filtersApptmt.ToArray();
                var restApptmts = apptmtbo.search(input.SearchParameter);

                //und zusammengefügt
                result.AddRange(restApptmts.results);

                Debug.WriteLine("searchApptmtsWithRecurr took: " + (DateTime.Now - start).TotalMilliseconds + " ms");

                return result;
            }

         
        }

        /// <summary>
        /// Erstellt ein Recurring ApptmtDto aus einem APPTMT und einer Startzeit
        /// </summary>
        /// <param name="app"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private ApptmtDto CreateRecurringApptmt(ApptmtDto app, DateTime startDate)
        {
            ApptmtDto apptmt = Mapper.Map(app, new ApptmtDto());
            var dif = (startDate - apptmt.startDate.Value.Date).Days;
            apptmt.startDate = apptmt.startDate.Value.AddDays(dif);
            apptmt.endDate = apptmt.endDate.Value.AddDays(dif);
            apptmt.recurring = true;
            return apptmt;
        }

        /// <summary>
        /// Liefert einen Array von den Tagen, welche in RECURR ausgewählt sind
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        private DayOfWeek[] GetDays(RecurrDto rec)
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            if (CheckValue(rec.onMondays))
                days.Add(DayOfWeek.Monday);
            if (CheckValue(rec.onTuesdays))
                days.Add(DayOfWeek.Tuesday);
            if (CheckValue(rec.onWednesdays))
                days.Add(DayOfWeek.Wednesday);
            if (CheckValue(rec.onThursdays))
                days.Add(DayOfWeek.Thursday);
            if (CheckValue(rec.onFridays))
                days.Add(DayOfWeek.Friday);
            if (CheckValue(rec.onSaturdays))
                days.Add(DayOfWeek.Saturday);
            if (CheckValue(rec.onSundays))
                days.Add(DayOfWeek.Sunday);
            if (CheckValue(rec.onWorkdays))
            {
                days.Add(DayOfWeek.Monday);
                days.Add(DayOfWeek.Tuesday);
                days.Add(DayOfWeek.Wednesday);
                days.Add(DayOfWeek.Thursday);
                days.Add(DayOfWeek.Friday);
            }
            return days.ToArray();
        }

        /// <summary>
        /// Überprüft ob ein Wert existiert und dieser 1 ist.
        /// Wurde verwendet, weil es anfangs mit RECURR gemacht wurde, welches Nullable Types hat
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool CheckValue(int? val)
        {
            return val.HasValue && val.Value == 1;
        }

        /// <summary>
        /// Erzeugt alle Apptmts zu einer Recurrence welche in einem bestimmten Intervall liegen.
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ApptmtDto> CreateApptmts(RecurrDto rec, ApptmtDto apptmt, DateTime startDate, DateTime endDate)
        {
            List<DateTime> dates = new List<DateTime>();
            startDate = startDate.Date;
            endDate = endDate.Date;
            var apptmtLength = (apptmt.endDate.Value - apptmt.startDate.Value).Days;
            var days = GetDays(rec);
            DateTime currentDate = rec.startDate.Value.Date;
            int numb = 0;
            switch (rec.period)
            {
                //Daily Pattern
                case 1:
                    {
                        //Entspricht dem Weekly Pattern
                        if (rec.onWorkdays == 1)
                        {
                            while (currentDate <= endDate)
                            {
                                //Falls das Ende des Apptmts im gesuchten Intervall liegt.
                                //und der Tag zwischen Montag und Freitag liegt.
                                if (currentDate.AddDays(apptmtLength) >= startDate && days.Contains(currentDate.DayOfWeek))
                                {
                                    dates.Add(currentDate);
                                }
                                numb++;

                                //Falls wir über der Anzahl an Wiederholungen sind, aufhören
                                if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr <= numb)
                                    break;

                                currentDate = currentDate.AddDays(1);
                                //Falls wir über dem Ende der Recurrence sind, aufhören
                                if (rec.endDate.HasValue && rec.endDate.Value.Date < currentDate)
                                    break;
                            }
                        }
                        else
                        {
                            if (!rec.intervall.HasValue)
                                break;

                            //Jump to Start
                            // |----| <- gesuchter Zeitraum
                            // -|----| <- Länge des Apptmts, falls es vorne rausschaut
                            //_____-|----| <- _ Differenz zwischen aktuellem Datum und dem Start
                            //Damit gleich direkt in den Intervall gesprungen wird, kann Ceiling verwendet werden.
                            //Floor springt zu dem, welches sich davor befindet.
                            int stepsToJump = (int)Math.Ceiling((double)(startDate.AddDays(-apptmtLength) - currentDate).Days / rec.intervall.Value);

                            if (stepsToJump > 0)
                            {
                                numb += stepsToJump;
                                //Falls wir schon über der Anzahl an Wiederholungen sind, aufhören
                                if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr < numb)
                                    break;

                                currentDate = currentDate.AddDays(rec.intervall.Value * stepsToJump);
                            }

                            while (currentDate <= endDate)
                            {
                                //Falls das Ende des Apptmts im gesuchten Intervall liegt.
                                if (currentDate.AddDays(apptmtLength) >= startDate)
                                {
                                    dates.Add(currentDate);
                                }
                                numb++;

                                //Falls wir über der Anzahl an Wiederholungen sind, aufhören
                                if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr <= numb)
                                    break;

                                currentDate = currentDate.AddDays(rec.intervall.Value);
                                //Falls wir über dem Ende der Recurrence sind, aufhören
                                if (rec.endDate.HasValue && rec.endDate.Value.Date < currentDate)
                                    break;
                            }
                        }
                    }
                    break;
                //Weekly-Pattern
                case 2:
                    {
                        if (days.Length == 0 || !rec.intervall.HasValue)
                            break;

                        //TODO Jump

                        int currentWeek = WeekInYear(currentDate);
                        while (currentDate <= endDate)
                        {
                            //Falls das Ende des Apptmts im gesuchten Intervall liegt.
                            //und der Tag zwischen Montag und Freitag liegt.
                            if (currentDate.AddDays(apptmtLength) >= startDate && days.Contains(currentDate.DayOfWeek))
                            {
                                dates.Add(currentDate);
                            }

                            numb++;
                            //Falls wir über der Anzahl an Wiederholungen sind, aufhören
                            if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr <= numb)
                                break;

                            currentDate = currentDate.AddDays(1);
                            if (currentWeek != WeekInYear(currentDate))
                            {
                                //Woche hat sich geändert
                                //Intervall 1: Jede Woche, also soll nicht gesprungen werden
                                //Intervall 2: Jede 2. Woche, also wird um 7 Tage gesprungen.
                                //...
                                currentDate = currentDate.AddDays((rec.intervall.Value - 1) * 7);
                                currentWeek = WeekInYear(currentDate);
                            }
                            //Falls wir über dem Ende der Recurrence sind, aufhören
                            if (rec.endDate.HasValue && rec.endDate.Value.Date < currentDate)
                                break;
                        }
                    }
                    break;
                //Monthly-Pattern
                case 3:
                    {
                        if (rec.dayWeekIndex.HasValue)
                        {
                            //RelativeMonthly
                            if (!rec.intervall.HasValue)
                                break;

                            int dayinweek = rec.dayWeekIndex.Value;

                            DateTime date = GetNthDayOfWeekInMonth(currentDate, dayinweek, days);
                            //Falls der Nte Tag noch nicht war, fängt das zählen erst im nächsten Monat an
                            if (date <= currentDate)
                            {
                                //Exchange springt auch gleich über den Intervall.
                                currentDate = currentDate.AddMonths(rec.intervall.Value);
                                currentDate = GetNthDayOfWeekInMonth(currentDate, dayinweek, days);
                            }
                            else
                            {
                                currentDate = date;
                            }

                            //Jump zum Start des gesuchten Intervalls
                            int stepsToJump = (int)Math.Ceiling((double)(startDate.AddDays(-apptmtLength).Month - currentDate.Month) / rec.intervall.Value);
                            if (stepsToJump > 0)
                            {
                                numb += stepsToJump;
                                //Falls wir schon über der Anzahl an Wiederholungen sind, aufhören
                                if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr < numb)
                                    break;

                                currentDate = currentDate.AddMonths(rec.intervall.Value * stepsToJump);
                                currentDate = GetNthDayOfWeekInMonth(currentDate, dayinweek, days);
                            }

                            while (currentDate <= endDate)
                            {
                                //Falls das Ende des Apptmts im gesuchten Intervall liegt.
                                //und der Tag zwischen Montag und Freitag liegt.
                                if (currentDate.AddDays(apptmtLength) >= startDate)
                                {
                                    dates.Add(currentDate);
                                }

                                numb++;
                                //Falls wir über der Anzahl an Wiederholungen sind, aufhören
                                if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr <= numb)
                                    break;

                                //Intervall 1: Spring zum nächsten Monat
                                //Intervall 2: Spring 2 Monate
                                //...
                                currentDate = currentDate.AddMonths(rec.intervall.Value);
                                //und wählt den Nten Tag.
                                currentDate = GetNthDayOfWeekInMonth(currentDate, dayinweek, days);

                                //Falls wir über dem Ende der Recurrence sind, aufhören
                                if (rec.endDate.HasValue && rec.endDate.Value.Date < currentDate)
                                    break;
                            }
                        }
                        else
                        {
                            if (!rec.dayIndex.HasValue || !rec.intervall.HasValue)
                                break;

                            int dayindex = rec.dayIndex.Value;
                            int currentMonth = currentDate.Month;

                            if (currentDate <= CreateMaxDate(currentDate.Year, currentMonth, dayindex))
                            {
                                //Falls das Startdatum vor dem Tag ist, an welchem das Pattern sich befindet
                                //Muss dieses Datum schon hinzugefügt werden
                                currentDate = CreateMaxDate(currentDate.Year, currentMonth, dayindex);
                            }
                            else
                            {
                                //Ansonsten muss man zum nächsten Monat wechseln.
                                //wird über AddMonths gemacht, damit von Dezember schon auf Januar richtig umgeschaltet wird
                                currentDate = currentDate.AddMonths(1);
                                currentDate = CreateMaxDate(currentDate.Year, currentDate.Month, dayindex);
                            }

                            //TODO Jump to Start of Intervall?

                            while (currentDate <= endDate)
                            {
                                //Falls das Ende des Apptmts im gesuchten Intervall liegt.
                                //und der Tag zwischen Montag und Freitag liegt.
                                if (currentDate.AddDays(apptmtLength) >= startDate)
                                {
                                    dates.Add(currentDate);
                                }

                                numb++;
                                //Falls wir über der Anzahl an Wiederholungen sind, aufhören
                                if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr <= numb)
                                    break;

                                //Intervall 1: Spring zum nächsten Monat
                                //Intervall 2: Spring 2 Monate
                                //...
                                currentDate = currentDate.AddMonths(rec.intervall.Value);

                                //Falls der Tagesindex nicht mehr stimmt müssen Anpassungen gemacht werden. (Kann nur kleiner werden.)
                                if (currentDate.Day < dayindex)
                                {
                                    int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                                    if (currentDate.Day < daysInMonth)
                                    {
                                        int toMove = Math.Min(daysInMonth, dayindex) - currentDate.Day;
                                        currentDate = currentDate.AddDays(toMove);
                                    }
                                }

                                //Falls wir über dem Ende der Recurrence sind, aufhören
                                if (rec.endDate.HasValue && rec.endDate.Value.Date < currentDate)
                                    break;
                            }
                        }
                    }
                    break;
                //Yearly
                case 4:
                    {
                        if (rec.dayWeekIndex.HasValue)
                        {
                            //RelativeYearly
                            if (!rec.monthIndex.HasValue)
                                break;

                            int dayinweek = rec.dayWeekIndex.Value;
                            int monthindex = rec.monthIndex.Value;

                            DateTime date = GetNthDayOfWeekInMonth(currentDate.Year,monthindex, dayinweek, days);
                            //Falls der Nte Tag noch nicht war, fängt das zählen erst im nächsten Monat an
                            if (date <= currentDate)
                            {
                                //Exchange springt auch gleich über den Intervall.
                                currentDate = GetNthDayOfWeekInMonth(currentDate.Year+1,monthindex, dayinweek, days);
                            }
                            else
                            {
                                currentDate = date;
                            }

                            //TODO Jump?
                            //Jump zum Start des gesuchten Intervalls
                            //int stepsToJump = (int)Math.Ceiling((double)(startDate.AddDays(-apptmtLength).Month - currentDate.Month) / rec.intervall.Value);
                            //if (stepsToJump > 0)
                            //{
                            //    numb += stepsToJump;
                            //    //Falls wir schon über der Anzahl an Wiederholungen sind, aufhören
                            //    if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr < numb)
                            //        break;

                            //    currentDate = currentDate.AddMonths(rec.intervall.Value * stepsToJump);
                            //    currentDate = GetNthDayOfWeekInMonth(currentDate, dayinweek, days);
                            //}

                            while (currentDate <= endDate)
                            {
                                //Falls das Ende des Apptmts im gesuchten Intervall liegt.
                                //und der Tag zwischen Montag und Freitag liegt.
                                if (currentDate.AddDays(apptmtLength) >= startDate)
                                {
                                    dates.Add(currentDate);
                                }

                                numb++;
                                //Falls wir über der Anzahl an Wiederholungen sind, aufhören
                                if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr <= numb)
                                    break;

                                //Spring zum nächsten Jahr
                                //und wählt den Nten Tag.
                                currentDate = GetNthDayOfWeekInMonth(currentDate.Year+1, monthindex, dayinweek, days);

                                //Falls wir über dem Ende der Recurrence sind, aufhören
                                if (rec.endDate.HasValue && rec.endDate.Value.Date < currentDate)
                                    break;
                            }
                        }
                        else
                        {
                            if (!rec.monthIndex.HasValue || !rec.dayIndex.HasValue)
                                break;

                            int dayindex = rec.dayIndex.Value;
                            int monthindex = rec.monthIndex.Value;

                            if (currentDate <= CreateMaxDate(currentDate.Year,monthindex,dayindex))
                            {
                                //Falls das Startdatum vor dem Tag ist, an welchem das Pattern sich befindet
                                //Muss dieses Datum schon hinzugefügt werden
                                currentDate = CreateMaxDate(currentDate.Year, monthindex, dayindex);
                            }
                            else
                            {
                                //Ansonsten muss man zum nächsten Jahr wechseln.
                                currentDate = CreateMaxDate(currentDate.Year + 1, monthindex, dayindex);
                            }

                            //TODO Jump to Start of Intervall?
                            while (currentDate <= endDate)
                            {
                                //Falls das Ende des Apptmts im gesuchten Intervall liegt.
                                //und der Tag zwischen Montag und Freitag liegt.
                                if (currentDate.AddDays(apptmtLength) >= startDate)
                                {
                                    dates.Add(currentDate);
                                }

                                numb++;
                                //Falls wir über der Anzahl an Wiederholungen sind, aufhören
                                if (rec.numberRecurr.HasValue && rec.numberRecurr > 0 && rec.numberRecurr <= numb)
                                    break;

                                //Spring zum nächsten Jahr
                                currentDate = currentDate.AddYears(1);

                                //Falls der Tagesindex nicht mehr stimmt müssen Anpassungen gemacht werden. (Kann nur kleiner werden.)
                                //Kann sein, dass man Februar den 31 gewählt hat...
                                if (currentDate.Day < dayindex)
                                {
                                    int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                                    if (currentDate.Day < daysInMonth)
                                    {
                                        int toMove = Math.Min(daysInMonth, dayindex) - currentDate.Day;
                                        currentDate = currentDate.AddDays(toMove);
                                    }
                                }

                                //Falls wir über dem Ende der Recurrence sind, aufhören
                                if (rec.endDate.HasValue && rec.endDate.Value.Date < currentDate)
                                    break;
                            }
                        }
                    }
                    break;
            }
            return dates.Select((d) => CreateRecurringApptmt(apptmt, d)).ToList();
        }

        /// <summary>
        /// Falls der Tag ausserhalb des Monats liegt, wieder der maximale Tag des Monats erzeugt.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private DateTime CreateMaxDate(int year, int month, int day)
        {
            return new DateTime(year, month, Math.Min(DateTime.DaysInMonth(year, month), day));
        }


        /// <summary>
        /// Sucht den Nten Wochentag im Monat.
        /// z.b 3. Mittwoch
        /// Falls Dayinweek >=5, wird der letzte Tag gesucht (also letzte Mittwoch)
        /// </summary>
        /// <param name="date">Datetime in dem gesucht werden soll (Jahr und Monat)</param>
        /// <param name="nth">nte-Nummer welche gesucht werden soll</param>
        /// <param name="days">Tage, welche erlaubt sind</param>
        /// <returns></returns>
        private DateTime GetNthDayOfWeekInMonth(DateTime date, int nth, DayOfWeek[] days)
        {
            return GetNthDayOfWeekInMonth(date.Year, date.Month, nth, days);
        }

        /// <summary>
        /// Sucht den Nten Wochentag im Monat.
        /// z.b 3. Mittwoch
        /// Falls Dayinweek >=5, wird der letzte Tag gesucht (also letzte Mittwoch)
        /// </summary>
        /// <param name="year">Jahr in dem gesucht werden soll</param>
        /// <param name="month">Monat in dem gesucht werden soll</param>
        /// <param name="nth">nte-Nummer welche gesucht werden soll</param>
        /// <param name="days">Tage, welche erlaubt sind</param>
        /// <returns></returns>
        private DateTime GetNthDayOfWeekInMonth(int year, int month, int nth, DayOfWeek[] days)
        {
            DateTime start = new DateTime(year, month, 1);
            if (nth >= 5)
            {
                //Suche letzten Tag
                start = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                //bis er einen Wochentag findet geht er einen zurück
                while (!days.Contains(start.DayOfWeek))
                {
                    start = start.AddDays(-1);
                }
            }
            else
            {
                if (days.Length == 1)
                {
                    //Sucht die erste Vorkommnis des Tags
                    start = NextDay(start, days.FirstOrDefault());
                    //und geht x Wochen weiter (das geht leider nur, wenn man einen Tag erlaubt)
                    start = start.AddDays((nth - 1) * 7);
                }
                else
                {
                    int numb = 0;
                    for (int i = 1; i < DateTime.DaysInMonth(year, month); i++)
                    {
                        //sucht den x-ten Tag, welcher einem der übergebenen Tage entspricht.
                        if (days.Contains(start.DayOfWeek))
                        {
                            numb++;
                            if (numb >= nth)
                            {
                                return start;
                            }
                        }
                        start = start.AddDays(1);
                    }
                }
            }
            return start;
        }

        ///<summary>Gets the first week day following a date.</summary>
        ///<param name="date">The date.</param>
        ///<param name="dayOfWeek">The day of week to return.</param>
        ///<returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns>
        public DateTime NextDay(DateTime date, DayOfWeek dayOfWeek)
        {
            return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
        }

        /// <summary>
        /// Berechnet die Woche des Jahrs von einem Datum
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekInYear(DateTime date)
        {
            GregorianCalendar cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}