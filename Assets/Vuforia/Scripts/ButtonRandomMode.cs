using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonRandomMode : MonoBehaviour
{

    public void ButtonClicked()
    {
        string buttonName = gameObject.name;

        if (buttonName == "ButtonRandom")
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            SceneManager.LoadSceneAsync("RandomMode");
        }
        else
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            SceneManager.LoadSceneAsync("SampleScene");
            //SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }

    
    public void MainMenuNormal()
    {
        SceneManager.UnloadSceneAsync("SampleScene");
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void MainMenuRandom()
    {
        SceneManager.UnloadSceneAsync("RandomMode");
        SceneManager.LoadSceneAsync("MainMenu");
    }

}
