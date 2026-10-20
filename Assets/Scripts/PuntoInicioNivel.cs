using UnityEngine;

public class PuntoInicioNivel : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = transform.position;
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto Player.");
        }
    }
}