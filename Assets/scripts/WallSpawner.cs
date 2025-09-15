using System.Collections;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject wallPrefab; // 生成する壁のプレハブ
    [SerializeField] private float spawnInterval = 1f; // 壁を生成する間隔（秒）
    
    [Header("X軸の生成範囲")]
    [SerializeField] private float spawnXRange = 8f; // この範囲のX座標にランダムで生成 (-8fから8fまで)

    [Header("Y軸の生成位置")]
    [SerializeField] private float spawnYPositionTop = 10f; // 画面の上に生成する場合のY座標
    [SerializeField] private float spawnYPositionBottom = -10f; // 画面の下に生成する場合のY座標
    
    void Start()
    {
        // ゲームが始まったら、壁の生成を開始する
        StartCoroutine(SpawnWalls());
    }

    // 壁を生成し続けるコルーチン
    private IEnumerator SpawnWalls()
    {
        // このループはゲーム中ずっと続く
        while (true)
        {
            // 1. 次の壁を生成するまで待つ
            yield return new WaitForSeconds(spawnInterval);

            // 2. 壁を生成する座標と移動方向を決める
            float randomX = Random.Range(-spawnXRange, spawnXRange); // X座標をランダムに決める
            Vector2 spawnPosition;
            Vector2 moveDirection;
            
            // 50%の確率で上から生成するか、下から生成するかを決める
            if (Random.value > 0.5f)
            {
                // 上から下へ動く壁
                spawnPosition = new Vector2(randomX, spawnYPositionTop);
                moveDirection = Vector2.down;
            }
            else
            {
                // 下から上へ動く壁
                spawnPosition = new Vector2(randomX, spawnYPositionBottom);
                moveDirection = Vector2.up;
            }

            // 3. プレハブから新しい壁オブジェクトをインスタンス化（生成）
            GameObject newWall = Instantiate(wallPrefab, spawnPosition, Quaternion.identity);

            // 4. 生成した壁の移動方向と速さを設定
            WallMovement wallMovement = newWall.GetComponent<WallMovement>();
            if (wallMovement != null)
            {
                wallMovement.moveDirection = moveDirection;
                // ここで速さをランダムに変えるなどの応用も可能
                // wallMovement.speed = Random.Range(4f, 8f); 
            }
        }
    }
}