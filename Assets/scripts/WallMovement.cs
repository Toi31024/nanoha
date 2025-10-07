using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public float speed = 5f; // 壁が動く速さ
    public Vector2 moveDirection = Vector2.up; // 動く方向（スポナーから設定される）

    // 画面外に出てからオブジェクトを破壊するまでの猶予Y座標
    private float destroyYBoundary = 12f;

    void Update()
    {
    // このログがConsoleに表示されるか、また値がどうなっているかを確認します
    //Debug.Log("Wall Update - Speed: " + speed + ", Direction: " + moveDirection);

        // 指定された方向に、指定された速さで移動し続ける
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 壁が画面の上下の遥か外側に出たら、オブジェクトを破壊（Destroy）する
        // これをしないと、壁が無限に増え続けてゲームが重くなる
        if (transform.position.y > destroyYBoundary || transform.position.y < -destroyYBoundary)
        {
            Destroy(gameObject);
        }
    }
}