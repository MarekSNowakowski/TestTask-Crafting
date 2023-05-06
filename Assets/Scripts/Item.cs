using UnityEngine;


public class Item : MonoBehaviour
{
    public Transform Anchor;
    public Collider Collider;
    public Rigidbody Rigidbody;

    [Header("Outlining")]
    public Outline Outline;
    public Color AbleToGrabOutlineColor = Color.black;
    public float AbleToGrabOutlineWidth = 6f;
    public Color PlayerNearOutlineColor = Color.white;
    public float PlayerNearOutlineWidth = 2f;

    [Header("PlayerDetection")]
    public SphereCollider SphereCollider;
    public float PlayerDetectionRadius = 10f;

    private bool _ableToGrab;
    private bool _grabbed;

    private const string PLAYER_TAG = "Player";

    private void Start()
    {
        SphereCollider.radius = PlayerDetectionRadius;
    }

    private void Update()
    {
        _ableToGrab = false;
    }

    private void LateUpdate()
    {
        if (_ableToGrab || _grabbed)
        {
            Outline.OutlineColor = AbleToGrabOutlineColor;
            Outline.OutlineWidth = AbleToGrabOutlineWidth;
            Outline.enabled = true;
        }
        else
        {
            Outline.OutlineWidth = PlayerNearOutlineWidth;
            Outline.OutlineColor = PlayerNearOutlineColor;
        }
    }
    
    public void OnGrabStart()
    {
        Rigidbody.useGravity = false;
        Rigidbody.angularVelocity = Vector3.zero;
        Rigidbody.velocity = Vector3.zero;
        Collider.enabled = false;
        _grabbed = true;
    }

    public void OnGrabEnd()
    {
        Rigidbody.useGravity = true;
        Collider.enabled = true;
        _grabbed = false;
    }

    public void OnAbleToGrab()
    {
        _ableToGrab = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Outline.OutlineColor = PlayerNearOutlineColor;
            Outline.OutlineWidth = PlayerNearOutlineWidth;
            Outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            Outline.enabled = false;
        }
    }
}
