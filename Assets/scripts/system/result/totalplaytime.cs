using UnityEngine;
using TMPro;

public class totalplaytime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI total_timer_txt;
    void Start()
    {
        
    }

    void Update()
    {
        total_timer_txt.text = "total play time: " + timer.time.ToString("F2");
    }
}
