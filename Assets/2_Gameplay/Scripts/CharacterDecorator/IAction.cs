using System.Collections;
using UnityEngine;

namespace Gameplay.Actions
{
    public enum ActionType
    {
        Move,
        Jump,
        _count
    }
    
    public interface IAction
    {
        //Custom Methods
        void SetTopAction(IAction top);
        void EnableAction(ActionType type, bool enable);
        IAction GetAction(ActionType type);
        void SetReferences(ActionReferences references);
        
        //Coroutine Helpers
        /// <summary>
        /// This is a bool to not make disabled decorators check the id without need
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="routineID"></param>
        /// <returns></returns>
        bool SetRoutine(Coroutine routine, string routineID);
        void SetRoutineStarter(System.Action<IEnumerator, string> routineStarter);
        void SetRoutineStopper(System.Action<string> routineStopper);
        
        //Unity Callbacks
        bool OnAwake();
        bool OnStart();
        bool OnUpdate(float dt);
        bool OnFixedUpdate(float dt);
        bool OnLateUpdate(float dt);
        bool OnDisable();
        bool OnEnable();
        bool OnDestroy();
        bool OnCollisionEnter(Collision collision);
        bool OnCollisionExit(Collision collision);
        bool OnCollisionStay(Collision collision);
        bool OnTriggerEnter(Collider other);
        bool OnTriggerExit(Collider other);
        bool OnTriggerStay(Collider other);
    }
}
