using UnityEngine;
using TMPro; // TextMeshProを使うために必要
using System.Collections.Generic;
using System.Text; // StringBuilderを使うために必要

public class DisplayRanking : MonoBehaviour
{
    // インスペクターから、作成したText - TextMeshProオブジェクトをドラッグ＆ドロップ
    [SerializeField]
    private TextMeshProUGUI rankingText;

    // シーン開始時にランキングを読み込んで表示
    void Start()
    {
        Display();
    }

    void Display()
    {
        // RankingManagerからスコアのリストを取得
        List<int> scores = RankingManager.GetRanking();

        // StringBuilderを使って効率的に文字列を連結する
        StringBuilder sb = new StringBuilder("--- HIGH SCORES ---\n");

        if (scores.Count == 0)
        {
            // まだスコアが保存されていない場合
            sb.AppendLine("No scores yet.");
        }
        else
        {
            // 保存されているスコアを一覧表示
            for (int i = 0; i < scores.Count; i++)
            {
                // 例: "1. 1500" のように表示
                sb.AppendLine($"{(i + 1)}. {scores[i]}");
            }
        }

        // 最後に、テキストコンポーネントに生成した文字列をセット
        rankingText.text = sb.ToString();
    }
}