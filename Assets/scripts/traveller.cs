using UnityEngine;

public class traveller : MonoBehaviour
{
    private Animator anim = null;
    private Traveller_controller traveller_InputAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        traveller_InputAction = new Traveller_controller();
        traveller_InputAction.Enable();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動モーション
        if (traveller_InputAction.Player.Fire.IsPressed())
        {
            Debug.Log("ぶっぱなすぜ！");
        }
    }
}
