using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GRT.FSM;

namespace Test
{
    [TestClass]
    public class FSM
    {
        const int AID = 0;
        const int BID = 1;
        const int CID = 2;
        const int DID = 3;

        const string A = "A";
        const string B = "B";
        const string C = "C";
        const string D = "D";

        const string TRIGGER_AB = "A to B";
        const string TRIGGER_BC = "B to C";
        const string TRIGGER_BD = "B to D";

        static bool _condition;
        static bool GetCondition() => _condition;

        [TestMethod]
        public void TestFSM()
        {
            var fsm = new FiniteStateMachine();

            var a = new NormalState(AID);
            var b = new NormalState(BID);
            var c = new NormalState(CID);
            var d = new NormalState(DID);

            var abCondition = new StringTriggerCondition(TRIGGER_AB);
            var abCondition2 = new DelegateCondition(GetCondition);
            var bcCondition = new StringTriggerCondition(TRIGGER_BC);
            var bdCondition = new TriggerCondition<string>(TRIGGER_BD);
            var caCondition = new DelegateCondition(GetCondition);
            var daCondition = new TriggerCondition<int>(DID);

            var a2b = new Transition(b.ID, new ICondition[] { abCondition, abCondition2 });
            var b2c = new Transition(c.ID, new ICondition[] { bcCondition });
            var b2d = new Transition(d.ID, new ICondition[] { bdCondition });
            var c2a = new Transition(a.ID, new ICondition[] { caCondition });
            var d2a = new Transition(a.ID, new ICondition[] { daCondition });

            a.AddNext(a2b);
            b.AddNext(b2c);
            b.AddNext(b2d);
            c.AddNext(c2a);
            d.AddNext(d2a);

            fsm.Add(a);
            fsm.Add(b);
            fsm.Add(c);
            fsm.Add(d);

            fsm.StartWith(a.ID);

            /**************************************************************/

            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentID);

            fsm.Trigger(TRIGGER_AB);
            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentID);

            _condition = true;
            fsm.Trigger(TRIGGER_AB);
            fsm.Update();
            Assert.AreEqual(BID, fsm.CurrentID);

            /*/// hard code switch A
            fsm.Trigger(TRIGGER_BC);
            fsm.Update();
            Assert.AreEqual(CID, fsm.CurrentID);

            // _condition = false;
            // fsm.Update();
            // Assert.AreEqual(CID, fsm.CurrentID);

            // _condition = true;
            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentID);

            /*/// hard code switch B
            fsm.Trigger(TRIGGER_BD);
            fsm.Update();
            Assert.AreEqual(DID, fsm.CurrentID);

            fsm.Trigger(DID);
            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentID);

            //*/// hard code switch end
            fsm.Update();
            fsm.Update();
            fsm.Update();
            fsm.Update();
            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentID);
        }
    }
}
