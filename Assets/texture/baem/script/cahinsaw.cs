using UnityEngine;

public class cahinsaw : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject beam_obj;
    private float cur_time = 0;
    private float pos_y;

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

        if (cur_time >= setup_sec && setup_bool == true)
        {
            cur_time = 0;
            Debug.Log("ビーム開始");
            pos_y = UnityEngine.Random.Range(-4.0f, 12.0f);
            this.transform.localPosition = new Vector3(-12.0f, pos_y, -3.0f);
            beam_obj.transform.position = new Vector3(0, pos_y - 3.5f, -3.1f);

            setup_bool = false;
            spin_bool = true;
        }

        //普通のスピン遷移
        if (cur_time >= spin_sec && spin_bool == true)
        {
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
            //攻撃のスクリプト書く
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
            }
        }
    }
}
