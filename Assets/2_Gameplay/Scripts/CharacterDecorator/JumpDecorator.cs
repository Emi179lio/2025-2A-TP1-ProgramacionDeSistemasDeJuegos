using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputActionReference = UnityEngine.InputSystem.InputActionReference;

namespace Gameplay.Actions
{
    [System.Serializable]
    public class JumpDecorator : ActionDecorator
    {
        [SerializeField] InputActionReference input;
        [SerializeField] float jumpForce = 10;
        [SerializeField] int maxJumps = 2;
        int _currentJumps;
        Rigidbody _rb;
        Coroutine _jumpCoroutine;

        public bool IsJumping => _currentJumps > 0;
        
        const string JumpID = "RB_Jump";
        
        public override void EnableAction(ActionType type, bool enable)
        {
            if (type != ActionType.Jump) _sourceAction.EnableAction(type, enable);
            else _isEnabled = enable;
        }
        public override IAction GetAction(ActionType type)
        {
            if (type == ActionType.Jump) return this;
            if (_sourceAction != null) return _sourceAction.GetAction(type);
            
            //If this isn't the action requested and there's no more actions, return null
            return null;
        }
        public override void SetReferences(ActionReferences references)
        {
            _rb = references.rb;
            base.SetReferences(references);
        }
        
        //Coroutine Helpers
        public override bool SetRoutine(Coroutine routine, string routineID)
        {
            if(!base.SetRoutine(routine, routineID)) return false;
            
            if(routineID == JumpID) _jumpCoroutine = routine;

            return true;
        }

        //Unity callbacks
        public override bool OnCollisionEnter(Collision collision)
        {
            if (!base.OnCollisionEnter(collision)) return false;
            
            List<ContactPoint> cPoints = new List<ContactPoint>();
            collision.GetContacts(cPoints);
            for (var index = 0; index < cPoints.Count; index++)
                if (Vector3.Angle(cPoints[index].normal, Vector3.up) < 5)
                    _currentJumps = 0;

            return true;
        }
        public override bool OnEnable()
        {
            if(!base.OnEnable()) return false;
            
            input.action.performed += (c) => Jump();

            return true;
        }
        public override bool OnDisable()
        {
            if(!base.OnDisable()) return false;
            
            input.action.performed += (c) => Jump();

            return true;
        }

        //Private Methods
        IEnumerator JumpCoroutine()
        {
            yield return new WaitForFixedUpdate();
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        void Jump()
        {
            if (_currentJumps >= maxJumps) return;
            
            //Stop previous jump
            if (_jumpCoroutine != null)
            {
                _routineStopper?.Invoke(JumpID);
                _jumpCoroutine = null;
            }
            
            //Start jump routine
            _routineStarter?.Invoke(JumpCoroutine(), JumpID);
            
            //Increase jump counter
            _currentJumps++;
        }
    }
}