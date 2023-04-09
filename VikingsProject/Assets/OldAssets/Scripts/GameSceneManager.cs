using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] GameObject towerPlace;
    [SerializeField] GameObject craftPlace;
    [SerializeField] GameObject boards;
    // Start is called before the first frame update
    void Start()
    {
        // Messenger<string>.AddListener(GameEvent.START_CRAFT, OnStartCraft);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStartCraft(string id)
    {
        // Instantiate(boards, craftPlace.transform.position, Quaternion.identity);
    }
}
