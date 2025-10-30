using UnityEngine;

public class cahinsaw : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject beam_obj;
    [SerializeField] private GameObject alert;
    private float cur_time = 0;
    private float pos_y;

    private int how_many_times = 0;

    private bool setup_bool = true;
    private float setup_sec = 2.0f;

    private bool spin_bool = false;
    private float spin_sec = 0.5f;

    private bool super_spin_bool = false;
    private float super_spin_sec = 1.5f;

    private bool attack_bool = false;
    private float attack_sec = 1.5f;
    private float attack_end_sec = 2.05f;

    void Update()
    {
        cur_time += Time.deltaTime;

        //ビームガンの位置を決定
        if (cur_time >= setup_sec && setup_bool == true)
        {
            cur_time = 0;
            pos_y = UnityEngine.Random.Range(-4.0f, 12.0f);
            this.transform.localPosition = new Vector3(-12.0f, pos_y, -3.0f);
            beam_obj.transform.position = new Vector3(2.2f, pos_y - 3.4f, -3.1f);
            alert.transform.position = new Vector3(0, pos_y - 3.885f, -2.9f);

            setup_bool = false;
            spin_bool = true;
        }

        //普通のスピン遷移
        if (cur_time >= spin_sec && spin_bool == true)
        {
            alert.SetActive(true);

            anim.SetBool("spin", true);
            anim.SetBool("superspin", false);
            anim.SetBool("wait", false);

            spin_bool = false;
            super_spin_bool = true;

            cur_time = 0;
        }

        //スーパースピン遷移
        if (cur_time >= super_spin_sec && super_spin_bool == true)
        {
            anim.SetBool("spin", false);
            anim.SetBool("superspin", true);
            anim.SetBool("wait", false);

            super_spin_bool = false;
            attack_bool = true;

            cur_time = 0;
        }
        
        //攻撃遷移
        if (cur_time >= attack_sec && attack_bool == true)
        {
            alert.SetActive(false);
            beam_obj.SetActive(true);
            if (cur_time >= attack_end_sec)
            {
                beam_obj.SetActive(false);

                anim.SetBool("spin", false);
                anim.SetBool("superspin", false);
                anim.SetBool("wait", true);

                attack_bool = false;
                setup_bool = true;

                cur_time = 0;

                //時間がたつほど、ビームの頻度が増えるようにする(難易度増加)
                how_many_times++;
                if (how_many_times >= 3 && how_many_times < 7)
                {
                    Debug.Log("難易度増加1");
                    setup_sec = 1.5f;
                    super_spin_sec = 1.0f;
                    attack_sec = 1.2f;
                    attack_end_sec = 1.75f;
                }
                if (how_many_times >= 7 && how_many_times < 14)
                {
                    Debug.Log("難易度増加2");
                    setup_sec = 1.0f;
                    spin_sec = 0.25f;
                    super_spin_sec = 0.75f;
                    attack_sec = 0.9f;
                    attack_end_sec = 1.45f;
                }
            }
        }
    }
}
