namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenOne.Common.Model.DdOl;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    #endregion

    public static class ZustandHelper
    {
        #region Methods
        public static bool VerifyAngebotStatus(long sysAngebot, DdOlExtended context, params AngebotZustand[] allowedStatuses)
        {
            // Query ANGEBOT
            String zustand = (from Angebot in context.ANGEBOT
                                  where Angebot.SYSID == sysAngebot
                                  select Angebot.ZUSTAND).FirstOrDefault();

            // Check if ANGEBOT was found
            if (zustand == null)
            {
                // Throw an exception
                throw new Exception("Specified angebot could not be found.");
            }

            // Get the status from ANGEBOT
            AngebotZustand CurrentStatus = GetEnumeratorFromString(zustand);

            // Return true or false depending on whether the status was found in the array or not
            return Array.Exists<AngebotZustand>(allowedStatuses, Status => Status == CurrentStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <param name="context"></param>
        /// <param name="allowedStatuses"></param>
        /// <returns></returns>
        public static bool AntragHasStatus(long sysAngebot, DdOlExtended context, params AntragZustand[] allowedStatuses)
        {
            // Query ANGEBOT
            String antragZustand = context.ExecuteStoreQuery<String>("select zustand from Antrag where sysangebot="+sysAngebot,null).FirstOrDefault();

            if (antragZustand == null) return false;

            // Get the status from ANGEBOT
            AntragZustand CurrentStatus = GetEnumeratorFromStringAntragZustand(antragZustand);

            // Return true or false depending on whether the status was found in the array or not
            return Array.Exists<AntragZustand>(allowedStatuses, Status => Status == CurrentStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <param name="context"></param>
        /// <param name="allowedStatuses"></param>
        /// <returns></returns>
        public static bool AntragHasNotStatus(long sysAngebot, DdOlExtended context, params AntragZustand[] allowedStatuses)
        {
            // Query ANGEBOT
            String antragZustand = context.ExecuteStoreQuery<String>("select zustand from Antrag where sysangebot=" + sysAngebot, null).FirstOrDefault();

            if (antragZustand == null) return true;

            // Get the status from ANGEBOT
            AntragZustand CurrentStatus = GetEnumeratorFromStringAntragZustand(antragZustand);

            // Return true or false depending on whether the status was found in the array or not
            return !Array.Exists<AntragZustand>(allowedStatuses, Status => Status == CurrentStatus);
        }

        public static void SetAngebotStatus(CIC.Database.OL.EF6.Model.ANGEBOT CurrentAngebot, long sysAngebot, DdOlExtended context, AngebotZustand newStatus)
        {
            // Query ANGEBOT
            if(CurrentAngebot==null)
                CurrentAngebot = (from Angebot in context.ANGEBOT
                                  where Angebot.SYSID == sysAngebot
                                  select Angebot).FirstOrDefault();

            // Check if ANGEBOT was found
            if (CurrentAngebot == null)
            {
                // Throw an exception
                throw new Exception("Specified angebot could not be found.");
            }

            // Write the new status and status change time and set Aktivkz
            CurrentAngebot.ZUSTAND = GetStringFromEnumerator(newStatus);
            if (VerifyAngebotStatus(sysAngebot, context, AngebotZustand.Storniert,AngebotZustand.StornoWiedereinreichung, AngebotZustand.Abgelaufen))
            {
                CurrentAngebot.AKTIVKZ = 0;
            }
            else
            {
                CurrentAngebot.AKTIVKZ = 1;
            }
            CurrentAngebot.ZUSTANDAM = DateTime.Now;

            try
            {
                // Save the changes
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                // Throw an exception
                throw new Exception("The angebot could not be saved.", exception);
            }
        }

        public static string GetStringFromEnumerator(AngebotZustand zustand)
        {
            FieldInfo Field = typeof(AngebotZustand).GetField(zustand.ToString());
            DescriptionAttribute Description = (DescriptionAttribute)Attribute.GetCustomAttribute(Field, typeof(DescriptionAttribute));

            if (Description != null)
            {
                return Description.Description;
            }

            return string.Empty;
        }

        public static AntragZustand GetEnumeratorFromStringAntragZustand(string zustand)
        {

            foreach (AntragZustand LoopValue in Enum.GetValues(typeof(AntragZustand)))
            {
                FieldInfo Field = typeof(AntragZustand).GetField((LoopValue.ToString()));
                if (Field == null) continue;
                DescriptionAttribute Description = (DescriptionAttribute)Attribute.GetCustomAttribute(Field, typeof(DescriptionAttribute));

                if (Description != null && zustand != null && Description.Description.ToUpper() == zustand.ToUpper())
                {
                    return LoopValue;
                }
            }

            return AntragZustand.Undefined;
        }

        public static AngebotZustand GetEnumeratorFromString(string zustand)
        {

            foreach (AngebotZustand LoopValue in Enum.GetValues(typeof(AngebotZustand)))
            {
                FieldInfo Field = typeof(AngebotZustand).GetField((LoopValue.ToString()));
                DescriptionAttribute Description = (DescriptionAttribute)Attribute.GetCustomAttribute(Field, typeof(DescriptionAttribute));

                if (Description != null && zustand != null  && Description.Description.ToUpper() == zustand.ToUpper())
                {
                    return LoopValue;
                }
            }

            return AngebotZustand.Undefined;
        }
        #endregion
    }
}