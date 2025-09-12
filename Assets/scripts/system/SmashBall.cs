using UnityEngine;
using System.Collections;

public class SmashBall : MonoBehaviour
{
    public void smashball_spawn()
    {
        StartCoroutine(Smash_spawn());
    }

    IEnumerator Smash_spawn()
    {
        yield return new WaitForSeconds(15f);
        Debug.Log("スマッシュボール");
    }

}
