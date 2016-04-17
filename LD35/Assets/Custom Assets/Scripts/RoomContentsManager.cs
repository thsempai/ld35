using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomContentsManager : MonoBehaviour {

    public GameObject[] doors;
    public GameObject button;
    public bool noMonster = false;
    private RoomsManager manager;
    private int monsterMax = 2;
    public bool open;
    public Vector2 maxOffset = new Vector2(15, 9);

    public List<GameObject> monsters = new List<GameObject>();
    // Use this for initialization
    void Start () {
        doors = new GameObject[4];
        doors[0] = transform.FindChild("north door").gameObject;
        doors[1] = transform.FindChild("south door").gameObject;
        doors[2] = transform.FindChild("east door").gameObject;
        doors[3] = transform.FindChild("west door").gameObject;

        List<Vector3> positionAlreayUsed = new List<Vector3>();
        manager = GameObject.Find("generic").GetComponent<RoomsManager>();

        button = transform.FindChild("button").gameObject;

        //random place for button
        int dx = Random.Range(0, (int) maxOffset.x + 1);
        int dz = Random.Range(0, (int) maxOffset.y + 1);

        Vector3 origin = button.transform.position;
        Vector3 buttonPosition = origin;

        buttonPosition.x = buttonPosition.x + 1.955f * (float)dx;
        buttonPosition.z = buttonPosition.z - 1.955f * (float)dz;

        button.transform.position = buttonPosition;
        positionAlreayUsed.Add(buttonPosition);


        if(!noMonster){
            int monsterNumber = Random.Range(1,monsterMax + 1);
            for(int index=0; index < monsterNumber; index++){
                GameObject monsterObject;
                if(manager.inMonsterMode){
                    monsterObject = Instantiate(Resources.Load("dummy", typeof(GameObject))) as GameObject;
                }
                else{
                    monsterObject = Instantiate(Resources.Load("monster", typeof(GameObject))) as GameObject;
                }
                monsters.Add(monsterObject);
                Vector3 monsterPosition;
                do{
                    dx = Random.Range(0, (int) maxOffset.x + 1);
                    dz = Random.Range(0, (int) maxOffset.y + 1);
                    monsterPosition = origin;
                    monsterPosition.x = monsterPosition.x + 1.955f * (float)dx;
                    monsterPosition.z = monsterPosition.z - 1.955f * (float)dz;
                }
                while(positionAlreayUsed.Contains(monsterPosition));
                monsterObject.transform.position = monsterPosition;
                monsterObject.transform.parent = transform;

                if(!manager.inMonsterMode){
                    MonsterPartManager partManager = monsterObject.GetComponent<MonsterPartManager>();
                    partManager.color = manager.GenerateColor();
                    int baseRnd = Random.Range(0, 3);
                    int headRnd = Random.Range(0, 3);

                    switch(baseRnd){
                        case 0: partManager.monsterBase = "stable";break;
                        case 1: partManager.monsterBase = "move-v";break;
                        case 2: partManager.monsterBase = "move-h";break;
                    }

                    switch(headRnd){
                        case 0: partManager.monsterHead = "agressive";break;
                        case 1: partManager.monsterHead = "watcher";break;
                        case 2: partManager.monsterHead = "shooter";break;
                    }
                    partManager.MonsterGeneration();
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update () {
        OpenRoom(open);
        foreach(GameObject monster in monsters){
            try{

            monster.SetActive(manager.currentRoom == gameObject);
            }
            catch(MissingReferenceException mre){
            }
        }

    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            open = false;
            button.GetComponent<ButtonManager>().Off();
            manager.previousRoom.GetComponent<BoxCollider>().enabled = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void OpenRoom(bool isOpen){
        foreach(GameObject door in doors){
            door.SetActive(!isOpen);
        }
        open = isOpen;
    }
}
