using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service.Services.DdOl.DTO
{
    /// <summary>
    /// wird für Testreports verwendet
    /// </summary>
    public class TestDTO : Cic.OpenOne.Common.DTO.oBaseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Wohnort
        /// </summary>
        public string Wohnort { get; set; }
        /// <summary>
        /// PLZ
        /// </summary>
        public int PLZ { get; set; }

        public bool IsGood { get; set; }

        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string F { get; set; }
        public string G { get; set; }
        public string H { get; set; }
        public string I { get; set; }
        public string J { get; set; }
        public string K { get; set; }
        public string L { get; set; }
        public string M { get; set; }
        public string N { get; set; }
        public string O { get; set; }
        public string P { get; set; }
        public string Q { get; set; }
        public string R { get; set; }
        public string S { get; set; }
        public string T { get; set; }
        public string U { get; set; }
        public string V { get; set; }
        public string W { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }

        public Adresse[] Adressen { get; set; }
        public int[] BigArray { get; set; }

        public class Adresse
        {
            public string[] Names { get; set; }
            public bool Show { get; set; }
            public bool Show2 { get; set; }
        }

        public static TestDTO getTestObject(int numb)
        {
            int[] array = new int[100];
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                array[i] = rand.Next(0, 100000);
            }

                return new TestDTO()
                {
                    BigArray = array,
                    Adressen = new Adresse[] { 
                    new Adresse { Names = new string[]{"Adresse_0.1","Adresse_0.2","Adresse_0.3"}, Show = true, Show2 = true },
                    new Adresse { Names = new string[]{"Adresse_1.1","Adresse_1.2","Adresse_1.3"}, Show = false, Show2 = true }, 
                    new Adresse { Names = new string[]{"Adresse_2.1","Adresse_2.2","Adresse_2.3"}, Show = true, Show2 = false } },
                    A = "A_" + numb,
                    B = "B_" + numb,
                    C = "C_" + numb,
                    D = "D_" + numb,
                    E = "E_" + numb,
                    F = "F_" + numb,
                    G = "G_" + numb,
                    H = "H_" + numb,
                    I = "I_" + numb,
                    J = "J_" + numb,
                    K = "K_" + numb,
                    L = "L_" + numb,
                    M = "M_" + numb,
                    N = "N_" + numb,
                    O = "O_" + numb,
                    P = "P_" + numb,
                    Q = "Q_" + numb,
                    R = "R_" + numb,
                    S = "S_" + numb,
                    T = "T_" + numb,
                    U = "U_" + numb,
                    V = "V_" + numb,
                    W = "W_" + numb,
                    X = "X_" + numb,
                    Y = "Y_" + numb,
                    Z = "Z_" + numb,
                    Id = numb,
                    PLZ = numb,
                    Name = "Philipp Mager",
                    Wohnort = "Am Büchl 39",
                    IsGood = false
                };
        }
    }
}