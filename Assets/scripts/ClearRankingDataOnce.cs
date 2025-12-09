using UnityEngine;

public class ClearRankingDataOnce : MonoBehaviour
{
    private const string RankingKey = "HighScores"; // 削除対象のキー

    void Start()
    {
        // PlayerPrefsから指定のキーのデータを削除する
        if (PlayerPrefs.HasKey(RankingKey))
        {
            PlayerPrefs.DeleteKey(RankingKey);
            PlayerPrefs.Save(); // 変更を即時保存
            Debug.Log("PlayerPrefsのランキングデータ (キー: " + RankingKey + ") を削除しました。");
        }
        else
        {
            Debug.Log("PlayerPrefsにランキングデータ (キー: " + RankingKey + ") は見つかりませんでした。");
        }

        // このスクリプトは役目を終えたので、自身を無効化または削除する
        // 開発環境で一度だけ実行したい場合は、この行をコメントアウト解除してシーンから削除してもOKです。
        // Destroy(gameObject); // このスクリプトがアタッチされているGameObjectを削除
        Destroy(this); // このスクリプトコンポーネントのみを削除
    }
}
