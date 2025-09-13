using UnityEngine;

public class traveller : MonoBehaviour
{
    private Animator anim = null;
    private Traveller_controller traveller_InputAction;
    void Start()
    {
        traveller_InputAction = new Traveller_controller();
        traveller_InputAction.Enable();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //移動モーション
        if (traveller_InputAction.Player.Fire.IsPressed())
        {
            Debug.Log("ぶっぱなすぜ！");
        }
    }
}
