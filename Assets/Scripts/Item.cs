using System.Collections;
using UnityEngine;

namespace TestTaskCrafting.Crafting
{
    public class Item : MonoBehaviour, IInteractable
    {
        [field: SerializeField]
        public Transform Anchor { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }

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

        private const int PLAYER_LAYER = 8; // Character layer

        private void Awake()
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
            if(other.gameObject.layer == PLAYER_LAYER) // Handle player entering detection radius
            {
                _outline.OutlineColor = _playerNearOutlineColor;
                _outline.OutlineWidth = _playerNearOutlineWidth;
                _outline.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.layer == PLAYER_LAYER) // Handle player leaving detection radius
            {
                _outline.enabled = false;
            }
        }

        public void OnInteracted()
        {
            // Grabbing starts
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

        public void OnAimedAt()
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
}
