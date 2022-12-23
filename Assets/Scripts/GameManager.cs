using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private int remainingHealth = 3;
    private TMPro.TextMeshProUGUI scoreText;
    private void Awake()
    {
        scoreText = FindObjectOfType<Canvas>().gameObject.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        scoreText.text = remainingHealth.ToString();
    }

    public void GameOver(){
        // check if remaining health is lower than 3;
        //if not decrease the remaining health by 1, update the text;
        //if so offer to watch a video for 3 extra healts;
        if(remainingHealth <= 1){
            return;
        }
        else{
            remainingHealth -= 1;
            updateText(scoreText, remainingHealth.ToString());
        }
    }

    private void updateText(TMPro.TextMeshProUGUI text, string newContent){
        text.text = newContent;
    }
}
