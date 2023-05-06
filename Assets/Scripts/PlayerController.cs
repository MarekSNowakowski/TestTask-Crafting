using StarterAssets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform ItemHoldAnchor;
    public Transform CameraAnchor;
    public float GrabDistance = 3f;
    public float LerpDuration = 5f;
    
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
                item.OnAbleToGrab();
                
                if (_input.grab)
                {
                    _grabbedItem = item;
                    _grabbedItem.OnGrabStart();
                    _moveTime = 0;
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
        }
        else
        {
            _grabbedItem.OnGrabEnd();
            _grabbedItem = null;
        }
    }
}
