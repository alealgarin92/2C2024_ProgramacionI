using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDerrota : MonoBehaviour
{
    [SerializeField] private GameObject pantallaMenuDerrota;

    public void Update()
    {
        if (pantallaMenuDerrota.activeSelf)
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
    }


    public void ReiniciarElNivel()
    {
        Time.timeScale = 1f; // Restaura el tiempo antes de recargar la escena.

        // Reinicia manualmente los estados si es necesario.
        pantallaMenuDerrota.SetActive(false);

        // Carga la escena actual.
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void MenuInicial(int nro)
    {
        SceneManager.LoadScene(nro);
    }
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}
