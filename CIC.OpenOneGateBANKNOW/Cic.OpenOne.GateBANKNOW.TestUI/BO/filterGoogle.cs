using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.TestUI.DTOS;


namespace Cic.OpenOne.GateBANKNOW.TestUI.BO
{
    class filterGoogle
    {
        string Fields = "";
        string[] Fieldlist = null;
        string befstring = "";
        string aftstring = "";
        bool replace = false;
        

        public void setReplace(bool replace)
        {
            if (replace)
            {
                befstring = "REPLACE(REPLACE(";
                aftstring = ",' ',''),'-','')";
            }
            this.replace = replace;
        }

        public filterGoogle()
        {



        }

        public string getQuery(TestDto input)
        {

            this.Fields = input.Fields;
            if (Fields == null)
            {
                Fieldlist = new string[0];
               
            }

            char[] separ = {','};
            Fieldlist = Fields.Split(separ);
            string[] sp;
            int[] styp = null;
            int scount = 0;
            string[] st = input.searchstring.Split(' ');
            
            scount = st.Length;
            sp = new string[scount * 2];
            styp = new int[scount * 2];
            int pos = 0;
            while (pos < st.Length)
            {
                sp[pos] = st[pos];
                if (sp[pos].IndexOf("-") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 1;
                }
                if (sp[pos].IndexOf("+") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 0;
                }
                if (sp[pos].IndexOf("=") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 3;
                }
                if (sp[pos].IndexOf("~") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 2;
                }
                if (sp[pos].IndexOf("\"") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    while (pos + 1 < st.Length && sp[pos].IndexOf("\"") != sp[pos].Length - 1)
                    {
                        sp[pos] = sp[pos] + " " + st[pos + 1];
                    }
                    sp[pos] = sp[pos].Substring(0, sp[pos].Length - 1);
                }
                pos++;

            }

            scount = pos;
            string querystring = "1=1";

            for (pos = 0; pos < scount; pos++)
            {
                if (styp[pos] == 3)
                {
                    querystring = querystring + " AND ( ";

                    for (int i = 0; i < Fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring = querystring + " OR ";
                        }
                        querystring = querystring + "UPPER(" + befstring + Fieldlist[i] + aftstring + ") = UPPER('" + toDB(sp[pos]) + "')";
                    }
                    querystring = querystring + " )";
                }
                else if (styp[pos] == 2)
                {
                    querystring = querystring + " AND ( ";

                    for (int i = 0; i < Fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring = querystring + " OR ";
                        }
                        querystring = querystring + "SOUNDEX(" + befstring + Fieldlist[i] + aftstring + ")" + " = SOUNDEX('" + toDB(sp[pos]) + "') OR SOUNDEX(" + befstring + Fieldlist[i] + aftstring + ") = SOUNDEX('" + toDB(sp[pos]) + "')";
                    }
                    querystring = querystring + " )";
                }
                else if (styp[pos] == 1)
                {
                    querystring = querystring + " AND NOT( ";

                    for (int i = 0; i < Fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring = querystring + " OR ";
                        }
                        querystring = querystring + "IFNULL(UPPER(" + befstring + Fieldlist[i] + aftstring + ") LIKE UPPER('%" + toDB(sp[pos]) + "%'),0)";
                    }
                    querystring = querystring + " )";

                }
                else
                {
                    querystring = querystring + " AND ( ";

                    for (int i = 0; i < Fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring = querystring + " OR ";
                        }
                        querystring = querystring + "UPPER(" + befstring + Fieldlist[i] + aftstring + ") LIKE UPPER('%" + toDB(sp[pos]) + "%')";
                     /*   try
                        {
                            string dat = de.sirconic.util.ParseDate.getDatum(sp[pos]);
                            if (!sp[pos].equals(dat)) {
                            querystring = querystring + " OR ";
                            querystring = querystring + "UPPER(" + befstring + Fieldlist[i] + aftstring + ") LIKE UPPER('%" + toDB(sp[pos]) + "%')";

                             }
                        }
                        catch (Exception Ex)
                        {
                        }*/
                    }
                    querystring = querystring + " )";

                }
            }



            return querystring;
        }

        private string toDBQuote(string text)
        {
            if (text == null)
            {
                return "''";
            }
            return "'" + toDB(text) + "'";
        }

        private string toDB(string text)
        {
            if (text == null)
            {
                return "";
            }
            System.Text.StringBuilder sbuf = new System.Text.StringBuilder(text);
            for (int i = 0; i < sbuf.Length; i++)
            {
                if (sbuf.ToString().ElementAt(i) == '\'' || sbuf.ToString().ElementAt(i) == '\\')
                {
                    sbuf.Insert(i++, '\\');
                }
            }
            text = sbuf.ToString();
            if (replace)
            {
                System.Text.StringBuilder sbuf2 = new System.Text.StringBuilder();
                for (int i = 0; i < sbuf.Length; i++)
                {
                    if (sbuf.ToString().ElementAt(i) != ' ' && sbuf.ToString().ElementAt(i) != '-')
                    {
                        sbuf2.Append(sbuf.ToString().ElementAt(i));
                    }
                }
                text = sbuf2.ToString();
            }
            return "" + text + "";
        }
    }
}