using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class traveller : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("移動範囲の制限")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    [Header("バニシングステップ設定")]
    [SerializeField] private float dashDistance = 3f; // 移動する距離
    [SerializeField] private float dashDuration = 0.2f; // 移動にかかる時間（この値が小さいほど速い）
    [SerializeField] private float dashCooldown = 1f; // 使用後のクールダウン時間

    [Header("ステータス設定")]
    [SerializeField] public int maxHp = 5;
    public int currentHp;

    [Header("ダメージ設定")]
    [SerializeField] private float invincibilityDuration = 1.5f; // ダメージ後の無敵時間
    [SerializeField] private float stunDuration = 0.5f; // ダメージ時の硬直時間（移動停止）
    private bool isStunned = false; // 硬直中かどうかのフラグ
    private bool isInvincible = false; // 無敵中かどうかのフラグ
    private SpriteRenderer spriteRenderer; // 点滅させるためのスプライトレンダラー

    [Header("攻撃設定")]
    [SerializeField] private GameObject attackHitbox;

    [Header("攻撃判定の形")]
    [SerializeField] private Vector2 horizontalHitboxSize = new Vector2(2, 1); // 横攻撃のサイズ
    [SerializeField] private Vector2 verticalHitboxSize = new Vector2(1, 2);   // 縦攻撃のサイズ

    [Header("攻撃判定の位置オフセット")]
    [SerializeField] private Vector2 rightAttackOffset = new Vector2(1, 0);   // 右攻撃の位置
    [SerializeField] private Vector2 leftAttackOffset = new Vector2(-1, 0);  // 左攻撃の位置
    [SerializeField] private Vector2 upAttackOffset = new Vector2(0, 1);     // 上攻撃の位置
    [SerializeField] private Vector2 downAttackOffset = new Vector2(0, -1);   // 下攻撃の位置
    private BoxCollider2D attackBoxCollider; // ヒットボックスのコライダーを保持する変数
    private Rigidbody2D rb;
    private Animator anim;
    private Traveller_controller traveller_InputAction;
    private Vector2 moveInput;

    // キャラクターの向きを保持するための変数
    private Vector2 lastMoveDirection; // 最後に移動した方向を記憶
    private bool isFacingRight = true;
    private bool isAttacking = false; // 攻撃中かどうかを判定するフラグ
    private bool isDashing = false; // ダッシュ中かどうかのフラグ
    private float dashCooldownTimer = 0f; // クールダウンタイマー
    public GameObject gameOverCanvas; // ゲームオーバー画面のCanvas
    private float survivalTimer = 0f; // 生存時間を計測するタイマー
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // スプライトレンダラーを取得
        traveller_InputAction = new Traveller_controller();
        // ヒットボックスのBoxCollider2Dを取得
        attackBoxCollider = attackHitbox.GetComponent<BoxCollider2D>();
        // Attackアクションが実行されたとき(ボタンが押されたとき)にAttackメソッドを呼び出す
        traveller_InputAction.Player.Fire.performed += context => Attack();
        // --- Dashアクションの受付を追加 ---
        traveller_InputAction.Player.Vanishing_step.performed += context => Dash();
    }

    private void OnEnable()
    {
        traveller_InputAction.Player.Move.Enable();
        traveller_InputAction.Player.Fire.Enable(); // Attackアクションも有効化
        traveller_InputAction.Player.Vanishing_step.Enable(); // Dashアクションも有効化
    }

    private void OnDisable()
    {
        traveller_InputAction.Player.Move.Disable();
        traveller_InputAction.Player.Fire.Disable(); // Attackアクションも無効化
        traveller_InputAction.Player.Vanishing_step.Disable(); // Dashアクションも無効化
    }

    void Start()
    {
        // ゲーム開始時のデフォルトの向きを下に設定
        lastMoveDirection = Vector2.down;
        currentHp = maxHp; // HPを最大値で初期化
        Debug.Log("ゲーム開始時のHP: " + currentHp);
    }
    void Update()
    {
        // 攻撃中,バニシングステップ中でなければ入力を受け付ける
        if (!isAttacking && !isDashing && !isStunned)
        {
            moveInput = traveller_InputAction.Player.Move.ReadValue<Vector2>();
        }
        // 生存時間を更新
        if (!isDashing && !isAttacking)
        {
            survivalTimer += Time.deltaTime;
        }

        UpdateVisuals();

        // 移動入力がある場合、最後の移動方向を更新
        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInput.normalized;
        }
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            MovePlayer();
        }
        EnforceBounds();
    }

    private void MovePlayer()
    {
        // 攻撃中、無敵中は移動を停止する
        if (isAttacking || isStunned)
        {
            rb.linearVelocity = Vector2.zero;
            return; // これ以降の処理はしない
        }
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    private void EnforceBounds()
    {
        Vector2 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);
        transform.position = clampedPosition;
    }

    // アニメーション＆反転処理 
    private void UpdateVisuals()
    {
        anim.SetBool("run", moveInput.sqrMagnitude > 0.01f && !isAttacking);

        // 左右の反転処理（攻撃中も向きは維持）
        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // ★★★ キャラクターを反転させるためのメソッド ★★★
    private void Flip()
    {
        // 現在の向きを反転させる
        isFacingRight = !isFacingRight;

        // transform.localScaleのxを反転させる
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    // --- ここからが新しい攻撃処理 ---
    private void Attack()
    {
        // 攻撃中は何もしない
        if (isAttacking || isDashing) return;

        isAttacking = true;
        moveInput = Vector2.zero; // 攻撃開始時に入力をゼロにして、runアニメーションを止める

        // 最後に移動していた方向に基づいてAttackDirectionを設定
        // 横方向の入力が大きい場合
        if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
        {
            anim.SetInteger("AttackDirection", 2); // 2: 横攻撃

            // 向きを合わせる
            if (lastMoveDirection.x > 0 && !isFacingRight) Flip();
            else if (lastMoveDirection.x < 0 && isFacingRight) Flip();
        }
        // 縦方向の入力が大きい場合
        else
        {
            // 上方向
            if (lastMoveDirection.y > 0)
            {
                anim.SetInteger("AttackDirection", 1); // 1: 上攻撃
            }
            // 下方向
            else
            {
                anim.SetInteger("AttackDirection", 3); // 3: 下攻撃
            }
        }

        // Attackトリガーを起動してアニメーションを開始
        anim.SetTrigger("Attack");
    }

    // アニメーションイベントから呼び出すためのメソッド
    public void OnAttackAnimationFinished()
    {
        isAttacking = false;
    }
    // Dashボタンが押されたときに呼び出されるメソッド
    private void Dash()
    {
        // 攻撃中、ダッシュ中、クールダウン中は発動しない
        if (isAttacking || isDashing || dashCooldownTimer > 0) return;

        // ダッシュ処理の本体であるコルーチンを開始
        StartCoroutine(DashCoroutine());
    }

    //Hitboxの有効化
    // Animation Eventから呼び出す：ヒットボックスを有効化・位置調整する
    public void ActivateHitbox()
    {
        // 最後に移動していた方向が横方向か判定
        if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
        {
            // 横攻撃用の形を設定
            attackBoxCollider.size = horizontalHitboxSize;

            // キャラクターの向きに応じて、右か左の位置オフセットを適用
            if (isFacingRight)
            {
                attackBoxCollider.offset = rightAttackOffset;
            }
            else
            {
                attackBoxCollider.offset = leftAttackOffset;
            }
        }
        else // 縦方向の場合
        {
            // 縦攻撃用の形を設定
            attackBoxCollider.size = verticalHitboxSize;

            // 向きに応じて、上か下の位置オフセットを適用
            if (lastMoveDirection.y > 0)
            {
                attackBoxCollider.offset = upAttackOffset;
            }
            else
            {
                attackBoxCollider.offset = downAttackOffset;
            }
        }

        // 全ての設定が終わったら、ヒットボックスを有効化
        attackHitbox.SetActive(true);
    }


    //Animation Eventから呼び出す：ヒットボックスを無効化する
    public void DeactivateHitbox()
    {
        attackHitbox.SetActive(false);
    }
    // バニシングステップの本体処理（IEnumerator型）
    private IEnumerator DashCoroutine()
    {
        // 1. ダッシュ開始時の設定
        isDashing = true;
        dashCooldownTimer = dashCooldown; // クールダウンタイマーを開始
        anim.SetTrigger("Dash"); // ダッシュアニメーションを再生
        moveInput = Vector2.zero; // 移動入力を止める

        // 2. 移動先の計算
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + lastMoveDirection * dashDistance;

        // 移動先が移動範囲を超えないようにClampする
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

        // 3. 高速移動の実行
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            // 現在地から目的地までを線形補間して移動させる
            rb.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsedTime / dashDuration));
            elapsedTime += Time.fixedDeltaTime; // 物理更新の時間で進める
            yield return new WaitForFixedUpdate(); // 次の物理更新まで待つ
        }
        // 念のため、最終位置に正確に移動させる
        rb.MovePosition(targetPosition);

        // 4. ダッシュ終了時の設定
        isDashing = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. まず、何かに衝突したかを確認
        Debug.Log("何かに衝突した！ 相手の名前: " + other.gameObject.name);
        if (other.CompareTag("DamageObject"))
        {
            AttackData attack = other.gameObject.GetComponent<AttackData>();

            // 2. 衝突した相手のタグが "DamageObject" かどうかを確認
            if (attack != null)
            {
                Debug.Log(other.name + "からダメージを受けます。スタン：" + attack.causesStun);
                TakeDamage(attack.damage, attack.causesStun); // 1ダメージを受ける
            }
            else
            {
                Debug.Log(other.name + "はDamageObjectタグですがAttackDataがありません。" + other.gameObject.tag);
                TakeDamage(1, false); // スタンせず一ダメージを受ける
            }
        }
    }

    // ダメージを受ける処理をまとめたメソッド
    public void TakeDamage(int damage, bool causesStun)
    {
// 無敵時間中はダメージを受けない
        if (isInvincible) return;

        currentHp -= damage;
        Debug.Log("プレイヤーがダメージを受けた！ 現在のHP: " + currentHp);

        // HPが0より大きい場合はダメージモーションと無敵処理
        if (currentHp > 0)
        {
            //anim.SetTrigger("Hurt");
            
            // コルーチンにもスタン情報を渡す
            StartCoroutine(DamageEffectCoroutine(causesStun)); // ◀◀ 修正後
        }
        // HPが0以下の場合は死亡処理
        else
        {
            Die();
        }
    }

// 無敵時間と点滅を管理するコルーチン
    private IEnumerator DamageEffectCoroutine(bool causesStun)
    {
        // 1. 無敵と硬直（必要なら）を開始
        isInvincible = true;
        if (causesStun)
        {
            isStunned = true;
            moveInput = Vector2.zero;
            anim.SetBool("Hurt stun", true);
        }
        else
        {
            anim.SetTrigger("Hurt");
        }

        // 2. 点滅処理とスタン解除のタイマーを並行して開始
        float flashTimer = 0f; // 点滅と無敵時間を計測するタイマー
        float stunTimer = 0f;  // スタン時間を計測するタイマー
        bool isStunOver = !causesStun; // スタンしない攻撃なら最初から true

        // 3. 無敵時間 (invincibilityDuration) が終わるまでループ
        while (flashTimer < invincibilityDuration)
        {
            // --- 点滅処理 ---
            spriteRenderer.enabled = !spriteRenderer.enabled;
            
            // --- 待機時間を計算 (点滅間隔は0.1秒) ---
            float waitTime = 0.1f;
            
            // 残り無敵時間が0.1秒未満なら、待機時間を残り時間にする
            if (invincibilityDuration - flashTimer < waitTime)
            {
                waitTime = invincibilityDuration - flashTimer;
            }

            // --- タイマーを進める ---
            flashTimer += waitTime; // 無敵タイマー
            
            // --- スタン解除処理（まだスタン中の場合） ---
            if (!isStunOver)
            {
                stunTimer += waitTime; // スタンタイマー
                
                // スタン時間が経過したら
                if (stunTimer >= stunDuration)
                {
                    // スタンとアニメーションを解除
                    isStunOver = true;
                    isStunned = false;
                    anim.SetBool("Hurt stun", false);
                }
            }
            
            // --- 待機 ---
            if (waitTime > 0.001f) // ほぼ時間が残ってないなら待たない
            {
                yield return new WaitForSeconds(waitTime);
            }
        }

        // 4. 無敵時間が終わったら、必ず元に戻す
        spriteRenderer.enabled = true; // 点滅終了
        isInvincible = false;        // 無敵終了
        
        // (万が一、無敵時間よりスタン時間が長かった場合のために、ここで再度解除)
        if (isStunned)
        {
            isStunned = false;
            anim.SetBool("Hurt stun", false);
        }
    }
    // 死亡時の処理
    private void Die()
    {
        Debug.Log("プレイヤーは力尽きた...");
        // ここにゲームオーバー処理などを書く（例：オブジェクトを非表示にする）
        // スコアマネージャーに最終的な生存時間を渡してスコアを計算させる
        ScoreManager.CalculateFinalScore(survivalTimer);
        Time.timeScale = 0.2f;
        // ゲームオーバー画面を有効化する（これによりアニメーションが再生開始される）
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
