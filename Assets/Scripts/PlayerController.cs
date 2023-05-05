using StarterAssets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform ItemHoldAnchor;
    
    private StarterAssetsInputs _input;

    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_input.grab)
        {
            Debug.Log("Works");
        }
    }
}
