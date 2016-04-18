using UnityEngine;
using System.Collections;

public class MonsterMoveBehavior : MonoBehaviour {

    public enum MoveType {moveV, moveH, stable,moveHV};
    public MoveType type;
    public MonsterHeadBehavior headManager;
    public float distanceReact = 18f;
    public float speed = 5f;
    public bool withoutHead;
    private RoomsManager manager;

    public GameObject player;
    // Use this for initialization
    void Start () {
        manager = GameObject.Find("generic").GetComponent<RoomsManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        if(!withoutHead){
            foreach(Transform child in transform){
                foreach(Transform otherChild in child){
                    Transform headTransform = otherChild.FindChild("shooter");
                    headManager = otherChild.gameObject.GetComponent<MonsterHeadBehavior>();
                    headManager.baseManager = this;
                    headManager.player = player;
                    }
                }
            }
    }
    
    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Weapon"){
            Kill();
        }
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "Player"){
            PlayerReStart();
        }
    }

    void OnMouseOver()
    {
        if(!withoutHead)
        EnterInMonster();
    }

    public void EnterInMonster(){
        if(Input.GetMouseButtonDown(1))
        manager.EnterInMonster(transform.parent.gameObject);
    }

    public void PlayerReStart(){
        player.transform.position = Vector3.zero;
        manager.Restart();
    }

    public void Kill(){
        Destroy(transform.parent.gameObject);
    }
    // Update is called once per frame
    void Update () {
        float x = 0f;
        float z = 0f;
        if(type != MoveType.stable){
            if(Vector3.Distance(transform.position, player.transform.position) <= distanceReact){
                float espace = Time.deltaTime * speed;
                if(type == MoveType.moveV||type == MoveType.moveHV){
                    float distance = player.transform.position.z - transform.position.z;
                    if(espace > Mathf.Abs(distance)){
                        espace = 0f;
                    }
                    if(distance < 0){
                        espace *= -1;
                    }
                    z = espace;

                }
                if(type == MoveType.moveH||type == MoveType.moveHV){
                    float distance = player.transform.position.x - transform.position.x;
                    if(espace > Mathf.Abs(distance)){
                        espace = 0f;
                    }
                    if(distance < 0){
                        espace *= -1;
                    }
                    x = espace;
                }
                Vector3 position = transform.position;
                position.z += z;
                position.x += x;
                transform.position = position;
            }
        }
    }
}
