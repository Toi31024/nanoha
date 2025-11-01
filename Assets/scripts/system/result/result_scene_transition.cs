using UnityEngine;
using UnityEngine.SceneManagement;

public class result_scene_transition : MonoBehaviour
{
    [SerializeField] GameObject target_txt;
    [SerializeField] GameObject time_txt;
    [SerializeField] GameObject score_txt;
    [SerializeField] GameObject press_to_title_txt;

    private float cur_time = 0;
    private float presta_time;

    void Update()
    {
        cur_time += Time.deltaTime;

        if (cur_time >= 0.8f)
        {
            target_txt.SetActive(true);
        }

        if (cur_time >= 1.3f)
        {
            time_txt.SetActive(true);
        }

        if (cur_time >= 1.8f)
        {
            score_txt.SetActive(true);
        }

        if (cur_time >= 2.3f)
        {
            press_to_title_txt.SetActive(true);
            presta_time += Time.deltaTime;
        }


        if (cur_time >= 2.3f && presta_time < 0.5f)
        {
            press_to_title_txt.SetActive(true);
        }

        else
        {
            press_to_title_txt.SetActive(false);
            if (presta_time > 1.0f)
            {
                presta_time = 0;
            }
        }

        //タイトル画面へ
        if (cur_time >= 2.6)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("TitleScean");
            }
        }
    }
}
