using UnityEngine;

// 攻撃によって再生するダメージエフェクトの種類を定義
public enum DamageEffectType
{
    RedFlash, // シンプルに画面を赤くする
    BiriBiri, // ビリビリした表現（将来的に実装）
}

// このスクリプトは、敵の攻撃やトゲ床など、
// ダメージを与えるオブジェクトにアタッチします。
public class AttackData : MonoBehaviour
{
    [Tooltip("この攻撃がプレイヤーに与えるダメージ量")]
    public int damage = 1;

    [Tooltip("この攻撃がプレイヤーを硬直（スタン）させるか")]
    public bool causesStun = false;

    [Tooltip("この攻撃がヒットした際に再生するエフェクトの種類")]
    public DamageEffectType effectType = DamageEffectType.RedFlash;
}
