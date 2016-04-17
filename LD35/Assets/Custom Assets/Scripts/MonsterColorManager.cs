using UnityEngine;
using System.Collections;

public class MonsterColorManager : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }
    
    public void ResetColor(Color color)
    {
    GetComponent<Renderer>().material.SetColor("_Color",color);
    }
    // Update is called once per frame
    void Update () {
    
    }
}
