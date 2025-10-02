using UnityEngine;

// このスクリプトはテスト専用です
public class SimpleTriggerLogger : MonoBehaviour
{
    // このオブジェクトのTriggerが、他のColliderに触れた瞬間に呼ばれる
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 成功したら、このメッセージが必ずConsoleに表示される
        Debug.Log("【成功！】Trigger検知！ 相手の名前: " + other.name, other.gameObject);
    }
}