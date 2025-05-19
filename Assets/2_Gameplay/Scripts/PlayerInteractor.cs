using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class PlayerInteractor : MonoBehaviour, IInteractor
    {
        [SerializeField] private InputActionReference interactInput;
        [SerializeField] private TMP_Text interactionText;
        private IInteractable _interactable; //Strategy Pattern
        private string _textFormat = string.Empty;

        private void Awake()
        {
            if (interactionText)
                _textFormat = interactionText.text;
        }

        private void OnEnable()
        {
            if (interactInput?.action != null)
                interactInput.action.started += HandleInteractInput;
        }
        
        private void OnDisable()
        {
            if (interactInput?.action != null)
                interactInput.action.started -= HandleInteractInput;
        }

        private void HandleInteractInput(InputAction.CallbackContext ctx)
        {
            if (_interactable == null)
                return;
            _interactable.Interact(this);
            interactionText?.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out _interactable)
                && interactionText)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.SetText(string.Format(_textFormat, _interactable));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable otherInteractable)
                && otherInteractable == _interactable)
            {
                _interactable = null;
                interactionText?.gameObject.SetActive(false);
            }
        }
    }
}