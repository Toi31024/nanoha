using UnityEngine;

public class optionAnimScript : MonoBehaviour
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
            animator.SetBool("Option_bool", false);
        }

        else
        {
            animator.SetBool("Option_bool", true);
        }
    }
}
