using System.Collections;
using UnityEngine;

namespace Gameplay.Actions
{
    [System.Serializable]
    public abstract class ActionDecorator : IAction
    {
        internal IAction _topAction;
        internal IAction _sourceAction;
        internal bool _isEnabled = true;
        /// <summary>
        /// Invoke when need to start a routine
        /// </summary>
        internal System.Action<IEnumerator, string> _routineStarter;
        /// <summary>
        /// Invoke when need to stop a routine
        /// </summary>
        internal System.Action<string> _routineStopper;
        
        
        //Pseudo-Constructors
        public void SetTopAction(IAction top)
        {
            if (top == null)
                _sourceAction?.SetTopAction(this);
            else
            {
                _topAction = top;
                _sourceAction?.SetTopAction(top);
            }
        }
        public void SetSource(IAction sourceAction) => _sourceAction = sourceAction;
        
        //Action type dependant methods
        public abstract void EnableAction(ActionType type, bool enable);
        public abstract IAction GetAction(ActionType type);
        public virtual void SetReferences(ActionReferences references) 
            => _sourceAction?.SetReferences(references);

        //Coroutine Helpers
        public virtual bool SetRoutine(Coroutine routine, string routineID)
        {
            _sourceAction?.SetRoutine(routine, routineID);

            return _isEnabled;
        }
        public virtual void SetRoutineStarter(System.Action<IEnumerator, string> routineStarter)
        {
            _routineStarter = routineStarter;
            _sourceAction?.SetRoutineStarter(routineStarter);
        }
        public virtual void SetRoutineStopper(System.Action<string> routineStopper)
        {
            _routineStopper = routineStopper;
            _sourceAction?.SetRoutineStopper(routineStopper);
        }
        
        //Unity callbacks
        public virtual bool OnAwake()
        {
            _sourceAction?.OnAwake();
            return _isEnabled;
        }
        public virtual bool OnStart()
        {
            _sourceAction?.OnStart();
            return _isEnabled;
        }
        public virtual bool OnUpdate(float dt)
        {
            _sourceAction?.OnUpdate(dt);
            return _isEnabled;
        }
        public virtual bool OnFixedUpdate(float dt)
        {
            _sourceAction?.OnFixedUpdate(dt);
            return _isEnabled;
        }
        public virtual bool OnLateUpdate(float dt)
        {
            _sourceAction?.OnLateUpdate(dt);
            return _isEnabled;
        }
        public virtual bool OnEnable()
        {
            _sourceAction?.OnEnable();
            return _isEnabled;
        }
        public virtual bool OnDisable()
        {
            _sourceAction?.OnDisable();
            return _isEnabled;
        }
        public virtual bool OnDestroy()
        {
            _sourceAction?.OnDestroy();
            return _isEnabled;
        }
        public virtual bool OnCollisionEnter(Collision collision)
        {
            _sourceAction?.OnCollisionEnter(collision);
            return _isEnabled;
        }
        public virtual bool OnCollisionExit(Collision collision)
        {
            _sourceAction?.OnCollisionExit(collision);
            return _isEnabled;
        }
        public virtual bool OnCollisionStay(Collision collision)
        {
            _sourceAction?.OnCollisionStay(collision);
            return _isEnabled;
        }
        public virtual bool OnTriggerEnter(Collider other)
        {
            _sourceAction?.OnTriggerEnter(other);
            return _isEnabled;
        }
        public virtual bool OnTriggerExit(Collider other)
        {
            _sourceAction?.OnTriggerExit(other);
            return _isEnabled;
        }
        public virtual bool OnTriggerStay(Collider other)
        {
            _sourceAction?.OnTriggerStay(other);
            return _isEnabled;
        }
    }
}
