using UnityEngine;
using InputActionReference = UnityEngine.InputSystem.InputActionReference;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Gameplay.Actions
{
    [System.Serializable]
    public class MoveDecorator : ActionDecorator
    {
        [SerializeField] InputActionReference input;
        [SerializeField] float speed = 3;
        [SerializeField] float acceleration = 10;
        [SerializeField] float airborneSpeedMultiplier = 0.5f;
        JumpDecorator _jumpDecorator;
        Rigidbody _rb;
        Vector3 _direction;

        public override void EnableAction(ActionType type, bool enable)
        {
            if(type != ActionType.Move) _sourceAction.EnableAction(type, enable);
            else _isEnabled = enable;
        }
        public override IAction GetAction(ActionType type)
        {
            if(type == ActionType.Move) return this;
            if(_sourceAction != null) return _sourceAction.GetAction(type);
            
            //If this isn't the action requested and there's no more actions, return null
            return null;
        }
        public override void SetReferences(ActionReferences references)
        {
            _rb = references.rb;
            base.SetReferences(references);
        }

        //Unity callbacks
        public override bool OnAwake()
        {
            if (!base.OnAwake()) return false;
                
            if(_topAction != null)
                _jumpDecorator = (JumpDecorator)_topAction.GetAction(ActionType.Jump);

            if (_jumpDecorator == null)
            {
                _isEnabled = false;
                Debug.LogError("JumpDecorator not found for MoveDecorator");
            }

            return true;
        }
        public override bool OnFixedUpdate(float dt)
        {
            if(!base.OnFixedUpdate(dt)) return false;
            
            var scaledDirection = _direction * acceleration;
            if (_rb.linearVelocity.IgnoreY().magnitude < speed)
                _rb.AddForce(scaledDirection, ForceMode.Force);

            return true;
        }
        public override bool OnEnable()
        {
            if(!base.OnEnable()) return false;
            
            input.action.started += Move;
            input.action.performed += Move;
            input.action.canceled += Move;
            

            return true;
        }
        public override bool OnDisable()
        {
            if(!base.OnDisable()) return false;
            
            input.action.started -= Move;
            input.action.performed -= Move;
            input.action.canceled -= Move;

            return true;
        }

        //Private Methods
        void Move(CallbackContext ctx)
        {
            var direction = ctx.ReadValue<Vector2>().ToHorizontalPlane();
            if (_jumpDecorator.IsJumping)
                direction *= airborneSpeedMultiplier;
            _direction = direction;
        }
    }
}