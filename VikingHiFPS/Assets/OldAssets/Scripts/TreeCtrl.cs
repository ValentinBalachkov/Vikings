using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCtrl : MonoBehaviour
{
    [SerializeField] float coolDown;
    [SerializeField] int maxVolume = 6;
    public int volume;
    public int freeVolume;
    // Start is called before the first frame update
    void Start()
    {
        volume = maxVolume;
        freeVolume = maxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable() {
        Invoke("EnableTree", coolDown);
    }

    private void OnEnable() {
        volume = maxVolume;
        freeVolume = maxVolume; 
    }
    void EnableTree()
    {
        gameObject.SetActive(true);
        Messenger<int>.Broadcast(GameEvent.TREE_RESPAWN, maxVolume);
    }
}
