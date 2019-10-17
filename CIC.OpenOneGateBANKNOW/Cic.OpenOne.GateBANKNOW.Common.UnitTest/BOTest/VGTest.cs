using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using NUnit.Framework;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Summary description for VGTest
    /// </summary>
    [TestFixture()]
    public class VGTest
    {
        /// <summary>
        /// VGTest Constructor
        /// </summary>
        public VGTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// testVG
        ///</summary>
        [Test]
        public void testVG()
        {

            VGDao vgold = new VGDao();
            ValueGroupDao vgnew = new ValueGroupDao();
            _log.Debug("Start TEST");
            List<long> vgs = null;
            using (PrismaExtended ctx = new PrismaExtended())
            {
                vgs = ctx.ExecuteStoreQuery<long>("select distinct sysvg from vg").ToList<long>();
            }
            DateTime frueher = new DateTime(2008, 10, 20);
            DateTime jetzt = DateTime.Now;
            DateTime spaeter = new DateTime(2018, 10, 20);
            DateTime[] dates = new DateTime[3];
            dates[0] = frueher;
            dates[1] = jetzt;
            dates[2] = spaeter;

            /*
            double testnv = 0;

            for (int i = 0; i < 20; i++)
            {
                long sysvg = 42;
                String xval = "18";
                String yval = "23000";
                VGInterpolationMode ipol = VGInterpolationMode.LINEAR;
                try
                {
                    testnv = vgnew.getVGValue(sysvg, jetzt, "10", "Prämiensatz", ipol);
                }
                catch (Exception e)
                {
                }
            }*/



            foreach (long sysvg in vgs)
            {
                foreach (DateTime d in dates)
                {

                    try
                    {
                        log("*");
                        log("*");
                        log("*");
                        log("---------------------------------- NEW VG TESTCASE---------------------- SYSVG: " + sysvg + " DATE: " + d + " -----------------------------------------");
                        log("*");
                        log("*");
                        log("*");
                        String xmin = vgold.getVGScaleValues(sysvg, d, VGAxisType.XAXIS)[0];
                        String xminnew = vgnew.getVGScaleValues(sysvg, d, VGAxisType.XAXIS)[0];
                        String ymin = vgold.getVGScaleValues(sysvg, d, VGAxisType.YAXIS)[0];
                        String yminnew = vgnew.getVGScaleValues(sysvg, d, VGAxisType.YAXIS)[0];
                        if (!xmin.Equals(xminnew))
                            log("Scale XMIN differ for " + sysvg + " / " + d + " :" + xmin + " != " + xminnew);
                        if (!xmin.Equals(xminnew))
                            log("Scale YMIN differ for " + sysvg + " / " + d + " :" + ymin + " != " + yminnew);

                        VGBoundaries boundsOld = vgold.getVGBoundaries(sysvg, d);
                        VGBoundaries boundsNew = vgnew.getVGBoundaries(sysvg, d);
                        if (boundsOld.xmin != boundsNew.xmin || boundsOld.ymin != boundsNew.ymin || boundsOld.xmax != boundsNew.xmax || boundsOld.ymax != boundsNew.ymax)
                            log("VGBoundaries differ for " + sysvg + " / " + d);
                        String[] xbounds = new String[6];
                        xbounds[0] = (boundsOld.xmin - 1).ToString();
                        xbounds[1] = (boundsOld.xmin).ToString();
                        xbounds[2] = (boundsOld.xmax).ToString();
                        xbounds[3] = (boundsOld.xmax + 1).ToString();
                        xbounds[4] = (boundsOld.xmin + (boundsOld.xmax - boundsOld.xmin) / 2.0).ToString();
                        xbounds[5] = xmin;
                        String[] ybounds = new String[6];
                        ybounds[0] = (boundsOld.ymin - 1).ToString();
                        ybounds[1] = (boundsOld.ymin).ToString();
                        ybounds[2] = (boundsOld.ymax).ToString();
                        ybounds[3] = (boundsOld.ymax + 1).ToString();
                        ybounds[4] = (boundsOld.ymin + (boundsOld.ymax - boundsOld.ymin) / 2.0).ToString();
                        ybounds[5] = ymin;
                        log("Valid VGBoundaries for " + sysvg + " / " + d + " :" + boundsOld.xmin + "|" + boundsOld.xmax + "|" + boundsOld.ymin + "|" + boundsOld.ymax);
                        for (int i = 0; i < 6; i++)
                        {
                            double ov = 0, nv = 0;
                            try
                            {
                                ov = vgold.getVGValue(sysvg, d, xbounds[i], ybounds[i], VGInterpolationMode.LINEAR, plSQLVersion.V2);
                            }
                            catch (Exception)
                            {
                                log("no old VGValue for " + sysvg + " / " + d + " xval: " + xbounds[i] + " yval: " + ybounds[i]);
                            }
                            try
                            {
                                nv = vgnew.getVGValue(sysvg, d, xbounds[i], ybounds[i], VGInterpolationMode.LINEAR, plSQLVersion.V2);
                                if (ov != nv)
                                    log("VGValue differ for " + sysvg + " / " + d + " xval: " + xbounds[i] + " yval: " + ybounds[i] + " old: " + ov + " new: " + nv);
                                else log("Valid VGValue for " + sysvg + " / " + d + " xval: " + xbounds[i] + " yval: " + ybounds[i] + " :" + ov);
                            }
                            catch (Exception)
                            {
                                log("no new VGValue for " + sysvg + " / " + d + " xval: " + xbounds[i] + " yval: " + ybounds[i]);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        log("no VGBoundaries for " + sysvg + " / " + d);
                    }
                }
            }
        }
        private static void log(String message)
        {

            System.Console.WriteLine("Error: " + message);
            _log.Debug(message);
        }
    }
}