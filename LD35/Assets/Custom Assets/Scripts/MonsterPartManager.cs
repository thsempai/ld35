using UnityEngine;
using System.Collections;

public class MonsterPartManager : MonoBehaviour {

    // Use this for initialization

    public string monsterBase;
    public string monsterHead;
    public Color color; 

    GameObject baseObject;
    GameObject headObject;

    void Start () {
        MonsterGeneration();
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    public void MonsterGeneration(){


        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }

        baseObject = Instantiate(Resources.Load(monsterBase, typeof(GameObject))) as GameObject;
        baseObject.transform.parent = transform;
        baseObject.transform.localPosition = Vector3.zero;

        baseObject.GetComponent<MonsterColorManager>().ResetColor(color);

        Transform headPlace = baseObject.transform.FindChild("head").transform;

        headObject = Instantiate(Resources.Load(monsterHead, typeof(GameObject))) as GameObject;
        headObject.transform.parent = headPlace;
        headObject.transform.localPosition = Vector3.zero;

        headObject.GetComponent<MonsterColorManager>().ResetColor(color);
    }
}
