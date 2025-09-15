using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    [SerializeField] private int maxHp = 5;
    private int currentHp;

    [Header("ダメージ設定")]
    [SerializeField] private float invincibilityDuration = 1.5f; // ダメージ後の無敵時間
    private bool isInvincible = false; // 無敵中かどうかのフラグ
    private SpriteRenderer spriteRenderer; // 点滅させるためのスプライトレンダラー
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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // スプライトレンダラーを取得
        traveller_InputAction = new Traveller_controller();
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
        if (!isAttacking && !isDashing && !isInvincible)
        {
            moveInput = traveller_InputAction.Player.Move.ReadValue<Vector2>();
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
        if (isAttacking || isInvincible)
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
    // 1. まず、何かに衝突したかを確認
    Debug.Log("何かに衝突した！ 相手の名前: " + collision.gameObject.name);

    // 2. 衝突した相手のタグが "DamageObject" かどうかを確認
    if (collision.gameObject.CompareTag("DamageObject"))
    {
        Debug.Log("ダメージオブジェクトに衝突した！ TakeDamageを呼び出します。");
        TakeDamage(1); // 1ダメージを受ける
    }
    else
    {
        Debug.Log("衝突した相手はダメージオブジェクトではありませんでした。相手のタグ: " + collision.gameObject.tag);
    }
    }

    // ダメージを受ける処理をまとめたメソッド
    public void TakeDamage(int damage)
    {
        // 無敵時間中はダメージを受けない
        if (isInvincible) return;

        currentHp -= damage;
        Debug.Log("プレイヤーがダメージを受けた！ 現在のHP: " + currentHp);

        // HPが0より大きい場合はダメージモーションと無敵処理
        if (currentHp > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(InvincibilityCoroutine());
        }
        // HPが0以下の場合は死亡処理
        else
        {
            Die();
        }
    }

    // 無敵時間と点滅を管理するコルーチン
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // 指定された無敵時間の間、点滅を繰り返す
        float timer = 0f;
        while (timer < invincibilityDuration)
        {
            // スプライトの表示・非表示を切り替える
            spriteRenderer.enabled = !spriteRenderer.enabled;
            
            // 0.1秒待つ
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        // 無敵時間が終わったら、必ず表示状態に戻してフラグを解除
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    // 死亡時の処理
    private void Die()
    {
        Debug.Log("プレイヤーは力尽きた...");
        // ここにゲームオーバー処理などを書く（例：オブジェクトを非表示にする）
        gameObject.SetActive(false);
    }
}
