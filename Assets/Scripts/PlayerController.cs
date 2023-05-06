using StarterAssets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform ItemHoldAnchor;
    [SerializeField]
    private Transform CameraAnchor;
    [SerializeField]
    private float GrabDistance = 3f;
    [SerializeField]
    private float LerpDuration = 5f;
    [SerializeField]
    private float throwForce = 300f;
    
    private StarterAssetsInputs _input;
    private Item _grabbedItem;
    private float _moveTime;

    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_grabbedItem == null)
        {
            HandleItemDetection();
        }
        else
        {
            HandleGrabbedItem();
        }
    }

    private void HandleItemDetection()
    {
        if (Physics.Raycast(CameraAnchor.position, CameraAnchor.transform.forward, out RaycastHit raycastHit, GrabDistance))
        {
            if (raycastHit.transform.TryGetComponent(out Item item))
            {
                if (!item.Thrown)
                {
                    item.OnAbleToGrab();
                
                    if (_input.grab)
                    {
                        _grabbedItem = item;
                        _grabbedItem.OnGrabStart();
                        _moveTime = 0;
                    }
                }
            }
            else if (raycastHit.transform.TryGetComponent(out Button button))
            {
                button.OnButtonAimed();

                if (_input.interact)
                {
                    button.OnButtonPressed();
                }
            }
        }
    }

    private void HandleGrabbedItem()
    {
        if (_input.grab)
        {
            if (_moveTime < LerpDuration)
            {
                _grabbedItem.Anchor.position = Vector3.Lerp(_grabbedItem.Anchor.position, ItemHoldAnchor.position, _moveTime / LerpDuration);
            }
            else
            {
                _moveTime = 0;
            }

            _grabbedItem.Anchor.rotation = ItemHoldAnchor.rotation;
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
            _grabbedItem._rigidbody.AddForce(CameraAnchor.transform.forward * throwForce);
            _grabbedItem = null;
        }
    }
}
