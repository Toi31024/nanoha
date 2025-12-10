using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面全体のエフェクト（赤いフラッシュなど）を再生するシンプルなクラス
/// </summary>
public class DamageScreenEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("色を変えたいUIのImageコンポーネントをここに設定")]
    private Image effectImage;

    private void Start()
    {
        // 開始時は必ず透明にしておく
        if (effectImage != null)
        {
            effectImage.color = Color.clear;
        }
    }

    private void Update()
    {
        // イメージのアルファ値(透明度)が0より大きい場合、徐々に透明に戻していく
        if (effectImage != null && effectImage.color.a > 0)
        {
            effectImage.color = Color.Lerp(effectImage.color, Color.clear, Time.deltaTime * 3f);
        }
    }

    /// <summary>
    /// 画面を赤くフラッシュさせるエフェクトを再生します
    /// </summary>
    public void PlayRedFlash()
    {
        if (effectImage != null)
        {
            effectImage.color = new Color(0.7f, 0, 0, 0.9f);
        }
        else
        {
            Debug.LogError("Effect Imageがインスペクターで設定されていません！");
        }
    }
}
