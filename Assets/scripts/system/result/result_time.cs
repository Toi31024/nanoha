using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class result_time : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI time_txt;
    void Awake()
    {
        time_txt.text = "You survived " + timer.time.ToString("F2");
    }
}
