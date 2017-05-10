using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using work_log;
using System.Collections.Generic;

namespace LogTest {
    [TestClass]
    public class CLogTest {
        [TestMethod]
        public void TestRoundByVal() {

            List<Tuple<int, int>> expected = new List<Tuple<int, int>>();
            expected.Add(new Tuple<int, int>(0, 0));
            expected.Add(new Tuple<int, int>(0, 3));
            expected.Add(new Tuple<int, int>(6, 4));
            expected.Add(new Tuple<int, int>(6, 6));
            expected.Add(new Tuple<int, int>(6, 8));
            expected.Add(new Tuple<int, int>(12, 10));
            expected.Add(new Tuple<int, int>(12, 13));
            expected.Add(new Tuple<int, int>(18, 16));
            expected.Add(new Tuple<int, int>(18, 20));
            expected.Add(new Tuple<int, int>(24, 22));
            expected.Add(new Tuple<int, int>(30, 30));
            //expected.Add(new Tuple<int, int>(30, 37));
            //expected.Add(new Tuple<int, int>(45, 38));
            //expected.Add(new Tuple<int, int>(45, 39));
            //expected.Add(new Tuple<int, int>(45, 42));
            //expected.Add(new Tuple<int, int>(45, 44));
            //expected.Add(new Tuple<int, int>(45, 45));
            //expected.Add(new Tuple<int, int>(45, 50));
            //expected.Add(new Tuple<int, int>(45, 52));
            //expected.Add(new Tuple<int, int>(60, 53));
            //expected.Add(new Tuple<int, int>(60, 55));
            //expected.Add(new Tuple<int, int>(60, 59));

            CLogFile logFile = new CLogFile();

            foreach(Tuple<int, int> item in expected) {
                Assert.AreEqual(item.Item1, logFile.RoundByValue(item.Item2, CLogFile.k_interval), "" + item.Item2 + " gave " + logFile.RoundByValue(item.Item2, CLogFile.k_interval) + ". Expected " + item.Item1);
            }
        }

        [TestMethod]
        public void TestGenTimeString() {
            List<Tuple<DateTime, String>> expected = new List<Tuple<DateTime, String>>();
            expected.Add(new Tuple<DateTime, String>(new DateTime(2017, 4, 10, 9, 23, 0), "9:24 AM"));
            expected.Add(new Tuple<DateTime, String>(new DateTime(2017, 4, 10, 9, 45, 0), "9:42 AM"));
            expected.Add(new Tuple<DateTime, String>(new DateTime(2017, 4, 10, 9, 55, 0), "9:54 AM"));
            expected.Add(new Tuple<DateTime, String>(new DateTime(2017, 4, 10, 9, 12, 0), "9:12 AM"));
            expected.Add(new Tuple<DateTime, String>(new DateTime(2017, 4, 10, 9, 17, 0), "9:18 AM"));
            expected.Add(new Tuple<DateTime, String>(new DateTime(2017, 4, 10, 11, 58, 0), "12:00 PM"));

            CLogFile logFile = new CLogFile();

            foreach (Tuple<DateTime, String> item in expected) {
                String str = logFile.GenTimeString(item.Item1);
                Assert.AreEqual(item.Item2, str, item.Item1.ToString("h:mm tt") + " generated " + str + " expected " + item.Item2);
            }
        }
    }
}
