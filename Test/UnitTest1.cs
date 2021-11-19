using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GRT;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IntCycleTest()
        {
            var i = new Int32Cycle(3, 3, 3);

            Assert.AreEqual(i, 3);

            i.Step();
            Assert.AreEqual(i, 6);

            Assert.AreEqual(9, i.Previous(2));
            Assert.AreEqual(3, i.Following(2));

            i.Step(3);
            Assert.AreEqual(i, 6);

            i.Step();
            Assert.AreEqual(i, 9);
            i.Step();
            Assert.AreEqual(i, 3);
            i.Step();
            Assert.AreEqual(i, 6);

            i.InvertStep(3);
            Assert.AreEqual(i, 6);

            i.InvertStep();
            Assert.AreEqual(i, 3);
            i.InvertStep();
            Assert.AreEqual(i, 9);
            i.InvertStep();
            Assert.AreEqual(i, 6);
        }
    }
}
