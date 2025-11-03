using UnityEngine;
using TMPro; // TextMeshProを使うために必要

public class Result_score : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI score_txt;

    [SerializeField]
    private TextMeshProUGUI targetsDestroyed_txt;

    void Start()
    {
        // ScoreManagerから最終スコアを読み出し、テキストに表示する
        score_txt.text = "Your score : " + ScoreManager.finalScore;

        // ScoreManagerから破壊したターゲット数を読み出し、テキストに表示する
        targetsDestroyed_txt.text = "Targets Destroyed: " + ScoreManager.targetsDestroyed;

        // ★★★★★ ここから追加 ★★★★★
        // プレイ結果のスコアをランキングに追加する
        // (ScoreManager.finalScoreがint型であることを想定)
        RankingManager.AddScore(ScoreManager.finalScore);
        // ★★★★★ ここまで追加 ★★★★★
    }
}