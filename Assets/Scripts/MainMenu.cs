using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This function will be called when the Play button is pressed
    public void PlayGame()
    {
        // Replace "GameScene" with the exact name of your scene
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        
    }

}
