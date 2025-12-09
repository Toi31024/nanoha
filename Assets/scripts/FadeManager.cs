using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : Singleton<FadeManager>
{
    private Animator anim;
    private bool isChanged = false;

    protected override void Awake()
    {
        base.Awake();
        anim = this.GetComponent<Animator>();
    }

    public bool FadeToScene(string sceneName)
    {
        if (isChanged)
        {
            Debug.LogWarning("すでにフェード処理が実行中です。");
            return false;
        }

        StartCoroutine(FadeAndLoad(sceneName));
        return true;
    }

    public bool ReturnNowFade()
    {
        return isChanged;
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isChanged = true;

        anim.SetTrigger("FadeIn");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // "FadeIn"アニメーションの開始を待つ
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"));
        // "FadeIn"アニメーションの終了を待つ
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        yield return new WaitForSecondsRealtime(0.05f);

        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);
        yield return new WaitForSecondsRealtime(0.05f);
        yield return StartCoroutine(Fade(0));
        isChanged = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        anim.SetTrigger(targetAlpha == 1 ? "FadeIn" : "FadeOut");
        string stateName = targetAlpha == 1 ? "FadeIn" : "FadeOut";

        // 指定されたアニメーションステートに遷移するまで待機
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            yield return null;
        }

        // アニメーションの再生が完了するまで待機
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
    }
}