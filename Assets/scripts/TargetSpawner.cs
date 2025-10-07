using System.Collections;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("ターゲット設定")]
    [SerializeField] private GameObject targetPrefab; // スポーンさせるターゲットのプレハブ
    [SerializeField] private float spawnInterval = 30f; // スポーンさせる間隔（秒）

    [Header("スポーン範囲")]
    [SerializeField] private Vector2 spawnAreaMin; // スポーン範囲の左下の座標
    [SerializeField] private Vector2 spawnAreaMax; // スポーン範囲の右上の座標

    // 現在シーン上にいるターゲットを記憶しておくための変数
    private GameObject currentTargetInstance;

    void Start()
    {
        // ゲームが開始したら、ターゲットのスポーン処理を開始する
        StartCoroutine(SpawnTargetRoutine());
        // 3. 新しいターゲットをスポーンさせるランダムな位置を決める
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);
        // 4. プレハブから新しいターゲットを生成し、その情報を記憶しておく
        currentTargetInstance = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        
        Debug.Log("新しいターゲットをスポーンしました。 位置: " + spawnPosition);
    }

    private IEnumerator SpawnTargetRoutine()
    {
        // ゲーム中、この処理を無限に繰り返す
        while (true)
        {
            // 1. 指定された時間（30秒）待つ
            yield return new WaitForSeconds(spawnInterval);

            // 2. もし古いターゲットがまだ存在していたら、それを破壊（デスポーン）する
            if (currentTargetInstance != null)
            {
                Destroy(currentTargetInstance);
            }

            // 3. 新しいターゲットをスポーンさせるランダムな位置を決める
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            // 4. プレハブから新しいターゲットを生成し、その情報を記憶しておく
            currentTargetInstance = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
            
            Debug.Log("新しいターゲットをスポーンしました。 位置: " + spawnPosition);
        }
    }
}