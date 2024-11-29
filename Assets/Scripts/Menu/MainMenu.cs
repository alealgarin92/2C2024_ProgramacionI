#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton, optionsButton, quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonPressed);
        quitButton.onClick.AddListener(OnOptionsButtonPressed);
        optionsButton.onClick.AddListener(OnQuitButtonPressed);
    }

    // Update is called once per frame
    private void OnPlayButtonPressed()
    {
        Debug.Log("Play Game");
        SceneManager.LoadScene(sceneName: "Nivel_1");
    }

    private void OnOptionsButtonPressed()
    {
        Debug.Log("Quit Game");

        //Application.Quit(); //Solo funciona en la build, no en el editor

#if UNITY_EDITOR 
        EditorApplication.ExitPlaymode(); //Solo editor, no funciona en la build!!!
#endif
    }

    private void OnQuitButtonPressed()
    {
        Debug.Log("Options");
    }




}

