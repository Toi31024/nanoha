using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
public class canon : MonoBehaviour
{
    //プレイヤーオブジェクト
    [SerializeField] private GameObject player;
    //弾のプレハブオブジェクト
    [SerializeField] private GameObject tama;

    private int how_many_time = 0;

    //3秒ごとに弾を発射するための変数
    private float targetTime = 3.0f;
    private float currentTime = 0;
    private float speed = 0.8f;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > targetTime)
        {
            currentTime = 0;
            //敵の座標を変数posに保存
            var pos = this.gameObject.transform.position;
            //弾のプレハブ作成
            var t = Instantiate(tama) as GameObject;
            //弾のプレハブの位置を敵の位置に設定
            t.transform.position = pos;

            //敵からプレイヤーに向かうベクトルを作る
            UnityEngine.Vector2 vec = player.transform.position - pos;
            //弾のRigidBody2Dコンポーネントのvelocityに、先ほど求めたベクトルを入れてチカラを加える
            t.GetComponent<Rigidbody2D>().linearVelocity = vec * speed;
            tama.gameObject.transform.localScale = new UnityEngine.Vector3(0.3f, 0.3f, 0.3f);

            how_many_time++;

            if (how_many_time >= 12)
            {
                Debug.Log("キャノン難易度増加１");
                speed = 1.0f;
                targetTime = 2.4f;
            }

            if (how_many_time >= 25)
            {
                Debug.Log("キャノン難易度増加２");
                speed = 0.6f;
                targetTime = 1.1f;
            }
        }
    }
}