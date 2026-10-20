using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaludJugador : MonoBehaviour
{
    [Header("Configuracion de Vidas")]
    public int vidasMaximas = 3;
    private int vidasActuales;

    [Header("UI de Corazones")]
    public Image[] imagenesCorazones; // Arrastra aquí tus imágenes de corazones desde la UI

    private void Start()
    {
        vidasActuales = vidasMaximas;
        ActualizarCorazones();
    }

    public void RecibirDano(int cantidad)
    {
        vidasActuales -= cantidad;
        if (vidasActuales < 0) vidasActuales = 0;

        ActualizarCorazones();

        if (vidasActuales <= 0)
        {
            MorirYReiniciar();
        }
    }

    private void ActualizarCorazones()
    {
        for (int i = 0; i < imagenesCorazones.Length; i++)
        {
            if (i < vidasActuales)
            {
                imagenesCorazones[i].enabled = MuestraCorazon(true); // Muestra el corazón
            }
            else
            {
                imagenesCorazones[i].enabled = false; // Oculta el corazón si perdió vida
            }
        }
    }

    private bool MuestraCorazon(bool estado)
    {
        return estado;
    }

    private void MorirYReiniciar()
    {
        Debug.Log("¡El jugador ha muerto! Reiniciando nivel...");
        // Recarga la escena actual para reiniciar el nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}