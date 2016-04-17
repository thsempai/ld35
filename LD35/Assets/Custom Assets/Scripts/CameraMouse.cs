using UnityEngine;
using System.Collections;

public class CameraMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update () {
        RaycastHit hit;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            Debug.DrawLine (transform.position, hit.point, Color.cyan);
            print(hit.collider.name);
            if(hit.collider.tag == "Monster"){
            print(hit.collider.gameObject.name);
            }
        }
    }
}
