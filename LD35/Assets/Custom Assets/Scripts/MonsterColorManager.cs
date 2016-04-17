using UnityEngine;
using System.Collections;

public class MonsterColorManager : MonoBehaviour {

    public Color color;
    // Use this for initialization
    void Start () {
        ResetColor();
    }
    
    public void ResetColor()
    {
    GetComponent<Renderer>().material.SetColor("_Color",color);
    }
    // Update is called once per frame
    void Update () {
    
    }
}
