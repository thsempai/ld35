using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomsManager : MonoBehaviour {

    public enum Direction {north, south, east, west};

    private Dictionary<Vector2, GameObject> rooms = new Dictionary<Vector2, GameObject>();
    private Dictionary<GameObject, Vector2> reverseRooms = new Dictionary<GameObject, Vector2>();

    private Dictionary<Vector2, GameObject> roomsMonster = new Dictionary<Vector2, GameObject>();
    private Dictionary<GameObject, Vector2> reverseRoomsMonster = new Dictionary<GameObject, Vector2>();

    public Vector2 offset = new Vector2(18, 12);
    public GameObject firstRoom;
    public GameObject currentRoom;
    public GameObject previousRoom;
    public bool inMonsterMode = false;
    public Color roomColor;
    public GameObject monster;
    public float dungeonMonster = 40f;
    public GameObject player;
    public bool transition = false;
    public GameObject dungeonReturn;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        rooms[new Vector2(0, 0)] = firstRoom;
        reverseRooms[firstRoom] = new Vector2(0, 0);
        AddRooms(firstRoom);
        currentRoom = firstRoom;
        previousRoom = firstRoom;
	}

    private void AddRooms(GameObject originRoom){
        Vector2 originRoomPosition;
        if(inMonsterMode){
            originRoomPosition = reverseRoomsMonster[originRoom];
        }
        else{
            originRoomPosition = reverseRooms[originRoom];
        }

        int x = (int)originRoomPosition.x;
        int y = (int)originRoomPosition.y;

        Vector2 northRoomPosition = new Vector2(x, y + 1);
        Vector2 southRoomPosition = new Vector2(x, y - 1);
        Vector2 westRoomPosition = new Vector2(x - 1, y);
        Vector2 eastRoomPosition = new Vector2(x + 1, y);

        Color color = roomColor;
        float h = 0f;
        if(inMonsterMode){
            h = dungeonMonster;
        }

        if(!inMonsterMode){
            color = GenerateColor();
        }

        AddRoom(northRoomPosition, originRoom.transform.rotation, color, h);

        if(!inMonsterMode){
            color = GenerateColor();
        }

        AddRoom(southRoomPosition, originRoom.transform.rotation, color, h);

        if(!inMonsterMode){
            color = GenerateColor();
        }

        AddRoom(westRoomPosition, originRoom.transform.rotation, color, h);

        if(!inMonsterMode){
            color = GenerateColor();
        }

        AddRoom(eastRoomPosition, originRoom.transform.rotation, color, h);
    }

    private void AddRoom(Vector2 roomPosition, Quaternion rotation, Color color, float height=0f){
        GameObject newRoom;
        bool newOne = true;
        if(inMonsterMode){
            if(roomsMonster.TryGetValue(roomPosition, out newRoom)){
                newOne = false;
            }
        }
        else {
            if(rooms.TryGetValue(roomPosition, out newRoom)){
                newOne = false;
            }
        }
        if(newOne){
            Vector3 newPosition = new Vector3(roomPosition.x * offset.x, height, roomPosition.y * offset.y);
            newRoom = Instantiate(Resources.Load("room", typeof(GameObject)), newPosition, rotation) as GameObject;
            if(inMonsterMode){
                roomsMonster[roomPosition] = newRoom;
                reverseRoomsMonster[newRoom] = roomPosition;
            }
            else{
                rooms[roomPosition] = newRoom;
                reverseRooms[newRoom] = roomPosition;
            }

            // change color
            foreach(Material RoomMaterial in newRoom.GetComponent<Renderer>().materials){
                string materialName = RoomMaterial.name.Replace("(Instance)","");
                if(materialName == "wall-decored" || materialName == "ground")
                    RoomMaterial.SetColor("_Color", Color.yellow);
                    RoomMaterial.SetColor("_EmissionColor", color);
            }
        }
        OpenRoom(newRoom);
    }

    public Color GenerateColor(){
        float r;
        float g;
        float b;
        Color color;

        r = (float)Random.Range(0, 11) /10f;
        g = (float)Random.Range(0, 11) /10f;
        b = (float)Random.Range(0, 11) /10f;
        color = new Color(r, g, b, 1f);
        return color;
    }

    public void ExitRoom(GameObject exitRoom, Direction direction){
        Vector2 exitToOffset = new Vector2(0,0);
        Vector2 exitRoomPosition;
        if(inMonsterMode){
            exitRoomPosition = reverseRoomsMonster[exitRoom];
        }
        else{
            exitRoomPosition = reverseRooms[exitRoom];
        }

        switch(direction){
            case Direction.north: exitToOffset = new Vector2(0, 1);break;
            case Direction.south: exitToOffset = new Vector2(0, -1);break;
            case Direction.east: exitToOffset = new Vector2(1, 0);break;
            case Direction.west: exitToOffset = new Vector2(-1, 0);break;
        }

        Vector2 originRoomPosition = new Vector2(exitRoomPosition.x + exitToOffset.x, exitRoomPosition.y + exitToOffset.y);
        GameObject originRoom;
        if(inMonsterMode){
            originRoom = roomsMonster[originRoomPosition];
        }
        else{
            originRoom = rooms[originRoomPosition];
        }
        AddRooms(originRoom);

        //change camera

        GameObject cameraOff = exitRoom.transform.FindChild("camera").gameObject;
        GameObject cameraOn = originRoom.transform.FindChild("camera").gameObject.gameObject;

        cameraOff.SetActive(false);
        cameraOn.SetActive(true);
        previousRoom = currentRoom;
        currentRoom = originRoom;
    }

    public void Restart(){

        //change camera
        inMonsterMode = false;
        GameObject cameraOff = currentRoom.transform.FindChild("camera").gameObject;
        GameObject cameraOn = firstRoom.transform.FindChild("camera").gameObject.gameObject;

        cameraOff.SetActive(false);
        cameraOn.SetActive(true);
        previousRoom = currentRoom;
        currentRoom = firstRoom;
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonUp(1)){
            transition = false;
        }
	   if(Input.GetMouseButtonDown(1) && inMonsterMode && !transition){
        Returns();
       }
	}

    public void Returns(){
        inMonsterMode = false;
        float x = PlayerPrefs.GetFloat("x-dungeon");
        float z = PlayerPrefs.GetFloat("z-dungeon");

        Vector3 position = new Vector3(x, 0f, z);
        player.transform.position = position;
        GameObject cameraOff = currentRoom.transform.FindChild("camera").gameObject;
        GameObject cameraOn = dungeonReturn.transform.FindChild("camera").gameObject.gameObject;

        cameraOff.SetActive(false);
        cameraOn.SetActive(true);

        previousRoom = currentRoom;
        currentRoom = dungeonReturn;

    }

    public void OpenRoom(GameObject room){
        room.GetComponent<RoomContentsManager>().OpenRoom(true);
    }

    public void EnterInMonster(GameObject monsterTarget){
        dungeonReturn = currentRoom;
        inMonsterMode = true;
        PlayerPrefs.SetFloat("x-dungeon", player.transform.position.x);
        PlayerPrefs.SetFloat("z-dungeon", player.transform.position.z);
        foreach(KeyValuePair<Vector2, GameObject> entry in roomsMonster)
        {
        Destroy(entry.Value);
        }
        roomsMonster = new Dictionary<Vector2, GameObject>();
        reverseRoomsMonster = new Dictionary<GameObject, Vector2>();

        monster = monsterTarget;
        roomColor = monster.GetComponent<MonsterPartManager>().color;
        Vector3 newPosition = new Vector3(0f, dungeonMonster, 0f);
        GameObject newRoom = Instantiate(Resources.Load("room", typeof(GameObject))) as GameObject;
        newRoom.transform.position = newPosition;
        foreach(Material RoomMaterial in newRoom.GetComponent<Renderer>().materials){
                string materialName = RoomMaterial.name.Replace("(Instance)","");
                if(materialName == "wall-decored" || materialName == "ground")
                    RoomMaterial.SetColor("_Color", roomColor);
                    RoomMaterial.SetColor("_EmissionColor", roomColor);
            }
        GameObject cameraOff = currentRoom.transform.FindChild("camera").gameObject;
        GameObject cameraOn = newRoom.transform.FindChild("camera").gameObject.gameObject;

        cameraOff.SetActive(false);
        cameraOn.SetActive(true);

        roomsMonster[new Vector2(0, 0)] = newRoom;
        reverseRoomsMonster[newRoom] = new Vector2(0, 0);
        AddRooms(newRoom);
        player.transform.position = newRoom.transform.position;
        transition = true;
        }
}
