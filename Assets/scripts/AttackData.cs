using UnityEngine;

// このスクリプトは、敵の攻撃やトゲ床など、
// ダメージを与えるオブジェクトにアタッチします。
public class AttackData : MonoBehaviour
{
    [Tooltip("この攻撃がプレイヤーに与えるダメージ量")]
    public int damage = 1;

    [Tooltip("この攻撃がプレイヤーを硬直（スタン）させるか")]
    public bool causesStun = false; 
}