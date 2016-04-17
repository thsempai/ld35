using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

    GameObject button;
    public bool on = false;
    public Color offColor;
    public Color onColor;
    public float offHeight = 0.25f;
    public float onHeight = 0f;
    public RoomsManager manager;

    // Use this for initialization
    void Start () {
        button = transform.FindChild("button").gameObject;

        Off();

        manager = GameObject.Find("generic").GetComponent<RoomsManager>();
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    public void On(){
        button.GetComponent<Renderer>().material.SetColor("_Color",onColor);

        Vector3 position = button.transform.position;
        position.y = onHeight;
        button.transform.position = position;
        manager.OpenRoom(transform.parent.gameObject);
        on = true;
    }

    public void Off(){
        button.GetComponent<Renderer>().material.SetColor("_Color",offColor);

        Vector3 position = button.transform.position;
        position.y = offHeight;
        button.transform.position = position;
        on = false;
    }


    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Weapon"){
            On();
        }

    }
}
