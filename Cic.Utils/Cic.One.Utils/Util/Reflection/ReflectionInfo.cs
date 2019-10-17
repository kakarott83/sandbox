using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Collections;

namespace Cic.One.Utils.Util.Reflection
{
    /// <summary>
    /// Für den Reflection-Zugriff zuständig
    /// </summary>
    public class ReflectionInfo
    {
        object Data;
        private Dictionary<string, ReflectionInfo> loaded = new Dictionary<string, ReflectionInfo>();
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Type type;
        /// <summary>
        /// Type des zugehörigen Datenobjekts
        /// </summary>
        public Type Type
        {
            get
            {
                if (type == null)
                    type = Data.GetType();
                return type;
            }
            set { type = value; }
        }
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="Data">Datenobjekt</param>
        public ReflectionInfo(object Data)
        {
            this.Data = Data;
        }

        ReflectionInfo parent;
        /// <summary>
        /// Gibt den Parent an, welcher über diesem Objekt steht (ein InfoPfad darüber)
        /// </summary>
        public ReflectionInfo Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// Gibt das Basisobjekt an, zu welchem es gehört
        /// </summary>
        public ReflectionInfo ParentRoot
        {
            get
            {
                if (parent == null)
                    return this;
                else
                    return Parent.ParentRoot;
            }
        }

        public ReflectionInfo(object Data, ReflectionInfo parent)
            : this(Data)
        {
            this.parent = parent;
        }


        /// <summary>
        /// Liefert das Objekt von dem InfoPfad (angewandt auf das Datenobjekt) zurück
        /// </summary>
        /// <param name="InfoPath">InfoPfad, wo sich das Objekt befindet</param>
        /// <returns>Objekt an der stelle vom Infopfad</returns>
        public object getValue(string InfoPath, out bool success)
        {

            // List<string> infos = InfoPath.Split('.').ToList();
            List<string> infos = splitPath(InfoPath);
            return getValue(infos, out success);
        }

        /// <summary>
        /// Splittet einen vollständigen Pfad in mehrere Teilpfade
        /// </summary>
        /// <param name="InfoPath">Vollständige Pfad</param>
        /// <returns></returns>
        private List<string> splitPath(string InfoPath)
        {
            List<string> infos = new List<string>();
            string path = "";
            bool started = false;
            char previous = ' ';
            for (int i = 0; i < InfoPath.Length; i++)
            {
                char c = InfoPath[i];
                if (c == '.' && !started)
                {
                    infos.Add(path);
                    path = "";
                }
                else if (c == '"' && previous == '\\')
                {
                    path = path.Remove(path.Length - 1) + c;
                }
                else if (c == '"')
                {
                    started = !started;
                }
                else
                    path += c;

                previous = c;
            }
            infos.Add(path);
            return infos;
        }

        /// <summary>
        /// Setzt für einen gewissen InfoPfad ein Objekt, damit Reflection so selten wie möglich verwendet wird.
        /// So kann zum Beispiel beim expandieren schon alle Listenelemente gespeichert werden und somit Reflection umgangen werden
        /// </summary>
        /// <param name="InfoPath">Stelle, wo es gespeichert werden soll</param>
        /// <param name="o">Objekt zum speichern</param>
        public void setValue(string InfoPath, object o)
        {
            //List<string> infos = InfoPath.Split('.').ToList();
            List<string> infos = splitPath(InfoPath);
            setValue(infos, o);
        }

        /// <summary>
        /// Setzt für einen gewissen InfoPfad ein Objekt, damit Reflection so selten wie möglich verwendet wird.
        /// So kann zum Beispiel beim expandieren schon alle Listenelemente gespeichert werden und somit Reflection umgangen werden
        /// </summary>
        /// <param name="InfoPath">Stelle, wo es gespeichert werden soll (schon gesplittet durch ".". Die Punkte, welche in Gänsefüßchen sind bleiben vorhanden)</param>
        /// <param name="o">Objekt zum speichern</param>
        private void setValue(List<string> InfoPath, object o)
        {
            string currentCheck = InfoPath[0];
            InfoPath.RemoveAt(0);
            if (InfoPath.Count == 0)
            {
                if (!loaded.ContainsKey(currentCheck))
                    loaded.Add(currentCheck, new ReflectionInfo(o, this));
                return;
            }
            else
            {
                if (!loaded.ContainsKey(currentCheck))
                {
                    bool success;
                    getValue(currentCheck, out success);
                }

                if (loaded[currentCheck] == null)
                {
                    return;
                }
                else
                {
                    loaded[currentCheck].setValue(InfoPath, o);
                    return;
                }
            }
        }

        /// <summary>
        /// Liefert das Objekt an dem Infopfad zurück
        /// </summary>
        /// <param name="infoPath">Pfad, getrennt durch '.'</param>
        /// <param name="success">Gibt an ob der Wert erfolgreich geladen werden konnte</param>
        /// <returns></returns>
        private object getValue(List<string> infoPath, out bool success)
        {
            if (infoPath.Count == 0)
            {
                success = true;
                return Data;
            }

            object nextObject = null;
            string currentCheck = infoPath[0];
            infoPath.RemoveAt(0);

            if (currentCheck == "")
            {
                success = true;
                return Data;
            }

            if (loaded.ContainsKey(currentCheck))
            {
                if (loaded[currentCheck] == null)
                {
                    success = false;
                    _Log.Warn("Could not parse value of " + currentCheck);
                    return "";
                }
                return loaded[currentCheck].getValue(infoPath, out success);
            }

            try
            {
                if (Data is ExpandoObject)
                {
                    nextObject = (Data as ExpandoObject).FirstOrDefault((x) => x.Key == currentCheck).Value;
                }
                else
                    switch (getKind(currentCheck))
                    {
                        case Infotype.Property:
                            nextObject = getProperty(currentCheck);
                            break;
                        case Infotype.Method:
                            nextObject = getMethodResult(currentCheck);
                            break;
                        case Infotype.IEnumerable:
                            nextObject = getIEnumerable(currentCheck);
                            break;
                    }
            }
            catch
            {

            }

            if (nextObject == null)
            {
                //TODO Exception?
                loaded.Add(currentCheck, null);
                success = false;
                _Log.Warn("Could not parse value of " + currentCheck);
                return "";
            }

            ReflectionInfo rbo = new ReflectionInfo(nextObject, this);
            loaded.Add(currentCheck, rbo);
            return rbo.getValue(infoPath, out success);
        }

        /// <summary>
        /// Falls das Objekt einen Index hat, wird diese Methode verwendet zB. {$object.Addressen[1]}
        /// </summary>
        /// <param name="currentCheck"></param>
        /// <returns></returns>
        private object getIEnumerable(string currentCheck)
        {
            string pattern = "(.*?)\\[(\\d{1,10000})\\]";

            Match match = Regex.Match(currentCheck, pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.None);

            string propertyName = match.Groups[1].Value;
            int index = Convert.ToInt32(match.Groups[2].Value);

            object found = null;
            if (propertyName == "")
                found = Data;
            else
                found = getProperty(propertyName);

            if (found is IEnumerable)
            {
                IEnumerable enu = found as IEnumerable;
                int currentIndex = 0;
                foreach (object o in enu)
                {
                    if (currentIndex == index)
                        return o;
                    currentIndex++;
                }

                return null;
            }
            return null;
        }

        /// <summary>
        /// Liefert das Ergebniss einer Methode auf das Datenobjekt zurück. Die Methode darf keine Parameter haben
        /// </summary>
        /// <param name="currentCheck"></param>
        /// <returns></returns>
        private object getMethodResult(string currentCheck)
        {
            if (currentCheck.EndsWith("()"))
            {
                currentCheck = currentCheck.Remove(currentCheck.Length - 2);
                MethodInfo methodinfo = Type.GetMethod(currentCheck, Type.EmptyTypes);
                if (methodinfo != null)
                    return methodinfo.Invoke(Data, null);
            }
            else
            {
                Match method = Regex.Match(currentCheck, "(.*?)\\((.*)\\)");
                object[] parameters = (object[])method.Groups[2].Value.Split(',').Select(obj => obj.Trim()).ToArray();

                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = TryToParseParameter(parameters[i]);
                }

                if (method.Groups[1].Value == "Operator")
                {
                    return Operator(parameters);
                }
                else if (method.Groups[1].Value == "Regex")
                {
                    return UseRegex(parameters);
                }

                Type[] types = new System.Type[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                    types[i] = typeof(string);

                MethodInfo methodinfo = Type.GetMethod(method.Groups[1].Value, types);
                if (methodinfo != null)
                {
                    return methodinfo.Invoke(Data, parameters);
                }
            }
            return null;
        }

        /// <summary>
        /// Versucht einen Parameter auszuwerten. (Falls es sich um ein $object... handelt soll der Wert von dem Objekt geladen werden)
        /// </summary>
        /// <param name="parameter">Parameter, welcher geparst werden soll</param>
        /// <returns></returns>
        private object TryToParseParameter(object parameter)
        {
            string parameterString = parameter.ToString();
            Match m = Regex.Match(parameterString, "$object\\.?(.*?)", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.None);
            if (m != null && m.Success && parameterString == m.Value)
            {
                string infoPath = m.Groups[1].Value;
                ReflectionInfo rinfo = ParentRoot;
                bool success = false;
                object ob = rinfo.getValue(infoPath, out success);
                if (success)
                    return ob;
            }
            return parameter;
        }

        /// <summary>
        /// Erstellt einen Regex anhand den Parametern(0 = Pattern, 1 = gewählte Gruppe)
        /// </summary>
        /// <param name="parameters">Parameter</param>
        /// <returns>gewählte Gruppe von dem Regex</returns>
        private object UseRegex(object[] parameters)
        {
            if (parameters.Length < 2)
                return null;

            string first = Data.ToString();
            string second = parameters[0].ToString();
            int third;
            if (int.TryParse(parameters[1].ToString(), out third))
            {
                Match m = Regex.Match(first, second, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
                if (m != null && m.Success && m.Groups.Count > third)
                {
                    return m.Groups[third].Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Berechnet erweiterte if Operationen: $object....Operator(EQ,KASKO)  NOT, EQ, NE, GT, LT, GE, LE, REGEX (!, ==, !=, >, <, >=, <=, reg) 
        /// </summary>
        /// <param name="parameters">Enthält alle Parameter die der Operator Methode übergeben werden</param>
        /// <returns>Ausgewertetes Ergebnis</returns>
        private bool Operator(object[] parameters, int opCodeIndex = 0)
        {
            bool result = false;

            if (parameters.Length == opCodeIndex)
                return result;

            string op = parameters[opCodeIndex].ToString();
            if (op == "NOT" || op == "!")
            {
                if (Data is bool)
                {
                    result = (!(bool)Data);
                }
                opCodeIndex++;
            }
            else
            {
                if (parameters.Length == opCodeIndex + 1)
                    return false;

                string first = Data.ToString();
                string second = parameters[opCodeIndex + 1].ToString();

                if (op == "EQ" || op == "==")
                {
                    result = Compare(first, second, (a, b) => a == b, (a, b) => a == b);
                    opCodeIndex += 2;
                }
                else if (op == "NE" || op == "!=")
                {
                    result = Compare(first, second, (a, b) => a != b, (a, b) => a != b);
                    opCodeIndex += 2;
                }
                else if (op == "GT" || op == ">")
                {
                    result = Compare(first, second, (a, b) => a > b, (a, b) => a.CompareTo(b) > 0);
                    opCodeIndex += 2;
                }
                else if (op == "LT" || op == "<")
                {
                    result = Compare(first, second, (a, b) => a < b, (a, b) => a.CompareTo(b) < 0);
                    opCodeIndex += 2;
                }
                else if (op == "GE" || op == ">=")
                {
                    result = Compare(first, second, (a, b) => a >= b, (a, b) => a.CompareTo(b) >= 0);
                    opCodeIndex += 2;
                }
                else if (op == "LE" || op == "<=")
                {
                    result = Compare(first, second, (a, b) => a <= b, (a, b) => a.CompareTo(b) <= 0);
                    opCodeIndex += 2;
                }
                else if (op == "REGEX" || op == "reg")
                {
                    Match m = Regex.Match(first, second, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    result = m != null && m.Success;
                    opCodeIndex += 2;
                }
            }
            if (parameters.Length > opCodeIndex)
            {
                string combOpCode = parameters[opCodeIndex].ToString();
                if (combOpCode == "AND" || combOpCode == "&&")
                {
                    return result && Operator(parameters, opCodeIndex + 1);
                }
                else if (combOpCode == "OR" || combOpCode == "||")
                {
                    return result || Operator(parameters, opCodeIndex + 1);
                }
            }
            return result;
        }

        /// <summary>
        /// Vergleicht zwei decimal bzw. zwei strings mit den übergebenen methoden
        /// </summary>
        /// <param name="first">Das Hauptobjekt</param>
        /// <param name="second">Das zweite Objekt</param>
        /// <param name="funcDec">Vergleichsmethode, falls die Objekte decimal sind</param>
        /// <param name="funcStr">Vergleichsmethode, falls sie strings sind </param>
        /// <returns>Ergebnis der ausgeführten Methode</returns>
        private bool Compare(string first, string second, Func<decimal, decimal, bool> funcDec, Func<string, string, bool> funcStr)
        {
            decimal firstDec;
            decimal secondDec;

            if (decimal.TryParse(first, out firstDec) && decimal.TryParse(second, out secondDec))
            {
                return funcDec(firstDec, secondDec);
            }
            else
                return funcStr(first, second);
        }


        /// <summary>
        /// Liefert eine Eigenschaft zurück
        /// </summary>
        /// <param name="currentCheck"></param>
        /// <returns></returns>
        private object getProperty(string currentCheck)
        {
            PropertyInfo property = Type.GetProperty(currentCheck);
            if (property == null)
            {
                FieldInfo fi = Type.GetField(currentCheck);
                if (fi == null)
                    return null;
                else
                    return fi.GetValue(Data);
            }
            else
                return property.GetValue(Data, null);
        }


        enum Infotype
        {
            Method,
            IEnumerable,
            Property
        }

        /// <summary>
        /// Entscheidet, wie ein Ausdruck aufgelöst werden soll
        /// </summary>
        /// <param name="currentCheck"></param>
        /// <returns></returns>
        private Infotype getKind(string currentCheck)
        {
            if (currentCheck.Contains('(') && currentCheck.Contains(')'))
                return Infotype.Method;
            else if (currentCheck.Contains('[') && currentCheck.Contains(']'))
                return Infotype.IEnumerable;
            else
                return Infotype.Property;
        }

    }
}
