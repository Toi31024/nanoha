using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timer_text;
    float time = 0;
    void Start()
    {

    }

    void Update()
    {
        time += Time.deltaTime;
        timer_text.text = time.ToString("F2");
    }
}
