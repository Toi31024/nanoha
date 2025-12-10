using UnityEngine;

// ダメージエフェクトをAnimatorで管理するシングルトンクラス
public class DamageEffectManager : MonoBehaviour
{
    private static DamageEffectManager instance;
    public static DamageEffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DamageEffectManager>();
                if (instance == null)
                {
                    var prefab = Resources.Load<GameObject>("DamageEffectManager");
                    if (prefab == null)
                    {
                        Debug.LogError("Resourcesフォルダに 'DamageEffectManager' プレハブが見つかりません。");
                        return null;
                    }
                    var go = Instantiate(prefab);
                    instance = go.GetComponent<DamageEffectManager>();
                }
            }
            return instance;
        }
    }

    private Animator anim;

    /// <summary>
    /// 指定された種類のエフェクトを再生する
    /// </summary>
    public void PlayEffect(DamageEffectType effectType)
    {
        // アニメーターのTriggerを起動して、対応するアニメーションを再生
        switch (effectType)
        {
            case DamageEffectType.RedFlash:
                anim.SetTrigger("PlayRedFlash");
                break;

            case DamageEffectType.BiriBiri:
                anim.SetTrigger("PlayBiriBiri");
                break;

            default:
                Debug.LogWarning("未対応のエフェクトタイプです: " + effectType);
                break;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        anim = GetComponent<Animator>(); // 自身にアタッチされたAnimatorを取得
        DontDestroyOnLoad(this.gameObject);
    }
}
