using StarterAssets;
using TestTaskCrafting.Crafting;
using UnityEngine;

namespace TestTaskCrafting.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform _itemHoldAnchor;
        [SerializeField]
        private Transform _cameraAnchor;
        [SerializeField]
        private float _grabDistance = 3f;
        [SerializeField]
        private float _lerpDuration = 5f;
        [SerializeField]
        private float _throwForce = 300f;
        
        private StarterAssetsInputs _input;
        private Item _grabbedItem;
        private float _moveTime;

        private void Awake()
        {
            _input = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if (_grabbedItem == null)
            {
                HandleInteraction();
            }
            else
            {
                HandleGrabbedItem();
            }
        }

        private void HandleInteraction()
        {
            if (Physics.Raycast(_cameraAnchor.position, _cameraAnchor.transform.forward, out RaycastHit raycastHit, _grabDistance))
            {
                if (raycastHit.transform.TryGetComponent(out IInteractable interactable))
                {
                    if (interactable is Item item)
                    {
                        HandleItemInteraction(item);
                    }
                    else
                    {
                        HandleInteraction(interactable);
                    }
                }
            }
        }

        private void HandleItemInteraction(Item item)
        {
            if (!item.Thrown)
            {
                item.OnAimedAt();
                    
                if (_input.grab)
                {
                    _grabbedItem = item;
                    _grabbedItem.OnInteracted();
                    _moveTime = 0;
                }
            }
        }

        private void HandleInteraction(IInteractable interactable)
        {
            interactable.OnAimedAt();

            if (_input.interact)
            {
                interactable.OnInteracted();
            }
        }

        private void HandleGrabbedItem()
        {
            if (_input.grab)
            {
                if (_moveTime < _lerpDuration)
                {
                    _grabbedItem.Anchor.position = Vector3.Lerp(_grabbedItem.Anchor.position, _itemHoldAnchor.position, _moveTime / _lerpDuration);
                }
                else
                {
                    _moveTime = 0;
                }

                _grabbedItem.Anchor.rotation = _itemHoldAnchor.rotation;
                _moveTime += Time.deltaTime;

                HandleThrow();
            }
            else
            {
                _grabbedItem.OnGrabEnd();
                _grabbedItem = null;
            }
        }

        private void HandleThrow()
        {
            if (_input.interact)
            {
                _grabbedItem.OnThrowStart();
                _grabbedItem._rigidbody.AddForce(_cameraAnchor.transform.forward * _throwForce);
                _grabbedItem = null;
            }
        }
    }
}
