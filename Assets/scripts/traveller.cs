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
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        traveller_InputAction = new Traveller_controller();
    }

    private void OnEnable()
    {
        traveller_InputAction.Player.Move.Enable();
    }

    private void OnDisable()
    {
        traveller_InputAction.Player.Move.Disable();
    }

    void Update()
    {
        moveInput = traveller_InputAction.Player.Move.ReadValue<Vector2>();

        // アニメーションとキャラクターの向きを更新する処理
        UpdateVisuals();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        EnforceBounds();
    }

    private void MovePlayer()
    {
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
        // --- アニメーションの切り替え ---
        // moveInputのベクトルの長さが0より大きい（＝入力がある）か判定
        if (moveInput.sqrMagnitude > 0.01f)
        {
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }

        // --- キャラクターの向きの反転 ---
        // 水平方向の入力があり、かつ現在の向きと入力方向が違う場合に反転させる
        if (moveInput.x > 0 && !isFacingRight)
        {
            // 右入力 かつ 現在左向きなら、右を向かせる
            Flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            // 左入力 かつ 現在右向きなら、左を向かせる
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
}