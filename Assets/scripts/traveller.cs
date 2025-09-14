using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class traveller : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("移動範囲の制限")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Rigidbody2D rb;
    private Animator anim;
    private Traveller_controller traveller_InputAction;
    private Vector2 moveInput;

    // キャラクターの向きを保持するための変数
    private Vector2 lastMoveDirection; // 最後に移動した方向を記憶
    private bool isFacingRight = true;
    private bool isAttacking = false; // 攻撃中かどうかを判定するフラグ

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        traveller_InputAction = new Traveller_controller();
        // Attackアクションが実行されたとき(ボタンが押されたとき)にAttackメソッドを呼び出す
        traveller_InputAction.Player.Fire.performed += context => Attack();
    }

    private void OnEnable()
    {
        traveller_InputAction.Player.Move.Enable();
        traveller_InputAction.Player.Fire.Enable(); // Attackアクションも有効化
    }

    private void OnDisable()
    {
        traveller_InputAction.Player.Move.Disable();
        traveller_InputAction.Player.Fire.Disable(); // Attackアクションも無効化
    }

    void Start()
    {
        // ゲーム開始時のデフォルトの向きを下に設定
        lastMoveDirection = Vector2.down;
    }
    void Update()
    {
        // 攻撃中でなければ入力を受け付ける
        if (!isAttacking)
        {
            moveInput = traveller_InputAction.Player.Move.ReadValue<Vector2>();
        }
        
        UpdateVisuals();
        
        // 移動入力がある場合、最後の移動方向を更新
        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInput.normalized;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        EnforceBounds();
    }

    private void MovePlayer()
    {
        // 攻撃中は移動を停止する
        if (isAttacking)
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
        if (isAttacking) return;
        
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
}