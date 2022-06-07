using GRT;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.FSM
{
    public class FSMManager
    {
        #region Singleton

        public static FSMManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FSMManager();
                }
                return _instance;
            }
        }

        private static FSMManager _instance;

        #endregion Singleton

        public IDictionary<int, FiniteStateMachine> Collection { get; }

        public IBlackBoard GlobalVariables { get; }

        private FiniteStateMachine _cache;

        private FSMManager()
        {
            Collection = new Dictionary<int, FiniteStateMachine>();
            GlobalVariables = new BlackBoard();
        }

        public FiniteStateMachine GetFSM(int id)
        {
            if (_cache != null && _cache.ID == id)
            {
                return _cache;
            }
            return Collection.TryGetValue(id, out _cache) ? _cache : null;
        }

        #region global variables

        #region set

        public void SetGlobalStringVariable(string varName, string value) => GlobalVariables.Set(varName, value);

        public void SetGlobalIntVariable(string varName, int value) => GlobalVariables.Set(varName, value);

        public void SetGlobalFloatVariable(string varName, float value) => GlobalVariables.Set(varName, value);

        public void SetGlobalDoubleVariable(string varName, double value) => GlobalVariables.Set(varName, value);

        public void SetGlobalObjectVariable(string varName, object value) => GlobalVariables.Set(varName, value);

        #endregion set

        #region get

        public string GetGlobalStringVariable(string varName) => GlobalVariables.Get(varName, string.Empty);

        public int GetGlobalIntVariable(string varName) => GlobalVariables.Get(varName, 0);

        public float GetGlobalFloatVariable(string varName) => GlobalVariables.Get(varName, 0f);

        public double GetGlobalDoubleVariable(string varName) => GlobalVariables.Get(varName, 0d);

        public object GetGlobalObjectVariable(string varName) => GlobalVariables.Get<object>(varName, null);

        #endregion get

        #endregion global variables

        #region fsm variables

        #region set

        public void SetVariable<T>(int fsmID, string varName, T value)
        {
            if (_cache != null && _cache.ID == fsmID)
            {
                _cache.Variables.Set(varName, value);
            }
            else if (Collection.TryGetValue(fsmID, out _cache))
            {
                _cache.Variables.Set(varName, value);
            }
        }

        public void SetStringVariable(int id, string varName, string value) => SetVariable(id, varName, value);

        public void SetIntVariable(int id, string varName, int value) => SetVariable(id, varName, value);

        public void SetFloatVariable(int id, string varName, float value) => SetVariable(id, varName, value);

        public void SetDoubleVariable(int id, string varName, double value) => SetVariable(id, varName, value);

        public void SetObjectVariable(int id, string varName, object value) => SetVariable(id, varName, value);

        #endregion set

        #region get

        public T GetVariable<T>(int fsmID, string varName, T defaultValue)
        {
            if (_cache != null && _cache.ID == fsmID)
            {
                return _cache.Variables.Get(varName, defaultValue);
            }
            else if (Collection.TryGetValue(fsmID, out _cache))
            {
                return _cache.Variables.Get(varName, defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        public string GetStringVariable(int fsmID, string varName, string defaultValue = "") => GetVariable(fsmID, varName, defaultValue);

        public int GetIntVariable(int fsmID, string varName, int defaultValue = 0) => GetVariable(fsmID, varName, defaultValue);

        public float GetFloatVariable(int fsmID, string varName, float defaultValue = 0f) => GetVariable(fsmID, varName, defaultValue);

        public double GetDoubleVariable(int fsmID, string varName, double defaultValue = 0d) => GetVariable(fsmID, varName, defaultValue);

        public object GetObjectVarialble(int fsmID, string varName, object defaultValue = null) => GetVariable(fsmID, varName, defaultValue);

        #endregion get

        #endregion fsm variables

        #region trigger

        public void Trigger<T>(int id, T value)
        {
            if (_cache != null && _cache.ID == id)
            {
                _cache.Trigger(value);
            }
            else if (Collection.TryGetValue(id, out _cache))
            {
                _cache.Trigger(value);
            }
            else
            {
                Debug.LogWarning($"there is not a fsm that id is {id}");
            }
        }

        public void TriggerString(int id, string value) => Trigger(id, value);

        public void TriggerInt(int id, int value) => Trigger(id, value);

        public void TriggerFloat(int id, float value) => Trigger(id, value);

        public void TriggerDouble(int id, double value) => Trigger(id, value);

        public void TriggerObject(int id, object value) => Trigger(id, value);

        #endregion trigger
    }
}