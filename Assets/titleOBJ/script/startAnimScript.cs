using UnityEngine;

public class startAnimScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    private GameObject UI;
    UI_selector selector_script;
    void Start()
    {
        UI = GameObject.Find("UI");
        selector_script = UI.GetComponent<UI_selector>();
    }

    void Update()
    {
        if (selector_script.selector_start)
        {
            animator.SetBool("Start_bool", false);
        }

        else
        {
            animator.SetBool("Start_bool", true);
        }
    }
}
