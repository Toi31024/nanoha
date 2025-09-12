using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] player1 player1;

    void OnTriggerEnter(Collider other)
    {
        player1.HP_1 -= 1;
        Debug.Log("被弾");
        Debug.Log(player1.HP_1);
    }
}
