using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    [RequireComponent(typeof(Character))]
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        class Action
        {
            [SerializeField] InputActionReference input;
            public System.Action<InputAction.CallbackContext> onInput;
            public System.Action<Collision> onCollisionEnter;

            public void Connect(bool b)
            {
                if (!input) return;
                
                if (b)
                {
                    input.action.started += onInput;
                    input.action.performed += onInput;
                    input.action.canceled += onInput;
                }
                else
                {
                    input.action.started -= onInput;
                    input.action.performed -= onInput;
                    input.action.canceled -= onInput;
                }
            }
        }
        [SerializeField] private Action move;
        [SerializeField] private Action jump;
        [SerializeField] private float airborneSpeedMultiplier = .5f;
        [SerializeField] private int maxJumps = 2;
        List<Action> actions;
        private int _currentJumps;
        private Character _character;
        private Coroutine _jumpCoroutine;

        private void Awake()
        {
            _character = GetComponent<Character>();
            
            actions = new List<Action>();
            
            actions.Add(move);
            move.onInput += (ctx) =>
            {
                var direction = ctx.ReadValue<Vector2>().ToHorizontalPlane();
                if (_currentJumps > 0)
                    direction *= airborneSpeedMultiplier;
                _character?.SetDirection(direction);
            };
            
            actions.Add(jump);
            jump.onInput += (ctx) =>
            {
                if (ctx.canceled || ctx.started) return;
                if (_currentJumps >= maxJumps) return;
                if (_jumpCoroutine != null)
                    StopCoroutine(_jumpCoroutine);
                _jumpCoroutine = StartCoroutine(_character.Jump());
                _currentJumps++;
            };
            jump.onCollisionEnter += (c) =>
            {
                List<ContactPoint> cPoints = new List<ContactPoint>();
                c.GetContacts(cPoints);
                for (var index = 0; index < cPoints.Count; index++)
                    if (Vector3.Angle(cPoints[index].normal, Vector3.up) < 5)
                        _currentJumps = 0;
            };
        }

        private void OnEnable()
        {
            for (int i = 0; i < actions.Count; i++)
                actions[i].Connect(true);
        }
        private void OnDisable()
        {
            for (int i = 0; i < actions.Count; i++)
                actions[i].Connect(false);
        }

        void OnCollisionEnter(Collision other)
        {
            for (int i = 0; i < actions.Count; i++)
                actions[i].onCollisionEnter?.Invoke(other);
        }
    }
}