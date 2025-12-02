using UnityEngine;
using UnityEngine.UI; // ButtonなどのUI要素を使う場合に必要

// ランキングを安全に削除するための機能を提供するクラス
public class SafeRankingDeleter : MonoBehaviour
{
    // インスペクターから確認ダイアログのGameObjectをアタッチする
    [SerializeField]
    private GameObject confirmationDialog;

    // --- ダイアログ表示/非表示を制御するメソッド ---

    // 確認ダイアログを表示する（「ランキング削除」ボタンから呼ばれる）
    public void ShowConfirmationDialog()
    {
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(true);
        }
    }

    // 確認ダイアログを非表示にする（「いいえ」ボタンから呼ばれる）
    public void HideConfirmationDialog()
    {
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false);
        }
    }

    // --- 実際の削除処理を行うメソッド ---

    // 削除を確定し、実行する（「はい」ボタンから呼ばれる）
    public void ConfirmAndDelete()
    {
        // 1. RankingManagerを使ってランキングを削除
        RankingManager.ClearRanking();

        // 2. 確認ダイアログを閉じる
        HideConfirmationDialog();

        // 3. (任意) ランキング表示を更新する
        //    シーンにDisplayRankingスクリプトがあれば、それを探してDisplay()メソッドを呼ぶ
        var rankingDisplay = FindObjectOfType<DisplayRanking>();
        if (rankingDisplay != null)
        {
            // Display()メソッドを実行するには、
            // DisplayRanking.csのDisplay()をprivateからpublicに変更する必要があります。
            // rankingDisplay.Display(); 
            Debug.Log("ランキングを削除しました。表示を更新するには、DisplayRanking.csのDisplay()をpublicにしてください。");
        }
    }
}
