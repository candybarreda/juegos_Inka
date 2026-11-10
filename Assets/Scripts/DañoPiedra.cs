using UnityEngine;

public class DañoPiedra : MonoBehaviour
{
    public int danio = 1; // Cantidad de vida que quita

    void OnTriggerEnter2D(Collider2D otro)
    {
        // Si choca con el Jugador
        if (otro.CompareTag("Player"))
        {
            // Busca el script SaludJugador en el jugador y le quita vida
            SaludJugador saludJugador = otro.GetComponent<SaludJugador>();
            if (saludJugador != null)
            {
                saludJugador.RecibirDano(danio);
            }

            // Destruye la piedra al impactar
            Destroy(gameObject);
        }
        // Si choca con el suelo o una plataforma, también se destruye
        else if (otro.CompareTag("Suelo") || otro.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}