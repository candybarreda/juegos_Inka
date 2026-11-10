using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaludJugador : MonoBehaviour
{
    [Header("Configuracion de Vidas")]
    public int vidasMaximas = 3;

    // Cambiamos a propiedad pública para que la Chicha de Jora pueda consultar cuántas vidas tienes
    public int VidasActuales { get; private set; }

    [Header("UI de Corazones")]
    public Image[] imagenesCorazones; // Arrastra aquí tus imágenes de corazones desde la UI

    private void Start()
    {
        VidasActuales = vidasMaximas;
        ActualizarCorazones();
    }

    public void RecibirDano(int cantidad)
    {
        VidasActuales -= cantidad;
        if (VidasActuales < 0) VidasActuales = 0;

        ActualizarCorazones();

        if (VidasActuales <= 0)
        {
            MorirYReiniciar();
        }
    }

    // --- FUNCIÓN NUEVA: Curar / Ganar vida (llamada por la Chicha de Jora) ---
    public void GanarVida(int cantidad)
    {
        VidasActuales += cantidad;

        if (VidasActuales > vidasMaximas)
        {
            VidasActuales = vidasMaximas; // Evita que tengas más corazones del límite máximo
        }

        ActualizarCorazones(); // Refresca las imágenes de los corazones en la UI
        Debug.Log("¡Vida recuperada con la Chicha de Jora! Vidas actuales: " + VidasActuales);
    }

    private void ActualizarCorazones()
    {
        for (int i = 0; i < imagenesCorazones.Length; i++)
        {
            if (i < VidasActuales)
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