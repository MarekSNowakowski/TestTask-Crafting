using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Transform Anchor;
    public string Name;
    
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    public Rigidbody _rigidbody;

    [Header("Outlining")]
    [SerializeField]
    private Outline _outline;
    [SerializeField]
    private Color _ableToGrabOutlineColor = Color.black;
    [SerializeField]
    private float _ableToGrabOutlineWidth = 6f;
    [SerializeField]
    private Color _playerNearOutlineColor = Color.white;
    [SerializeField]
    private float _playerNearOutlineWidth = 2f;

    [Header("PlayerDetection")]
    [SerializeField]
    private SphereCollider _sphereCollider;
    [SerializeField]
    private float _playerDetectionRadius = 10f;

    [Header("Throwing")]
    [SerializeField]
    private float _throwTime = 2f;
    public bool Thrown { get; private set; }

    private bool _ableToGrab;
    private bool _grabbed;

    private const string PLAYER_TAG = "Player";

    private void Start()
    {
        _sphereCollider.radius = _playerDetectionRadius;
    }

    private void FixedUpdate()
    {
        // if player is able to grab, this value will be changed before late update
        _ableToGrab = false;    
    }

    private void LateUpdate()
    {
        UpdateOutlining();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG)) // Handle player entering detection radius
        {
            _outline.OutlineColor = _playerNearOutlineColor;
            _outline.OutlineWidth = _playerNearOutlineWidth;
            _outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG)) // Handle player leaving detection radius
        {
            _outline.enabled = false;
        }
    }
    
    public void OnGrabStart()
    {
        _rigidbody.useGravity = false;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
        _collider.enabled = false;
        _grabbed = true;
    }

    public void OnGrabEnd()
    {
        _rigidbody.useGravity = true;
        _collider.enabled = true;
        _grabbed = false;
    }

    public void OnAbleToGrab()
    {
        _ableToGrab = true;
    }

    public void OnThrowStart()
    {
        OnGrabEnd();
        StartCoroutine(ThrowCoroutine());
    }

    private void UpdateOutlining()
    {
        if (_ableToGrab || _grabbed)
        {
            _outline.OutlineColor = _ableToGrabOutlineColor;
            _outline.OutlineWidth = _ableToGrabOutlineWidth;
            _outline.enabled = true;
        }
        else
        {
            _outline.OutlineWidth = _playerNearOutlineWidth;
            _outline.OutlineColor = _playerNearOutlineColor;
        }
    }

    private IEnumerator ThrowCoroutine()
    {
        Thrown = true;
        yield return new WaitForSeconds(_throwTime);
        Thrown = false;
    }
}
