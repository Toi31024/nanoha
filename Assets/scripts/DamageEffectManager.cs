using UnityEngine;
using UnityEngine.UI;

// ダメージエフェクトを管理するシングルトンクラス
public class DamageEffectManager : MonoBehaviour
{
    // シングルトンのインスタンスを保持する静的変数
    private static DamageEffectManager instance;

    // 外部からインスタンスにアクセスするためのプロパティ
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

    // --- インスペクターで設定する項目 ---
    [Header("エフェクトパーツ")]
    [SerializeField] private Image redFlashImage; // 画面を赤くする用のImage
    // 将来的に、ここへAnimatorなどを追加していく
    // [SerializeField] private Animator biriBiriAnimator;

    void Update()
    {
        // 赤い画面を徐々に透明にしていく
        if (redFlashImage != null && redFlashImage.color.a > 0)
        {
            redFlashImage.color = Color.Lerp(redFlashImage.color, Color.clear, Time.deltaTime * 3);
        }
    }

    /// <summary>
    /// 指定された種類のエフェクトを再生する
    /// </summary>
    public void PlayEffect(DamageEffectType effectType)
    {
        switch (effectType)
        {
            case DamageEffectType.RedFlash:
                if (redFlashImage != null)
                {
                    redFlashImage.color = new Color(0.7f, 0, 0, 0.9f);
                }
                else
                {
                    Debug.LogError("RedFlashImageがインスペクターで設定されていません。");
                }
                break;

            case DamageEffectType.BiriBiri:
                // 将来的にBiriBiriエフェクトを実装する場合の処理
                Debug.Log("BiriBiriエフェクトを再生！");
                // biriBiriAnimator.SetTrigger("Play");
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
        DontDestroyOnLoad(this.gameObject);
    }
}
