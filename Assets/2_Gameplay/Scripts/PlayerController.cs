using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Actions.MoveDecorator move;
        [SerializeField] Actions.JumpDecorator jump;
        [SerializeField] Rigidbody rb;
        Actions.ActionDecorator _actionStack;
        System.Action<IEnumerator, string> routineStarter;
        System.Action<string> routineStopper;
        Dictionary<string, Coroutine> coroutinesByID;
        

        private void Awake()
        {
            List<Actions.ActionDecorator> actions = new List<Actions.ActionDecorator>();
            actions.Add(move);
            actions.Add(jump);

            for (int i = 1; i < actions.Count; i++)
            {
                _actionStack = actions[i]; //update current
                _actionStack.SetSource(actions[i-1]); //set previous as source
            }
            
            if(actions.Count == 1) _actionStack = actions[0];
            
            _actionStack.SetTopAction(null);

            Actions.ActionReferences refs = new Actions.ActionReferences
            {
                transform = transform, rb = rb
            };
            _actionStack.SetReferences(refs);

            coroutinesByID = new Dictionary<string, Coroutine>();
            routineStarter += (ie, id) =>
            {
                Coroutine routine = StartCoroutine(ie);
                if(coroutinesByID.TryAdd(id, routine))
                    _actionStack.SetRoutine(routine, id);
            };
            routineStopper += (id) =>
            {
                if (coroutinesByID.TryGetValue(id, out Coroutine routine))
                {
                    StopCoroutine(routine);
                    coroutinesByID.Remove(id);
                }
            };
            _actionStack.SetRoutineStarter(routineStarter);
            _actionStack.SetRoutineStopper(routineStopper);

            _actionStack.OnAwake();
        }
        void Start() => _actionStack.OnStart();
        void Update() => _actionStack.OnUpdate(Time.deltaTime);
        void FixedUpdate() => _actionStack.OnFixedUpdate(Time.fixedDeltaTime);
        void LateUpdate() => _actionStack.OnUpdate(Time.deltaTime);
        void OnEnable() => _actionStack.OnEnable();
        void OnDisable() => _actionStack.OnEnable();
        void OnDestroy() => _actionStack.OnEnable();
        void OnCollisionEnter(Collision other) => _actionStack.OnCollisionEnter(other);
        void OnCollisionStay(Collision other) => _actionStack.OnCollisionStay(other);
        void OnCollisionExit(Collision other) => _actionStack.OnCollisionExit(other);
        void OnTriggerEnter(Collider other) => _actionStack.OnTriggerEnter(other);
        void OnTriggerStay(Collider other) => _actionStack.OnTriggerStay(other);
        void OnTriggerExit(Collider other) => _actionStack.OnTriggerExit(other);
    }
}