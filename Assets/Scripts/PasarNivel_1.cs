using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasarNivel1 : MonoBehaviour
{
    
    
    private void OnTriggerStay(Collider other)
    {
        FrontDoor(other.gameObject);
    }

    private void FrontDoor(GameObject target)
    {
        MainCharacter player = target.GetComponent<MainCharacter>();
        if (player != null)
        {
            if (Input.GetKey(KeyCode.F) )
            {
                PasarNivelSubte();
            }
        }
    }


    public void PasarNivelSubte()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}