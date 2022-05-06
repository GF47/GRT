using GRT.FSM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class FSM
    {
        private const int AID = 0;
        private const int BID = 1;
        private const int CID = 2;
        private const int DID = 3;

        private const string A = "A";
        private const string B = "B";
        private const string C = "C";
        private const string D = "D";

        private const string TRIGGER_AB = "A to B";
        private const string TRIGGER_BC = "B to C";
        private const string TRIGGER_BD = "B to D";

        private static bool _condition;

        private static bool GetCondition() => _condition;

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

            a.AddTransition(a2b);
            b.AddTransition(b2c);
            b.AddTransition(b2d);
            c.AddTransition(c2a);
            d.AddTransition(d2a);

            fsm.Add(a);
            fsm.Add(b);
            fsm.Add(c);
            fsm.Add(d);

            fsm.EntryStateID = a.ID;
            fsm.Start();

            /**************************************************************/

            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentState);

            fsm.Trigger(TRIGGER_AB);
            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentState);

            _condition = true;
            fsm.Trigger(TRIGGER_AB);
            fsm.Update();
            Assert.AreEqual(BID, fsm.CurrentState);

            /*/// hard code switch A
            fsm.Trigger(TRIGGER_BC);
            fsm.Update();
            Assert.AreEqual(CID, fsm.CurrentState);

            // _condition = false;
            // fsm.Update();
            // Assert.AreEqual(CID, fsm.CurrentState);

            // _condition = true;
            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentState);

            /*/// hard code switch B
            fsm.Trigger(TRIGGER_BD);
            fsm.Update();
            Assert.AreEqual(DID, fsm.CurrentState);

            fsm.Trigger(DID);
            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentState);

            //*/// hard code switch end
            fsm.Update();
            fsm.Update();
            fsm.Update();
            fsm.Update();
            fsm.Update();
            Assert.AreEqual(AID, fsm.CurrentState);
        }
    }
}