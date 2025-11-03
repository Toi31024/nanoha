using UnityEngine;
using TMPro;

public class totalplaytime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI total_timer_txt;
    [SerializeField] GameObject th;
    void Start()
    {
        
    }

    void Update()
    {
        total_timer_txt.text = "total play time: " + timer.time.ToString("F2");
        th.SetActive(true);
    }
}
