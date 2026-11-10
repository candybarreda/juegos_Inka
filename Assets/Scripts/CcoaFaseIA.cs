using UnityEngine;

public class CcoaFaseIA : MonoBehaviour
{
    public enum FaseEnemigo { Caminando, Volando }
    public FaseEnemigo faseActual = FaseEnemigo.Caminando;

    [Header("Referencias")]
    public Transform jugador;
    public GameObject prefabPiedra;
    public Transform puntoDisparo;
    public Transform detectorSuelo;
    public LayerMask capaSuelo;

    [Header("Configuración de Movimiento")]
    public float velocidadTierra = 2f;
    public float velocidadAire = 3.5f;
    public float distanciaVision = 10f;
    public float distanciaAtaque = 4f;
    public float alturaVuelo = 3f;

    [Header("Configuración de la Parábola")]
    public float alturaCurvaTierra = 2f;
    public float alturaCurvaAire = 1.5f;
    public float gravedadPiedra = 3f;

    [Header("Control de Tiempos y Fases")]
    public float tiempoMaximoTierra = 5f;
    private float cronometroTierra = 0f;
    public float tiempoEntreAtaques = 2.5f;
    private float siguienteAtaque = 0f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (jugador == null)
        {
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
        }

        faseActual = FaseEnemigo.Caminando;
        cronometroTierra = 0f;

        if (rb != null)
        {
            rb.gravityScale = 1f;
        }

        if (animator != null)
        {
            animator.SetBool("Volando", false);
            animator.SetBool("Caminando", true);
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaJugador <= distanciaVision)
        {
            VerificarOrientacion();

            // --- FASE 1: CAMINANDO / TIERRA ---
            if (faseActual == FaseEnemigo.Caminando)
            {
                // Solo sumamos tiempo si está en tierra firme
                cronometroTierra += Time.deltaTime;

                bool haySuelo = true;
                if (detectorSuelo != null)
                {
                    haySuelo = Physics2D.OverlapCircle(detectorSuelo.position, 0.3f, capaSuelo);
                }

                // SI PASAN LOS 5 SEGUNDOS O LLEGA AL VACÍO (!haySuelo), CAMINA -> VUELA
                if (cronometroTierra >= tiempoMaximoTierra || !haySuelo)
                {
                    CambiarAFaseVoladora();
                    return;
                }

                if (distanciaJugador > distanciaAtaque)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(jugador.position.x, transform.position.y), velocidadTierra * Time.deltaTime);
                    if (animator != null)
                    {
                        animator.SetBool("Caminando", true);
                        animator.SetBool("Volando", false);
                    }
                }
                else
                {
                    if (animator != null)
                    {
                        animator.SetBool("Caminando", false);
                        animator.SetBool("Volando", false);
                    }

                    if (Time.time >= siguienteAtaque)
                    {
                        LanzarPiedraTierraParabola();
                        siguienteAtaque = Time.time + tiempoEntreAtaques;
                    }
                }
            }
            // --- FASE 2: VOLANDO ---
            else if (faseActual == FaseEnemigo.Volando)
            {
                Vector2 objetivoVuelo = new Vector2(jugador.position.x, jugador.position.y + alturaVuelo);
                transform.position = Vector2.MoveTowards(transform.position, objetivoVuelo, velocidadAire * Time.deltaTime);

                // FUERZA LA ANIMACIÓN DE VUELO EN EL ANIMATOR
                if (animator != null)
                {
                    animator.SetBool("Caminando", false);
                    animator.SetBool("Volando", true);
                }

                if (Time.time >= siguienteAtaque)
                {
                    LanzarPiedraAireParabola();
                    siguienteAtaque = Time.time + tiempoEntreAtaques;
                }
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("Caminando", false);
                animator.SetBool("Volando", false);
            }
        }
    }

    void CambiarAFaseVoladora()
    {
        faseActual = FaseEnemigo.Volando;

        if (animator != null)
        {
            animator.SetBool("Volando", true);
            animator.SetBool("Caminando", false);
        }

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
    }

    void LanzarPiedraTierraParabola()
    {
        // Activa la animación de ataque en tierra
        if (animator != null)
        {
            animator.SetTrigger("Atacar");
        }

        // ESTO ES LO QUE FALTABA: Llama a la función que calcula la parábola hacia el jugador
        InstanciarPiedraConCurva(alturaCurvaTierra);
    }

    void LanzarPiedraAireParabola()
    {
        InstanciarPiedraConCurva(alturaCurvaAire);
    }

    void InstanciarPiedraConCurva(float alturaCurva)
    {
        if (prefabPiedra != null && puntoDisparo != null)
        {
            GameObject piedra = Instantiate(prefabPiedra, puntoDisparo.position, Quaternion.identity);

            MovimientoPiedraParabola parabolaScript = piedra.GetComponent<MovimientoPiedraParabola>();
            if (parabolaScript != null)
            {
                parabolaScript.LanzarParabola(puntoDisparo.position, jugador.position, alturaCurva, gravedadPiedra);
            }
        }
    }

    void VerificarOrientacion()
    {
        if (jugador.position.x > transform.position.x && !mirandoDerecha)
            Girar();
        else if (jugador.position.x < transform.position.x && mirandoDerecha)
            Girar();
    }

    void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}