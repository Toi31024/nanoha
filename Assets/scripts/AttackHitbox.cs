using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("この攻撃のダメージ量")]
    [SerializeField] public int attackDamage = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("2. ヒットボックスが何かに触れた。相手の名前: " + other.name);
        if (other.CompareTag("Target"))
        {
            Debug.Log("3. 相手は'Target'タグ持ち。EnemyHealthスクリプトを検索。");
            TargetHp targetHp = other.GetComponent<TargetHp>();
            if (targetHp != null)
            {
                Debug.Log("4. EnemyHealthスクリプトを発見！ TakeDamageを呼び出します。");
                targetHp.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogError("エラー: 'Target'タグはありますが、EnemyHealthスクリプトが見つかりません！");
            }
            gameObject.SetActive(false);
        }
    }
}
