using UnityEngine;
using System.Collections;

public class ExitRoomManager : MonoBehaviour {

    public RoomsManager.Direction direction;
    public RoomsManager manager;
	// Use this for initialization
	void Start () {
        manager = GameObject.Find("generic").GetComponent<RoomsManager>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        manager.ExitRoom(transform.parent.gameObject, direction);
    }
}
