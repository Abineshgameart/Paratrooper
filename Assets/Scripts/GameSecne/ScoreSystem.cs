using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreUI;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreUI.text = "Score : " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.text = "Score : " + score.ToString();
    }
}
