using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Carga el Nivel 1 para comenzar el juego
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1"); // Asegúrate de que coincida con el nombre exacto de tu escena
    }

    // Cierra el juego (funciona en la compilación final)
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}