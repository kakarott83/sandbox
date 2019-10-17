using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class StateControl
    {
       private string _ProcessZeit;
       private double ini = 0;
       private  double end = 0;
       private string _StatusCode;


        /// <summary>
        /// 
        /// </summary>
        public void setIni()
        {
            ini = DateTime.Now.TimeOfDay.TotalMilliseconds;
            _ProcessZeit = "";
            setStatusCode("Neuer Prozess gestartet..." + _ProcessZeit);
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void setEnd()
        {
            end = DateTime.Now.TimeOfDay.TotalMilliseconds;
             CalculateProcessZeit();
             setStatusCode("Prozess abgeschlossen     " + _ProcessZeit);
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void setError()
        {

            setStatusCode("Prozess nicht erfolgreich abgeschlossen" + _ProcessZeit);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateProcessZeit()
        {  double zeit = end-ini;
           _ProcessZeit = zeit.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProcessZeit
        { get { return _ProcessZeit; }
            set
            {
               
            }
         
        }

        /// <summary>
        /// 
        /// </summary>
        public string StatusCode
        { get { return _StatusCode; }
            set
            {
               
            }
         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void setStatusCode(string s)
        {
            _StatusCode=s;

        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        private void Changed(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
    }

}
