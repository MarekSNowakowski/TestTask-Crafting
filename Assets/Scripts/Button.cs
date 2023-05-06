using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public enum ButtonState
    {
        Default,
        AimedAt,
        Clicked
    }

    public UnityAction ButtonPressed;

    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private Material _defaultMaterial;
    [SerializeField]
    private Material _aimedAtMaterial;
    [SerializeField]
    private Material _clickedColorMaterial;

    private ButtonState _buttonState;

    public void FixedUpdate()
    {
        _buttonState = ButtonState.Default;
    }

    private void LateUpdate()
    {
        UpdateButton();
    }

    public void OnButtonAimed()
    {
        _buttonState = ButtonState.AimedAt;
    }
    
    public void OnButtonPressed()
    {
        _buttonState = ButtonState.Clicked;
        ButtonPressed?.Invoke();
    }

    private void UpdateButton()
    {
        switch (_buttonState)
        {
            case ButtonState.Default:
                _renderer.material = _defaultMaterial;
                break;
            case ButtonState.AimedAt:
                _renderer.material = _aimedAtMaterial;
                break;
            case ButtonState.Clicked:
                _renderer.material = _clickedColorMaterial;
                break;
        }
    }
}
