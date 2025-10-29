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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("玉発射");
            //敵の座標を変数posに保存
            var pos = this.gameObject.transform.position;
            //弾のプレハブ作成
            var t = Instantiate(tama) as GameObject;
            //弾のプレハブの位置を敵の位置に設定
            t.transform.position = pos;

            //敵からプレイヤーに向かうベクトルを作る
            UnityEngine.Vector2 vec = player.transform.position - pos;
            //弾のRigidBody2Dコンポーネントのvelocityに、先ほど求めたベクトルを入れてチカラを加える
            t.GetComponent<Rigidbody2D>().linearVelocity = vec * 0.8f;
            tama.gameObject.transform.localScale = new UnityEngine.Vector3(0.3f, 0.3f, 0.3f);
        }
    }
}
