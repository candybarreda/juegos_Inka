using System.Collections;
using UnityEngine;

public class EnemigoMuerte : MonoBehaviour
{
    [Header("Configuracion de Combate")]
    public float fuerzaReboteJugador = 8f; // Impulso que recibe el jugador al pisarlo
    public int danoAlJugador = 1;          // Vida que le resta si te toca de lado

    private EnemigoPatrulla scriptPatrulla;
    private Collider2D miCollider;
    private SpriteRenderer spriteRenderer;
    private bool estaMuerto = false;

    private void Awake()
    {
        scriptPatrulla = GetComponent<EnemigoPatrulla>();
        miCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (estaMuerto) return;

        // Verificamos si el objeto que choca es el Jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rbJugador = collision.gameObject.GetComponent<Rigidbody2D>();

            // Evaluamos la dirección del choque a través del primer punto de contacto
            foreach (ContactPoint2D punto in collision.contacts)
            {
                // Si el punto de contacto viene desde ARRIBA de la cabeza del enemigo (normal.y < -0.5f)
                // O si el jugador venía cayendo (rbJugador.velocity.y < 0)
                if (punto.normal.y < -0.5f || (rbJugador != null && rbJugador.linearVelocity.y < -0.1f && punto.point.y > transform.position.y))
                {
                    // === EL JUGADOR PISÓ A LA JARCHAJA ===
                    PisarEnemigo(rbJugador);
                    return;
                }
            }

            // === SI NO FUE POR ARRIBA, LA JARCHAJA HACE DAÑO AL JUGADOR ===
            HacerDanoAlJugador(collision.gameObject);
        }
    }

    private void PisarEnemigo(Rigidbody2D rbJugador)
    {
        estaMuerto = true;

        // 1. Dar impulso hacia arriba al jugador (Rebote estilo Mario)
        if (rbJugador != null)
        {
            rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, fuerzaReboteJugador);
        }

        // 2. Desactivar la patrulla y colisiones del enemigo
        if (scriptPatrulla != null) scriptPatrulla.enabled = false;
        if (miCollider != null) miCollider.enabled = false;

        // 3. Animación simple de aplastado por código (Escala rápida)
        StartCoroutine(AnimacionMuerteAplastado());
    }

    private IEnumerator AnimacionMuerteAplastado()
    {
        // Se encoge verticalmente como aplastado
        Vector3 escalaInicial = transform.localScale;
        Vector3 escalaAplastada = new Vector3(escalaInicial.x * 1.2f, escalaInicial.y * 0.2f, escalaInicial.z);

        float tiempo = 0f;
        while (tiempo < 0.15f)
        {
            transform.localScale = Vector3.Lerp(escalaInicial, escalaAplastada, tiempo / 0.15f);
            tiempo += Time.deltaTime;
            yield return null;
        }

        // Parpadear / Desvanecer
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 3f;
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = alpha;
                spriteRenderer.color = c;
            }
            yield return null;
        }

        // Eliminar del juego
        Destroy(gameObject);
    }

    private void HacerDanoAlJugador(GameObject jugador)
    {
        SaludJugador salud = jugador.GetComponent<SaludJugador>();
        if (salud != null)
        {
            salud.RecibirDano(danoAlJugador); // Resta 1 vida y actualiza los corazones
        }
    

    Debug.Log("¡La Jarchaja te tocó y te hizo daño!");
    }



}