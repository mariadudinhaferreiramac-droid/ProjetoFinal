using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _playerRigidbody2D;
    public float _playerSpeed;
    public Vector2 _playerDirection;

    private Animator _playerAnimator;
    private bool _playerFaceRight = true;
    private bool _isWalk;
    private bool _isAttacking = false;

    private bool isPaused;
    private bool isKnockedBack = false;

    public GameObject pausePanel;
    public string cena;

    private SpriteRenderer spriteRenderer;

    // Hitbox do ataque (ATIVADA pela animação)
    public GameObject attackHitbox;

    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    public int maxCombo = 2;
    public float comboCooldown = 1f;
    private int comboCounter = 0;
    private bool canAttack = true;

    void Start()
    {
        Time.timeScale = 1f;
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isPaused && !isKnockedBack)
        {
            PlayerMove();
            UpdateAnimator();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (canAttack)
                    PlayerAttack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseScreen();
    }

    public void PauseScreen()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void FixedUpdate()
    {
        _isWalk = (_playerDirection.x != 0 || _playerDirection.y != 0);

        if (!isKnockedBack)
        {
            _playerRigidbody2D.MovePosition(
                _playerRigidbody2D.position + _playerDirection.normalized * _playerSpeed * Time.fixedDeltaTime
            );
        }

        UpdateSortingOrder();
    }

    void PlayerMove()
    {
        if (_isAttacking)
        {
            _playerDirection = Vector2.zero;
            return;
        }

        _playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_playerDirection.x < 0 && _playerFaceRight)
            Flip();
        else if (_playerDirection.x > 0 && !_playerFaceRight)
            Flip();
    }

    void UpdateAnimator()
    {
        _playerAnimator.SetBool("isWalk", _isWalk);
    }

    void Flip()
    {
        _playerFaceRight = !_playerFaceRight;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    void PlayerAttack()
    {
        _isAttacking = true;
        _playerDirection = Vector2.zero;
        _playerAnimator.SetTrigger("isAttack");
        attackHitbox.SetActive(true);

        comboCounter++;
        canAttack = false;

        StartCoroutine(EndAttackAndHandleCombo());
    }

    IEnumerator EndAttackAndHandleCombo()
    {
        yield return new WaitForSeconds(0.5f);

        attackHitbox.SetActive(false);
        _isAttacking = false;

        if (comboCounter >= maxCombo)
        {
            yield return new WaitForSeconds(comboCooldown);
            comboCounter = 0;
        }

        canAttack = true;
    }

    void UpdateSortingOrder()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    public void ApplyKnockback(Vector2 direction)
    {
        StartCoroutine(KnockbackCoroutine(direction));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;
        _playerRigidbody2D.velocity = Vector2.zero;
        _playerRigidbody2D.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        _playerRigidbody2D.velocity = Vector2.zero;
        isKnockedBack = false;
    }
}
