// このスクリプトはGameObjectにアタッチしません
public static class ScoreManager
{
    // ゲームの結果を記憶しておくための静的変数
    public static float survivalTime { get; private set; }
    public static int targetsDestroyed { get; private set; }
    public static int finalScore { get; private set; }
    public static bool isScoreSaved { get; private set; }

    // ゲーム開始時に呼ばれ、すべての記録をリセットする
    public static void Reset()
    {
        survivalTime = 0f;
        targetsDestroyed = 0;
        finalScore = 0;
        isScoreSaved = false; // 保存フラグをリセット
    }

    // isScoreSavedをtrueにするメソッド
    public static void MarkScoreAsSaved()
    {
        isScoreSaved = true;
    }

    // ターゲットが破壊されたときに呼ばれる
    public static void AddTargetDestroyed()
    {
        targetsDestroyed++;
    }

    // プレイヤーが死亡したときに呼ばれ、最終スコアを計算・保存する
    public static void CalculateFinalScore(float time)
    {
        survivalTime = time;
        
        // スコア計算式（この倍率は自由に調整してください）
        finalScore = (int)(survivalTime * 100) + (targetsDestroyed * 250);
    }
}