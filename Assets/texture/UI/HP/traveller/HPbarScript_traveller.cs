using UnityEngine;

public class HPbarScript_traveller : MonoBehaviour
{
    [SerializeField] Animator animator;
    private GameObject GameManager;
    player1 player1;

    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        player1 = GameManager.GetComponent<player1>();
    }

    void Update()
    {
        animator.SetInteger("Traveller_HP", player1.HP_1);
    }
}
