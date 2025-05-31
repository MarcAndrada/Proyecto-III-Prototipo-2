using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCanvasController : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Map");
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
