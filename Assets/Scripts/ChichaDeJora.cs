using UnityEngine;

public class ChichaDeJora : MonoBehaviour
{
    [Header("Configuración de Curación")]
    public int cantidadVidaARecuperar = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("¡Colisión detectada con: " + other.gameObject.name + " (Tag: " + other.tag + ")");

        // 1. Buscamos el componente SaludJugador en el objeto o en su jerarquía padre
        SaludJugador saludJugador = other.GetComponent<SaludJugador>();
        if (saludJugador == null)
        {
            saludJugador = other.GetComponentInParent<SaludJugador>();
        }

        // 2. Comprobamos si encontramos al jugador con su componente de salud
        if (saludJugador != null || other.CompareTag("Player"))
        {
            if (saludJugador != null)
            {
                Debug.Log("¡Componente SaludJugador encontrado!");

                // Comprobamos si le falta vida antes de curar
                if (saludJugador.VidasActuales < saludJugador.vidasMaximas)
                {
                    saludJugador.GanarVida(cantidadVidaARecuperar);
                    Debug.Log("¡Vida curada con éxito!");

                    // Destruimos el cuenco solo si pudo curarse
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("El jugador ya tiene la vida al máximo.");
                    // Si prefieres que el cuenco se consuma aunque tengas la vida llena, saca el Destroy de aquí.
                }
            }
            else
            {
                Debug.LogWarning("El objeto tiene el tag Player, pero NO tiene el script 'SaludJugador' adjunto.");
            }
        }
        else
        {
            Debug.Log("El objeto que tocó la chicha no es el jugador ni tiene el tag 'Player'.");
        }
    }
}