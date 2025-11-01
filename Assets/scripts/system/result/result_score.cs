using UnityEngine;
using TMPro; // TextMeshProを使うために必要

public class Result_score : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI score_txt;

    // ★★★ 破壊数を表示するためのテキスト参照を追加 ★★★
    [SerializeField]
    private TextMeshProUGUI targetsDestroyed_txt;

    void Start()
    {
        // ScoreManagerから最終スコアを読み出し、テキストに表示する
        score_txt.text = "Your score : " + ScoreManager.finalScore;

        // ★★★ ScoreManagerから破壊したターゲット数を読み出し、テキストに表示する処理を追加 ★★★
        targetsDestroyed_txt.text = "Targets Destroyed: " + ScoreManager.targetsDestroyed;
    }
}