using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // アニメーションイベントからこのメソッドを呼び出す
    public void LoadResultsScene()
    {
        // 時間の流れを元に戻す（重要！）
        Time.timeScale = 1f;
        SceneManager.LoadScene("Result");
    }
}