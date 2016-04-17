﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomsManager : MonoBehaviour {

    public enum Direction {north, south, east, west};

    private Dictionary<Vector2, GameObject> rooms = new Dictionary<Vector2, GameObject>();
    private Dictionary<GameObject, Vector2> reverseRooms = new Dictionary<GameObject, Vector2>();
    public Vector2 offset = new Vector2(18, 12);
    public GameObject firstRoom;

	// Use this for initialization
	void Start () {
        rooms[new Vector2(0, 0)] = firstRoom;
        reverseRooms[firstRoom] = new Vector2(0, 0);
        AddRooms(firstRoom);
	}

    private void AddRooms(GameObject originRoom){
        Vector2 originRoomPosition = reverseRooms[originRoom];

        int x = (int)originRoomPosition.x;
        int y = (int)originRoomPosition.y;

        Vector2 northRoomPosition = new Vector2(x, y + 1);
        Vector2 southRoomPosition = new Vector2(x, y - 1);
        Vector2 westRoomPosition = new Vector2(x - 1, y);
        Vector2 eastRoomPosition = new Vector2(x + 1, y);

        Color color;

        color = GenerateColor();

        AddRoom(northRoomPosition, originRoom.transform.rotation, color);

        color = GenerateColor();

        AddRoom(southRoomPosition, originRoom.transform.rotation, color);

        color = GenerateColor();

        AddRoom(westRoomPosition, originRoom.transform.rotation, color);

        color = GenerateColor();

        AddRoom(eastRoomPosition, originRoom.transform.rotation, color);
    }

    private void AddRoom(Vector2 roomPosition, Quaternion rotation, Color color){
        GameObject newRoom;
        if(rooms.TryGetValue(roomPosition, out newRoom)){
        }
        else{
            Vector3 newPosition = new Vector3(roomPosition.x * offset.x, 0f, roomPosition.y * offset.y);
            newRoom = Instantiate(Resources.Load("room", typeof(GameObject)), newPosition, rotation) as GameObject;
            rooms[roomPosition] = newRoom;
            reverseRooms[newRoom] = roomPosition;

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
        Vector2 exitRoomPosition = reverseRooms[exitRoom];

        switch(direction){
            case Direction.north: exitToOffset = new Vector2(0, 1);break;
            case Direction.south: exitToOffset = new Vector2(0, -1);break;
            case Direction.east: exitToOffset = new Vector2(1, 0);break;
            case Direction.west: exitToOffset = new Vector2(-1, 0);break;
        }

        Vector2 originRoomPosition = new Vector2(exitRoomPosition.x + exitToOffset.x, exitRoomPosition.y + exitToOffset.y);
        GameObject originRoom = rooms[originRoomPosition];
        AddRooms(originRoom);

        //change camera

        GameObject cameraOff = exitRoom.transform.FindChild("camera").gameObject;
        GameObject cameraOn = originRoom.transform.FindChild("camera").gameObject.gameObject;

        cameraOff.SetActive(false);
        cameraOn.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void OpenRoom(GameObject room){
        room.GetComponent<RoomContentsManager>().OpenRoom(true);
    }
}
