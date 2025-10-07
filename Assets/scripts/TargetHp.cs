using UnityEngine;

public class TargetHp : MonoBehaviour
{
    public int hp = 1;
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("5. TakeDamageが実行されました。残りHP: " + hp);

        if (hp <= 0)
        {
            Debug.Log("ターゲット破壊");
            ScoreManager.AddTargetDestroyed();
            Destroy(gameObject);
        }
    
    }
}
