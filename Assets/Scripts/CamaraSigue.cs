using UnityEngine;

public class CamaraSigue : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform objetivo; // Arrastra aquí a tu Player

    [Header("Configuracion de Seguimiento")]
    public float tiempoSuavizado = 0.2f; // Qué tan suave sigue la cámara (valores entre 0.1 y 0.3 se sienten genial)
    public Vector3 desfasaje = new Vector3(0f, 0f, -10f); // Mantiene el eje Z en -10 para no tapar la vista 2D

    [Header("Limites del Nivel (Opcional)")]
    public bool usarLimites = false;
    public float limiteIzquierda = -10f;
    public float limiteDerecha = 50f;
    public float limiteAbajo = -5f;
    public float limiteArriba = 10f;

    private Vector3 velocidad = Vector3.zero;

    private void LateUpdate()
    {
        if (objetivo == null) return;

        // Calculamos la posición deseada basada en la posición del jugador
        Vector3 posicionDeseada = objetivo.position + desfasaje;

        // Si activaste límites, restringimos la posición de la cámara dentro de los márgenes del nivel
        if (usarLimites)
        {
            posicionDeseada.x = Mathf.Clamp(posicionDeseada.x, limiteIzquierda, limiteDerecha);
            posicionDeseada.y = Mathf.Clamp(posicionDeseada.y, limiteAbajo, limiteArriba);
        }

        // Movemos la cámara de forma fluida y suave
        transform.position = Vector3.SmoothDamp(transform.position, posicionDeseada, ref velocidad, tiempoSuavizado);
    }
}