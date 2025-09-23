using UnityEngine;

public class HPbarScript_traveller : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject traveller_obj;
    traveller traveller_script;
    void Start()
    {
        traveller_script = traveller_obj.GetComponent<traveller>();
    }

    void Update()
    {
        animator.SetInteger("Traveller_HP", traveller_script.currentHp);
    }
}
