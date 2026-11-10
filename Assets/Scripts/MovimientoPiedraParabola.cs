using UnityEngine;

public class MovimientoPiedraParabola : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Esta función recibe el punto de origen, el punto de destino (jugador) y qué tan alta quieres la curva (alturaParabola)
    public void LanzarParabola(Vector2 origen, Vector2 destino, float alturaParabola, float gravedadPersonalizada)
    {
        if (rb == null) return;

        rb.gravityScale = gravedadPersonalizada;

        // Distancias entre el origen y el destino
        float desplazamientoX = destino.x - origen.x;
        float desplazamientoY = destino.y - origen.y;

        // Calculamos la velocidad inicial necesaria en X y Y para hacer la parábola con la gravedad dada
        float tiempoVuelo = Mathf.Sqrt(-2 * alturaParabola / (Physics2D.gravity.y * gravedadPersonalizada)) +
                            Mathf.Sqrt(2 * (desplazamientoY - alturaParabola) / (Physics2D.gravity.y * gravedadPersonalizada));

        float velocidadX = desplazamientoX / tiempoVuelo;
        float velocidadY = (-Physics2D.gravity.y * gravedadPersonalizada * tiempoVuelo) / 2f + (desplazamientoY / tiempoVuelo);

        // Aplicamos el impulso inicial en forma de curva
        rb.linearVelocity = new Vector2(velocidadX, velocidadY);
    }
}