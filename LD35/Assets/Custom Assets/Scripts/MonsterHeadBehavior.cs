using UnityEngine;
using System.Collections;

public class MonsterHeadBehavior : MonoBehaviour {

    public enum HeadType {shooter, agressive, watcher};
    public HeadType type;
    public MonsterMoveBehavior baseManager; 
    public GameObject player;
    public float RotationSpeed = 5f;
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    }

    void OnTriggerEnter(Collider collisionInfo){
        if (collisionInfo.gameObject.tag == "Weapon"){
            baseManager.Kill();
        }
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "Player"){
            baseManager.PlayerReStart();
        }
    }
    void OnMouseOver()
    {
        baseManager.EnterInMonster();
    }
}
