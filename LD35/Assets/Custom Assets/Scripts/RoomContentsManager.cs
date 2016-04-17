using UnityEngine;
using System.Collections;

public class RoomContentsManager : MonoBehaviour {

    public GameObject[] doors;
    public GameObject button;
    //private RoomsManager manager;
    public bool open;
    public Vector2 buttonMaxOffset = new Vector2(15, 9);
    // Use this for initialization
    void Start () {
        doors = new GameObject[4];
        doors[0] = transform.FindChild("north door").gameObject;
        doors[1] = transform.FindChild("south door").gameObject;
        doors[2] = transform.FindChild("east door").gameObject;
        doors[3] = transform.FindChild("west door").gameObject;

        //manager = GameObject.Find("generic").GetComponent<RoomsManager>();

        button = transform.FindChild("button").gameObject;

        //random place for button
        int dx = Random.Range(0, (int) buttonMaxOffset.x + 1);
        int dz = Random.Range(0, (int) buttonMaxOffset.y + 1);

        Vector3 buttonPosition = button.transform.position;
        buttonPosition.x = buttonPosition.x + 1.955f * (float)dx;
        buttonPosition.z = buttonPosition.z - 1.955f * (float)dz;

        button.transform.position = buttonPosition;
    }
    
    // Update is called once per frame
    void Update () {
        OpenRoom(open);
    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            open = false;
            button.GetComponent<ButtonManager>().Off();
        }
    }

    public void OpenRoom(bool isOpen){
        foreach(GameObject door in doors){
            door.SetActive(!isOpen);
        }
        open = isOpen;
    }
}
