using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    /// <summary>
    /// State Control Data Access Object
    /// UI Test
    /// </summary>
    public class StateControl
    {
       private string _ProcessZeit;
       private double ini = 0;
       private  double end = 0;
       private string _StatusCode;

        /// <summary>
        /// Set Ini
        /// </summary>
        public void setIni()
        {
            ini = DateTime.Now.TimeOfDay.TotalMilliseconds;
            _ProcessZeit = "";
            setStatusCode("Neuer Prozess gestartet..." + _ProcessZeit);
            
        }

        /// <summary>
        /// Set End
        /// </summary>
        public void setEnd()
        {
            end = DateTime.Now.TimeOfDay.TotalMilliseconds;
             CalculateProcessZeit();
             setStatusCode("Prozess abgeschlossen     " + _ProcessZeit);
            
        }

        /// <summary>
        /// Set Error
        /// </summary>
        public void setError()
        {

            setStatusCode("Prozess nicht erfolgreich abgeschlossen" + _ProcessZeit);
        }

        /// <summary>
        /// Calculate Process Time
        /// </summary>
        public void CalculateProcessZeit()
        {  double zeit = end-ini;
           _ProcessZeit = zeit.ToString();
        }

        /// <summary>
        /// Process Time
        /// </summary>
        public string ProcessZeit
        { get { return _ProcessZeit; }
            set
            {
               
            }
         
        }

        /// <summary>
        /// Status Code
        /// </summary>
        public string StatusCode
        { get { return _StatusCode; }
            set
            {
               
            }
         
        }

        /// <summary>
        /// Set Status Code
        /// </summary>
        /// <param name="s"></param>
        public void setStatusCode(string s)
        {
            _StatusCode=s;

        } 

        /// <summary>
        /// Changed Event
        /// </summary>
        /// <param name="propertyName"></param>
        private void Changed(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Declare PropertyChanged Event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
    }

}
