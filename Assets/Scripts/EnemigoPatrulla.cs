using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    [Header("Configuracion de Patrulla")]
    public Transform puntoInicio;
    public Transform puntoFin;
    public float velocidadMovimiento = 2f;

    [Header("Orientacion")]
    [Tooltip("Si está activo, el enemigo siempre mirará hacia la izquierda (hacia el jugador). Si se desactiva, mira hacia donde camina.")]
    public bool mantenerMiradaIzquierda = false;

    private Transform puntoObjetivo;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Evita que caiga o se hunda
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (puntoFin != null)
            puntoObjetivo = puntoFin;
        else
            puntoObjetivo = transform;
    }

    private void FixedUpdate()
    {
        if (puntoInicio == null || puntoFin == null) return;

        // Movimiento horizontal estable
        Vector2 nuevaPosicion = Vector2.MoveTowards(rb.position, new Vector2(puntoObjetivo.position.x, rb.position.y), velocidadMovimiento * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPosicion);

        // Cambiar de objetivo al llegar cerca
        if (Vector2.Distance(transform.position, new Vector2(puntoObjetivo.position.x, transform.position.y)) < 0.2f)
        {
            CambiarDeObjetivo();
        }

        ActualizarOrientacion();
    }

    private void CambiarDeObjetivo()
    {
        puntoObjetivo = (puntoObjetivo == puntoFin) ? puntoInicio : puntoFin;
    }

    private void ActualizarOrientacion()
    {
        if (mantenerMiradaIzquierda)
        {
            // Forzamos que siempre mire hacia la izquierda (o derecha según prefieras)
            // Asumiendo que el sprite original mira hacia la izquierda por defecto:
            spriteRenderer.flipX = false;
        }
        else
        {
            // Dependiendo de hacia dónde se esté moviendo, volteamos el sprite con flipX
            // sin alterar el Scale del objeto (evita deformaciones)
            if (puntoObjetivo.position.x > transform.position.x)
            {
                // Se mueve a la derecha -> volteamos el sprite
                spriteRenderer.flipX = true;
            }
            else
            {
                // Se mueve a la izquierda -> sprite normal
                spriteRenderer.flipX = false;
            }
        }
    }
}