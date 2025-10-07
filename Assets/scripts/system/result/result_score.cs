using UnityEngine;
using TMPro;

public class result_score : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI score_txt;
    void Awake()
    {
        score_txt.text = "Your score is " + score.game_score.ToString();
    }

}
