using UnityEngine;
using System.Collections;

public class SmashBall : MonoBehaviour
{

    float spawnX;
    float spawnY;

    public void smashball_spawn()
    {
        StartCoroutine(Smash_spawn());
    }

    IEnumerator Smash_spawn()
    {
        yield return new WaitForSeconds(15f);
        Debug.Log("スマッシュボール");
        Transform myTransform = this.transform;

        Vector2 pos = myTransform.position;
        pos.x = 6.0f;
        pos.y = -7.5f;

        myTransform.position = pos;
    }

}
