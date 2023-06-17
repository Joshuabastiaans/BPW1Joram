using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MessageRemove : MonoBehaviour
{
    [SerializeField] private Image textImage;
    [SerializeField] private bool isEnabledAtStart;

    private PlayerInputSystem playerControls;
    private InputAction trigger;
    private InputAction move;

    public void Awake()
    {
        if (textImage != null && isEnabledAtStart)
        {
            textImage.enabled = true;
            Time.timeScale = 0f;
        }
        playerControls = new PlayerInputSystem();
    }

    public void ShowImage()
    {
        if (textImage != null && !isEnabledAtStart)
        {
            textImage.enabled = true;
            Time.timeScale = 0f;
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        OnDisable();
        yield return new WaitForSeconds(2f);
        OnEnable();
    }

    public void OnEnable()
    {
        trigger = playerControls.Player.Fire;
        trigger.performed += OnTriggerSwitch;
        trigger.Enable();
        move = playerControls.Player.Move;
        move.performed += OnTriggerSwitch;
        move.Enable();
    }

    public void OnDisable()
    {
        trigger.Disable();
        move.Disable();
    }

    public void OnTriggerSwitch(InputAction.CallbackContext context)
    {
        if (textImage != null)
        {
            textImage.enabled = false;
            Time.timeScale = 1f;
        }
    }
}
