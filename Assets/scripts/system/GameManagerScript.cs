using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] player1 player1;
    [SerializeField] player2 player2;
    [SerializeField] SmashBall smashBall;

    void Start()
    {
        smashBall.smashball_spawn();
    }

    void Update()
    {
        if (player1.HP_1 == 0)
        {
            //ゲーム終わり
        }

        else if (player2.HP_2 == 0)
        {
            //ゲーム終わり
        }
    }
}
