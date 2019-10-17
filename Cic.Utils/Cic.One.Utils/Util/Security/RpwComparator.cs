using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Globalization;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.Common.Util.Security
{
    /// <summary>
    /// OpenLease Password Support
    /// </summary>
    public class RpwComparator
    {


        #region Private constants
        private const int ConstMaxCoded = 40;       // In Clarion: cMaxCoded
        private const int ConstMaxPassword = 20;    // In Clarion: cMaxPassword
        private const int ConstXTrans = 4;          // In Clarion: cXTrans
        private const int ConstYTrans = 10;         // In Clarion: cYTrans
        private const int ConstXRange = 9;          // In Clarion: cXRange
        private const int ConstYRange = 9;          // In Clarion: cYRange
        #endregion

        #region Private variables
        private static char[,] _AlphaTable = new char[ConstXRange, ConstYRange];    // In Clarion: _aAt
        private static int[] _DimTable = new int[ConstYRange];                      // In Clarion: _aDt
        private static int[,] _TransformTable = new int[ConstXTrans, ConstYTrans];  // In Clarion: _aTt
        private static int[,] _CodeTable = new int[ConstXTrans, 2];                 // In Clarion: _aCt
        #endregion

        #region Methods
        public static string Encode(string uncoded)
        {
            if (uncoded.Length > ConstMaxPassword)
            {
                throw new ArgumentException("Given text is longer than maximum allowed");
            }

            MyInitialize();

            string Coded = string.Empty;

            for (int I = 0; I < uncoded.Trim().Length; I++)
            {
                string Double = MySearchChar(uncoded[I]);
                if (!string.IsNullOrEmpty(Double))
                {
                    Coded = Coded.Trim() + Double;
                }
                else
                {
                    Coded = string.Empty;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(Coded))
            {
                Coded = MyTransformAlpha(Coded);
            }

            return Coded;
        }

        public static string Decode(string coded)
        {
            if (coded.Length > ConstMaxCoded)
            {
                throw new ArgumentException("Encoded text is longer than maximum allowed");
            }

            MyInitialize();

            string Coded = string.Empty;
            string CodedTemp = string.Empty;

            CodedTemp = MyRetransformAlpha(coded);

            for (int I = 0; I < CodedTemp.Trim().Length; I += 2)
            {
                int Value = MySearchCode(CodedTemp.Substring(I, 2));
                if (Value > 0)
                {
                    Coded += Convert.ToChar(Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    Coded = string.Empty;
                    break;
                }
            }

            return Coded;
        }
        #endregion

        #region My methods
        private static void MyInitialize()
        {
            // Fill alpha table
            _AlphaTable[0, 0] = '1';
            _AlphaTable[0, 1] = 'D';
            _AlphaTable[0, 2] = 'E';
            _AlphaTable[0, 3] = 'h';
            _AlphaTable[0, 4] = 'M';
            _AlphaTable[0, 5] = 's';
            _AlphaTable[0, 6] = 'V';
            _AlphaTable[0, 7] = 'N';
            _AlphaTable[0, 8] = 'ä';

            _AlphaTable[1, 0] = 'a';
            _AlphaTable[1, 1] = 'k';
            _AlphaTable[1, 2] = 'y';
            _AlphaTable[1, 3] = '5';
            _AlphaTable[1, 4] = 'n';
            _AlphaTable[1, 5] = 'x';
            _AlphaTable[1, 6] = 'e';
            _AlphaTable[1, 7] = '6';
            _AlphaTable[1, 8] = 'ö';

            _AlphaTable[2, 0] = 'A';
            _AlphaTable[2, 1] = 'Y';
            _AlphaTable[2, 2] = '2';
            _AlphaTable[2, 3] = 'B';
            _AlphaTable[2, 4] = 'b';
            _AlphaTable[2, 5] = 'R';
            _AlphaTable[2, 6] = 'U';
            _AlphaTable[2, 7] = '_';
            _AlphaTable[2, 8] = 'ß';

            _AlphaTable[3, 0] = 'q';
            _AlphaTable[3, 1] = 'j';
            _AlphaTable[3, 2] = 'z';
            _AlphaTable[3, 3] = 'o';
            _AlphaTable[3, 4] = 'd';
            _AlphaTable[3, 5] = 'F';
            _AlphaTable[3, 6] = '9';
            _AlphaTable[3, 7] = 'g';
            _AlphaTable[3, 8] = '/';

            _AlphaTable[4, 0] = 'c';
            _AlphaTable[4, 1] = 'C';
            _AlphaTable[4, 2] = 'p';
            _AlphaTable[4, 3] = 'i';
            _AlphaTable[4, 4] = 'G';
            _AlphaTable[4, 5] = 'v';
            _AlphaTable[4, 6] = 'Q';
            _AlphaTable[4, 7] = 'O';
            _AlphaTable[4, 8] = '.';

            _AlphaTable[5, 0] = 'J';
            _AlphaTable[5, 1] = '3';
            _AlphaTable[5, 2] = 'L';
            _AlphaTable[5, 3] = '4';
            _AlphaTable[5, 4] = 'u';
            _AlphaTable[5, 5] = 'X';
            _AlphaTable[5, 6] = 'f';
            _AlphaTable[5, 7] = '*';
            _AlphaTable[5, 8] = ',';

            _AlphaTable[6, 0] = 'r';
            _AlphaTable[6, 1] = 'm';
            _AlphaTable[6, 2] = '0';
            _AlphaTable[6, 3] = 'l';
            _AlphaTable[6, 4] = 'H';
            _AlphaTable[6, 5] = 'w';
            _AlphaTable[6, 6] = 'P';
            _AlphaTable[6, 7] = '7';
            _AlphaTable[6, 8] = '-';

            _AlphaTable[7, 0] = 'K';
            _AlphaTable[7, 1] = 'S';
            _AlphaTable[7, 2] = 'I';
            _AlphaTable[7, 3] = 't';
            _AlphaTable[7, 4] = 'W';
            _AlphaTable[7, 5] = '8';
            _AlphaTable[7, 6] = 'Z';
            _AlphaTable[7, 7] = 'T';
            _AlphaTable[7, 8] = '&';

            _AlphaTable[8, 0] = '"';
            _AlphaTable[8, 1] = 'Ü';
            _AlphaTable[8, 2] = 'Ö';
            _AlphaTable[8, 3] = 'Ä';
            _AlphaTable[8, 4] = 'ü';
            _AlphaTable[8, 5] = '!';
            _AlphaTable[8, 6] = ' ';
            _AlphaTable[8, 7] = '+';
            _AlphaTable[8, 8] = '@';

            // Fill dim table
            _DimTable[0] = 'C';
            _DimTable[1] = 'E';
            _DimTable[2] = 'G';
            _DimTable[3] = 'I';
            _DimTable[4] = 'L';
            _DimTable[5] = 'S';
            _DimTable[6] = 'Y';
            _DimTable[7] = 'Z';
            _DimTable[8] = 'A';

            // Fill transform table
            for (int X = 0; X < ConstXTrans; X++)
            {
                for (int Y = 0; Y < ConstYTrans; Y++)
                {
                    _TransformTable[X, Y] = '°';
                }
            }

            // Fill code table
            _CodeTable[0, 0] = 0;   // Value decremented by 1 because it is used as index to array
            _CodeTable[1, 0] = 1;   // -||-
            _CodeTable[2, 0] = 2;   // -||-
            _CodeTable[3, 0] = 3;   // -||-
            _CodeTable[0, 1] = 'S';
            _CodeTable[1, 1] = 'K';
            _CodeTable[2, 1] = 'Y';
            _CodeTable[3, 1] = 'E';
        }

        public static string MySearchChar(char character)
        {
            string Result = string.Empty;
            bool Done = false;

            for (int X = 0; X < ConstXRange; X++)
            {
                for (int Y = 0; Y < ConstYRange; Y++)
                {
                    if (_AlphaTable[X, Y] == character)
                    {

                        Result = string.Concat(Convert.ToChar(_DimTable[Y], CultureInfo.InvariantCulture), Convert.ToChar(_DimTable[X], CultureInfo.InvariantCulture));
                        Done = true;
                        break;
                    }
                }

                if (Done)
                {
                    break;
                }
            }

            return Result;
        }

        private static int MySearchCode(string value)
        {
            int X = -1; // Set to -1 because of 0-based array indexing
            int Y = -1; // - || -
            int Result = 0;

            for (int I = 0; I < ConstYRange; I++)
            {
                if (Convert.ToChar(_DimTable[I], CultureInfo.InvariantCulture) == value[0])
                {
                    Y = I;
                    break;
                }
            }

            for (int I = 0; I < ConstYRange; I++) // NOTE: Propably should be ConstXRange here
            {
                if (Convert.ToChar(_DimTable[I], CultureInfo.InvariantCulture) == value[1])
                {
                    X = I;
                    break;
                }
            }

            if (X >= 0 && Y >= 0)
            {
                Result = _AlphaTable[X, Y];
            }

            return Result;
        }

        private static string MyTransformAlpha(string coded)
        {
            int CodedLength = coded.Trim().Length;
            string Coded = string.Empty;

            int I = 0;
            bool Done = false;
            for (int Y = 0; Y < ConstYTrans; Y++)
            {
                for (int X = 0; X < ConstXTrans; X++)
                {
                    _TransformTable[X, Y] = coded[I];

                    I += 1;
                    if (I > CodedLength - 1)
                    {
                        Done = true;
                        break;
                    }
                }

                if (Done)
                {
                    break;
                }
            }

            List<CharIndexPair> PairList = new List<CharIndexPair>();
            for (int J = 0; J < ConstXTrans; J++)
            {

                char FirstChar = Convert.ToChar(_CodeTable[J, 1], CultureInfo.InvariantCulture);  //_CodeTable[J, 1].ToString()[0];
                PairList.Add(new CharIndexPair(FirstChar, _CodeTable[J, 0]));
            }

            PairList.Sort(CharIndexPair.SortAscending());

            if (((CodedLength / 2) % 2) == 1)
            {
                CodedLength += 2;
            }

            I = 0;
            Done = false;
            string CodedTemp = string.Empty;

            for (int Y = 0; Y < ConstYTrans; Y++)
            {
                foreach (var PairListLoop in PairList)
                {
                    I += 1;
                    if (I > CodedLength)
                    {
                        Done = true;
                        break;
                    }

                    CodedTemp += Convert.ToChar(_TransformTable[PairListLoop.Index, Y], CultureInfo.InvariantCulture);
                }

                if (Done)
                {
                    break;
                }
            }

            CodedTemp = CodedTemp.Replace("°", string.Empty);
            CodedLength = CodedTemp.Trim().Length;
            PairList = new List<CharIndexPair>();

            for (int J = 1; J <= CodedLength; J++)
            {
                int Y = (J / 9) + 1;
                int X = (J % 8);

                if (X == 0)
                {
                    X = 8;
                }

                X--;
                Y--;

                PairList.Add(new CharIndexPair(_AlphaTable[X, Y], J - 1));
            }

            PairList.Sort(CharIndexPair.SortDescending());

            foreach (var PairListLoop in PairList)
            {
                Coded += CodedTemp[PairListLoop.Index];
            }

            return Coded;
        }

        private static string MyRetransformAlpha(string coded)
        {
            int CodedLength = coded.Trim().Length;
            string Coded = string.Empty;

            List<CharIndexPair> PairList = new List<CharIndexPair>();

            for (int J = 1; J <= CodedLength; J++)
            {
                int Y = (J / 9) + 1;
                int X = (J % 8);

                if (X == 0)
                {
                    X = 8;
                }

                X--;
                Y--;

                PairList.Add(new CharIndexPair(_AlphaTable[X, Y], J - 1));
            }

            PairList.Sort(CharIndexPair.SortDescending());

            int K = 0;
            char[] CodedArray = new char[CodedLength];
            foreach (var PairListLoop in PairList)
            {
                CodedArray[PairListLoop.Index] = coded[K];
                K++;
            }

            Coded = new string(CodedArray);

            PairList = new List<CharIndexPair>();
            for (int J = 1; J <= CodedLength; J++)
            {
                int Y = (J / 9) + 1;
                int X = (J % 8);

                if (X == 0)
                {
                    X = 8;
                }

                X--;
                Y--;

                PairList.Add(new CharIndexPair(_AlphaTable[X, Y], J - 1));
            }

            string CodedTemp = string.Empty;
            foreach (var PairListLoop in PairList)
            {
                CodedTemp += Coded[PairListLoop.Index];
            }

            PairList = new List<CharIndexPair>();
            for (int J = 0; J < ConstXTrans; J++)
            {
                char FirstChar = _CodeTable[J, 1].ToString()[0];
                PairList.Add(new CharIndexPair(FirstChar, _CodeTable[J, 0]));
            }

            PairList.Sort(CharIndexPair.SortAscending());

            bool Done = false;
            int I = 0;
            for (int Y = 0; Y < ConstYTrans; Y++)
            {
                foreach (var PairListLoop in PairList)
                {
                    _TransformTable[PairListLoop.Index, Y] = Convert.ToInt32(CodedTemp[I], CultureInfo.InvariantCulture);

                    I += 1;
                    if (I > CodedLength - 1)
                    {
                        Done = true;
                        break;
                    }
                }

                if (Done)
                {
                    break;
                }
            }

            PairList = new List<CharIndexPair>();
            for (int J = 0; J < ConstXTrans; J++)
            {
                char FirstChar = _CodeTable[J, 1].ToString()[0];
                PairList.Add(new CharIndexPair(FirstChar, _CodeTable[J, 0]));
            }

            if (((CodedLength / 2) % 2) == 1)
            {
                CodedLength += 2;
            }

            I = 0;
            Done = false;
            Coded = string.Empty; // Has to be cleared

            for (int Y = 0; Y < ConstYTrans; Y++)
            {
                foreach (var PairListLoop in PairList)
                {
                    I += 1;
                    if (I > CodedLength)
                    {
                        Done = true;
                        break;
                    }

                    Coded += Convert.ToChar(_TransformTable[PairListLoop.Index, Y], CultureInfo.InvariantCulture);
                }

                if (Done)
                {
                    break;
                }
            }

            Coded = Coded.Replace("°", string.Empty);
            return Coded;
        }
        #endregion
    }


}


public class CharIndexPair : IComparable
{
    
    #region Constructors
    public CharIndexPair(char character, int index)
    {
        Character = character;
        Index = index;
    }
    #endregion

    #region Properties
    public char Character
    {
        get;
        set;
    }

    public int Index
    {
        get;
        set;
    }
    #endregion

    #region IComparer methods
    public int CompareTo(object other)
    {
        CharIndexPair OtherPair = other as CharIndexPair;
        return Character.CompareTo(OtherPair.Character);
    }
    #endregion

    #region Methods
    public static IComparer<CharIndexPair> SortDescending()
    {
        return new SortDescendingHelper();
    }

    public static IComparer<CharIndexPair> SortAscending()
    {
        return new SortAscendingHelper();
    }
    #endregion

    private class SortDescendingHelper : IComparer<CharIndexPair>
    {
        public int Compare(CharIndexPair pairThis, CharIndexPair pairOther)
        {
            
            
            
            if (pairThis.Character < pairOther.Character)
            {
                
                return 1;
            }

            if (pairThis.Character > pairOther.Character)
            {
                
                return -1;
            }
            if (pairThis.Index < pairOther.Index)
                return -1;
            if (pairThis.Index > pairOther.Index)
                return 1;
            
            return 0;
        }
    }

    private class SortAscendingHelper : IComparer<CharIndexPair>
    {
        public int Compare(CharIndexPair pairThis, CharIndexPair pairOther)
        {
            if (pairThis.Character < pairOther.Character)
            {
                
                return -1;
            }

            if (pairThis.Character > pairOther.Character)
            {
                
                return 1;
            }
            if (pairThis.Index < pairOther.Index)
                return -1;
            if (pairThis.Index > pairOther.Index)
                return 1;
            
            return 0;
        }
    }
}