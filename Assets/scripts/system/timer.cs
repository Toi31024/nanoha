using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timer_text;
    public static float time = 0;

    private float timer_senyou_time;
    void Start()
    {
        timer_senyou_time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        timer_senyou_time += Time.deltaTime;
        timer_text.text = timer_senyou_time.ToString("F2");
    }
}
