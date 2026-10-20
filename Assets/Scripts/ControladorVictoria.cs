using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorVictoria : MonoBehaviour
{
    [Header("UI Elementos")]
    public GameObject panelVictoria; // Aquí arrastraremos el PanelVictoria

    [Header("Configuración de Escena")]
    public string nombreEscenaMenu = "Menu"; // Nombre exacto de tu escena de menú principal

    private bool juegoTerminado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si el jugador (con tag "Player") toca la salida
        if (collision.CompareTag("Player") && !juegoTerminado)
        {
            MostrarVictoria();
        }
    }

    public void MostrarVictoria()
    {
        juegoTerminado = true;

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true); // Enciende el panel en pantalla
        }

        // Congela el tiempo del juego para que el personaje no se mueva
        Time.timeScale = 0f;
    }

    public void CargarMenu()
    {
        // Restaura el tiempo normal antes de cambiar de escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}