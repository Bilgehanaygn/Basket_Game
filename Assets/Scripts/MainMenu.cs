using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadLevelsScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
    
    
}
