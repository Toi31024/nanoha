using System.Collections.Generic;
using UnityEngine;

// 1. 保存するデータ構造を定義
// [System.Serializable] をつけることで、JsonUtilityで変換可能になる
[System.Serializable]
public class RankingData
{
    // スコアのリスト
    public List<int> scores = new List<int>();
}

// 2. ランキングを管理する静的クラス (MonoBehaviourを継承しない)
public static class RankingManager
{
    private const string RankingKey = "HighScores"; // PlayerPrefsに保存する時のキー
    private const int MaxRankingEntries = 10;      // ランキングに保存する最大数 (例: トップ10)

    // 新しいスコアを追加するメソッド
    public static void AddScore(int newScore)
    {
        // 1. 現在のランキングを読み込む
        RankingData data = LoadRanking();

        // 2. 新しいスコアを追加
        data.scores.Add(newScore);

        // 3. スコアを降順 (高い順) にソートする
        data.scores.Sort((a, b) => b.CompareTo(a));

        // 4. 最大数を超えていたら、はみ出した分を削除
        if (data.scores.Count > MaxRankingEntries)
        {
            // GetRange(0, MaxRankingEntries) で、0番目からMaxRankingEntries個の要素だけを抜き出す
            data.scores = data.scores.GetRange(0, MaxRankingEntries);
        }

        // 5. 変更したデータを保存
        SaveRanking(data);
    }

    // ランキングのリストを取得するメソッド
    public static List<int> GetRanking()
    {
        return LoadRanking().scores;
    }

    // (内部処理) データを読み込む
    private static RankingData LoadRanking()
    {
        // PlayerPrefsからJSON文字列を取得 ("{}" はデータが無い場合のデフォルト値)
        string json = PlayerPrefs.GetString(RankingKey, "{}");

        // JSON文字列をRankingDataクラスのオブジェクトに変換
        RankingData data = JsonUtility.FromJson<RankingData>(json);

        // 初回起動時など、scoresがnullの場合に初期化
        if (data.scores == null)
        {
            data.scores = new List<int>();
        }

        return data;
    }

    // (内部処理) データを保存する
    private static void SaveRanking(RankingData data)
    {
        // RankingDataオブジェクトをJSON文字列に変換
        string json = JsonUtility.ToJson(data);

        // PlayerPrefsに保存
        PlayerPrefs.SetString(RankingKey, json);
        PlayerPrefs.Save(); // 念のため即時保存
    }
}