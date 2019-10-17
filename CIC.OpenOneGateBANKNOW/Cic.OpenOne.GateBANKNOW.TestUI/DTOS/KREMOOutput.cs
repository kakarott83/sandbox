using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DTOS
{
    class KREMOOutput
    {
        private double _Grundbetrag;
        private double _Berechkrankenkasse;
        private double _Sozialausl1;

        private double _Sozialausl2;

        private double _Steuern;

        private double _Steuern2;

        private double _ReturnCode;

        private string _Version;

        private double _Kind1;

        private double _Kind2;

        private double _Kind3;

      




        public KREMOOutput()
        {
        }



        public double Grundbetrag
        {

            get { return _Grundbetrag; }
            set
            {
                _Grundbetrag = value;
                Changed("Grundbetrag");
            }
        }

        public double Berechkrankenkasse
        {
            get { return _Berechkrankenkasse; }
            set
            {
                _Berechkrankenkasse = value;
                Changed("Berechkrankenkasse");
            }
        }

        public double Sozialausl1
        {
            get { return _Sozialausl1; }
            set
            {
                _Sozialausl1 = value;
                Changed("Sozialausl1");
            }
        }
        public double Sozialausl12
        {
            get { return _Sozialausl2; }
            set
            {
                _Sozialausl2 = value;
                Changed("Sozialausl2 ");
            }
        }
        public double Steuern
        {
            get { return _Steuern; }
            set
            {
                _Steuern = value;
                Changed("Steuern");
            }
        }
        public double Steuern2
        {
            get { return _Steuern2; }
            set
            {
                _Steuern2 = value;
                Changed("Steuern2");
            }
        }

        public double ReturnCode
        {
            get { return _ReturnCode; }
            set
            {
                _ReturnCode = value;
                Changed("ReturnCode");
            }
        }

       
        public double Kind1
        {
            get { return _Kind1; }
            set
            {
                _Kind1 = value;
                Changed("Kind1");
            }
        }
       
        public double Kind2 
        {
            get { return _Kind2; }
            set
            {
                _Kind2 = value;
                Changed("Kind2");
            }
        }


        
        public double Kind3
        {
            get { return _Kind3; }
            set
            {
                _Kind3 = value;
                Changed("Kind3");
            }
        }






        public string Version
        {
            get { return _Version; }
            set
            {
                _Version = value;
                Changed("Version");
            }
        }

      


        private void Changed(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
