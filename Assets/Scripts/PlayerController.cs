using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 10f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _mirandoDerecha = true;
    private bool _enSuelo = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. Movimiento Horizontal (Siempre activo sin importar el suelo)
        float movimientoH = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(movimientoH * velocidadMovimiento, _rb.linearVelocity.y);

        // 2. Animación de correr
        bool estaMoviendose = movimientoH != 0;
        _animator.SetBool("Corriendo", estaMoviendose);

        // 3. Volteo del sprite (FlipX)
        if (movimientoH > 0 && !_mirandoDerecha)
        {
            _spriteRenderer.flipX = false;
            _mirandoDerecha = true;
        }
        else if (movimientoH < 0 && _mirandoDerecha)
        {
            _spriteRenderer.flipX = true;
            _mirandoDerecha = false;
        }

        // 4. Salto simplificado
        if (Input.GetButtonDown("Jump") && _enSuelo)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, fuerzaSalto);
            _enSuelo = false;
        }
    }

    // Detección de suelo simplificada por contacto de superficies
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Detecta cualquier objeto plano o plataforma como suelo válido
        _enSuelo = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _enSuelo = false;
    }
}