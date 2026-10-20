using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaNivel : MonoBehaviour
{
    public string siguienteNivel = "Nivel2";

    [Header("Final del juego")]
    public bool esNivelFinal = false;
    public GameObject panelVictoria;

    private bool salidaActivada = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (salidaActivada || !other.CompareTag("Player"))
            return;

        salidaActivada = true;

        if (esNivelFinal)
        {
            if (panelVictoria != null)
            {
                panelVictoria.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Debug.Log("¡VICTORIA! Has completado los 5 niveles.");
            }
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(siguienteNivel);
        }
    }
}