using UnityEngine;
using UnityEngine.InputSystem;

public class controllerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove_press(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("pressed");
        rb.AddForce(Vector2.up * 1.5f, ForceMode2D.Impulse);
    }

    public void OnMove_release(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("released");
        rb.AddForce(Vector2.up * 0, ForceMode2D.Impulse);
    }
}
