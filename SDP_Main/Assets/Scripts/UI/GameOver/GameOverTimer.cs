
using UnityEngine;
using Color = UnityEngine.Color;
using TMPro;

public class GameOverTimer : MonoBehaviour
{
    public float startTime = 120f;
    public float currentTime = 0;
    [SerializeField] TextMeshProUGUI textColoum;
    [SerializeField] TextMeshProUGUI textColor;

    Color red = Color.red;
    Color green = Color.green;
    

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;

        textColoum.text = "Time " + currentTime.ToString("000");

        Color red = Color.red;

        if (currentTime <= 5)
        {
            textColoum.color = red;
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
        }
    }

    public void setColor_green()
    {
        if (currentTime <= 10)
        {
            textColoum.color = green;
        }
    }
    public void setColor_red()
    {
        if (currentTime <= 5)
        {
            textColoum.color = red;
        }
    }

    public void setTime(float nTime)
    {
        startTime = nTime;
    }

    public void Negtive_Time(float nTime)
    {
        if (nTime<= 0)
        {
            print("Error");
        }

    }

    public void OverThe_Time(float nTime)
    {
       
        if (nTime>= startTime)
        {
            startTime = nTime;
        }

        
    }

    public void LessThe_Time(float nTime)
    {

        if (nTime <= startTime)
        {
            startTime = nTime;
        }


    }
    //    textColoum.color = red;


}
