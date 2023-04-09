using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] int speedLevel = 1;
    [SerializeField] int miningLevel = 1;
    [SerializeField] int fishingLevel = 1;
    [SerializeField] int woodingLevel = 1;
    [SerializeField] float speed = 10;

    [SerializeField] RuntimeAnimatorController  idleCtrl;
    [SerializeField] RuntimeAnimatorController  woodingCtrl;
    [SerializeField] RuntimeAnimatorController  miningCtrl;
    [SerializeField] RuntimeAnimatorController  fishingCtrl;
    [SerializeField] RuntimeAnimatorController  runingCtrl;
    [SerializeField] RuntimeAnimatorController  gatheringCtrl;

    [SerializeField] GameObject axe;
    [SerializeField] GameObject pick;
    [SerializeField] GameObject rod;

    public enum CharacterStatus
    {
        Free,
        Gathering,
        RunningToResource,
        RunningToTarget,
        Crafting,
        Unloading
    }

    public CharacterStatus status;

    [SerializeField] int maxVolume = 4;
    [SerializeField] int curVolume = 0;
    GameObject[] tools;
    NavMeshAgent agent;
    Transform resourceTransform;
    string resourceType; 
    int amount; 
    Transform targetTransform;
    float timeToPick = 0.5f;
    float timeToCut = 0.5f;
    string taskType;
    Animator animator;
    // Start is called before the first frame update

    private void Awake() {
        status = CharacterStatus.Free;
    }
    void Start()
    {
        tools = new GameObject[] {axe, pick, rod};
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Messenger<(string resourceType, string taskType, int amount, Transform targetTransform)>.AddListener(GameEvent.START_TASK, OnStartTask);
    }

    // Update is called once per frame
    void Update()
    {
        if (status == CharacterStatus.RunningToResource || status == CharacterStatus.RunningToTarget)
        {
            if (CheckReachedDestination()) 
            {
                if (status == CharacterStatus.RunningToResource)
                    GatheringResource();
                else
                    ProcessTarget();
            }
        }
    }

    private void GatheringResource()
    {
        status = CharacterStatus.Gathering;
        if (resourceType == "Wood")
        {
            animator.runtimeAnimatorController = woodingCtrl;
            axe.SetActive(true);
            Invoke("CutWood", timeToCut);
        }
        else if (resourceType == "Stone")
        {
            animator.runtimeAnimatorController = miningCtrl;
            pick.SetActive(true);
            Invoke("PickStone", timeToPick);
        }
        else if (resourceType == "WoodStorage")
        {
            curVolume = amount;
            animator.runtimeAnimatorController = gatheringCtrl;
            Messenger<(string resourceType, int amount)>.Broadcast(GameEvent.PICK_FROM_STORAGE, (resourceType, amount));
            Invoke("StartMovingTowardTarget", timeToPick);
        }
        else if (resourceType == "StoneStorage")
        {
            animator.runtimeAnimatorController = gatheringCtrl;
            curVolume = amount;
            Messenger<(string resourceType, int amount)>.Broadcast(GameEvent.PICK_FROM_STORAGE, (resourceType, amount));
            Invoke("StartMovingTowardTarget", timeToPick);
        }
    }

    void ProcessTarget()
    {
        if (curVolume > 0)
        {
            // animator.runtimeAnimatorController = gatheringCtrl;
            curVolume = 0;
            status = CharacterStatus.Free;
            animator.runtimeAnimatorController = idleCtrl;
            var message = (resourceType, taskType, amount, gameObject);
            Messenger<(string resourceType, string taskType, int amount, GameObject character)>.Broadcast(GameEvent.FINISH_TASK, message);
            // CheckNewResource();
        }
    }

    void PickStone()
    {
        curVolume++;
        if (curVolume == amount || curVolume == maxVolume)
            StartMovingTowardTarget();
        else
            Invoke("PickStone", timeToPick); 
    
    }

    void CutWood()
    {
        if (resourceTransform != null)
        {
            TreeCtrl treeCtrl = resourceTransform.gameObject.GetComponent<TreeCtrl>();
            treeCtrl.volume--;
            curVolume++;

            if (curVolume == amount || curVolume == maxVolume)
            {
                StartMovingTowardTarget();

                if (treeCtrl.volume <= 0)
                    resourceTransform.gameObject.SetActive(false);
            }
            else
            {
                if (treeCtrl.volume > 0)
                    Invoke("CutWood", timeToCut);     
                else if (treeCtrl.volume <= 0)
                {
                    resourceTransform.gameObject.SetActive(false);
                    CheckNewResource();
                }
            }
        }
        else 
            CheckNewResource();   
    }

    bool CheckReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnStartTask((string resourceType, string taskType, int amount, Transform targetTransform) message)
    {
        resourceType = message.resourceType; 
        amount = message.amount;
        taskType = message.taskType;
        targetTransform = message.targetTransform;
        status = CharacterStatus.RunningToResource;
        StartMovingTowardResource();
    }

    void CheckNewResource()
    {
        StartMovingTowardResource();
        // Invoke("StartMovingTowardResource", 0.05f);
    }

    void StartMovingTowardTarget()
    {
        GetComponent<Animator>().runtimeAnimatorController = runingCtrl;
        status = CharacterStatus.RunningToTarget;
        agent.SetDestination(targetTransform.position);
        for (int i = 0; i<tools.Length; i++){
            tools[i].SetActive(false);
        }
    }

    void StartMovingTowardResource()
    {
        if (resourceType == "WoodStorage" || resourceType == "StoneStorage")
        {
            GetComponent<Animator>().runtimeAnimatorController = runingCtrl;
            status = CharacterStatus.RunningToResource;
            resourceTransform = GameObject.FindWithTag(resourceType).transform;
            agent.SetDestination(resourceTransform.position);
        }
        else
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag(resourceType);
            if (resources.Length == 0)
            {
                GetComponent<Animator>().runtimeAnimatorController = idleCtrl;
                status = CharacterStatus.Free;
                Invoke("StartMovingTowardResource", 1f);
            }
            else
            {
                GetComponent<Animator>().runtimeAnimatorController = runingCtrl;
                status = CharacterStatus.RunningToResource;
                resourceTransform = GetClosestResource(resources, resourceType);
                agent.SetDestination(resourceTransform.position);
            }
        }
    }

    Transform GetClosestResource(GameObject[] resources, string resourceType)
    {
        resourceTransform = null;
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in resources)
        {
            Transform targetTransform = potentialTarget.transform;
            Vector3 directionToTarget = targetTransform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (resourceType == "Wood")
            {
                TreeCtrl potentialTreeCtrl = potentialTarget.GetComponent<TreeCtrl>();
                if (dSqrToTarget < closestDistanceSqr && potentialTreeCtrl.freeVolume > 0)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                    // potentialTreeCtrl.freeVolume -= amount;
                }
            }
            else
            {
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }

        }

        if (resourceType == "Wood" && bestTarget != null)
        {
            bestTarget.GetComponent<TreeCtrl>().freeVolume -= amount;
        }

        return bestTarget;
    }
}
