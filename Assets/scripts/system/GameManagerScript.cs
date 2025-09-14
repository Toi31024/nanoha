using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] player1 player1;
    [SerializeField] player2 player2;
    SmashBall smashBall;
    GameObject obj;

    void Start()
    {
        obj = GameObject.Find("target1_0");
        smashBall = obj.GetComponent<SmashBall>();
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
