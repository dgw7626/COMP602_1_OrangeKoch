using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_3dModelManager : MonoBehaviour
{
    public Material purple;
    public Material orange;
    private Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = transform.GetChild(0).GetComponent<Renderer>();
        if(myRenderer ==  null )
        {
            Debug.LogError("Null Renderer found on player " + gameObject.name);
        }
    }

    public void SetTeamColour(int teamNumber)
    {
        if (teamNumber == 0)
            myRenderer.material = orange;
        else
            myRenderer.material = purple;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
