using UnityEngine;
using TMPro; // TextMeshProを使うために必要

public class result_time : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI time_txt;

    void Start()
    {
        // ScoreManagerが記憶している生存時間を読み出し、テキストに表示する
        // .ToString("F2") は、小数点以下2桁まで表示するための書式設定
        // time_txt.text = "You survived: " + ScoreManager.survivalTime.ToString("F2") + "s";
    }
}