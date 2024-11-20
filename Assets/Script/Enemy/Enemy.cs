using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;
public enum NPCState { Searching, Collecting, Building }
public class Enemy : CharacterBase
{
    public NavMeshAgent agent;
    public float searchRadius = 100f;
    private Brick targetBrick;
    private int maxBricksToCollect;
    public NPCState currentState = NPCState.Searching;

    private StairsCheck targetStairs;
    public Transform final;
    public bool Play = false;
    public EnemyStateChinge stateMachinge { get; private set; }

    public EnemyIdle enemyIdleState { get; private set; }
    public EnemyMove enemyMoveState { get; private set; }
    public EnemyNhayState enemyNhayState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachinge = new EnemyStateChinge();
        enemyIdleState = new EnemyIdle(this, stateMachinge, "Idle");
        enemyMoveState = new EnemyMove(this, stateMachinge, "Move");
        enemyNhayState = new EnemyNhayState(this, stateMachinge, "Nhay");
    }
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();

        //StartCoroutine(StateMachine());
        stateMachinge.Initialize(enemyIdleState);
    }
    protected override void Update()
    {
        base.Update();
        if (Play == true)
        {
            switch (currentState)
            {
                case NPCState.Searching:
                    FindBrick();
                    break;

                case NPCState.Collecting:
                    MoveToBrick();
                    break;

                case NPCState.Building:
                    MoveToStairs();
                    break;
            }
            CheckStairs();
        }
        stateMachinge.currentState.Update();
    }
    public void CheckStairs()
    {
        if (Physics.Raycast(checkStair.transform.position, Vector3.down, out RaycastHit hit, checkStairDistance, lmStair))
        {
            if (hit.transform == null) return;

            if (hit.transform.TryGetComponent(out StairsCheck stairBrick))
            {
                if (stairBrick == null || listBrickHiden.Count < 1) return;
                Brick brick = listBrickHiden[listBrickHiden.Count - 1];
                if (stairBrick.color == color) return;

                stairBrick.AddBrick(brick);
                stairBrick.ChangeStairColor(color);
                RemoveBrick(brick);
            }
        }
    }
    // State machine to control behavior
    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case NPCState.Searching:
                    FindBrick();
                    break;

                case NPCState.Collecting:
                    MoveToBrick();
                    break;

                case NPCState.Building:
                    MoveToStairs();
                    break;
            }
            yield return null;
        }
    }
    public void EnemyFinal()
    {
        stateMachinge.ChangeSate(enemyNhayState);
    }
    // Tìm viên gạch gần nhất cùng màu
    public void FindBrick()
    {
        if (currentState != NPCState.Searching) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        float closestDistance = Mathf.Infinity;
        targetBrick = null;

        foreach (Collider collider in colliders)
        {
            Brick brick = collider.GetComponent<Brick>();
            if (brick != null && brick.color == this.color && brick.gameObject.activeInHierarchy)
            {
                Vector3 directionToBrick = brick.transform.position - transform.position;
                float distance = directionToBrick.magnitude;

                if (distance < closestDistance && !IsBrickBehindNPC(brick, directionToBrick))
                {
                    closestDistance = distance;
                    targetBrick = brick;
                    currentState = NPCState.Collecting;
                }
            }
        }

        if (targetBrick != null)
        {
            currentState = NPCState.Collecting;
        }
    }

    // Phương thức hỗ trợ kiểm tra viên gạch có nằm sau NPC không
    private bool IsBrickBehindNPC(Brick brick, Vector3 directionToBrick)
    {
        float dotProduct = Vector3.Dot(transform.forward, directionToBrick.normalized);

        return dotProduct < 0;
    }

    // Di chuyển đến viên brick và thu thập
    public void MoveToBrick()
    {
        if (targetBrick == null || listBrickHiden.Count == maxBricksToCollect)
        {
            currentState = listBrickHiden.Count == maxBricksToCollect ? NPCState.Building : NPCState.Searching;
            return;
        }

        agent.SetDestination(targetBrick.transform.position);

        if (Vector3.Distance(transform.position, targetBrick.transform.position) < 0.5f)
        {
            targetBrick = null;
        }
    }

    // tìm cầu thang
    private void FindNearestStairs()
    {
        StairsCheck[] allStairs = FindObjectsOfType<StairsCheck>();

        // Bước 1: Tìm cầu thang cùng màu gần điểm final nhất
        StairsCheck sameColorStairs = null;
        float closestSameColorDistance = Mathf.Infinity;

        foreach (StairsCheck stairs in allStairs)
        {
            if (stairs.color != this.color) continue; // Chỉ quan tâm cầu thang cùng màu

            float distanceToFinal = Vector3.Distance(stairs.transform.position, final.position);
            if (distanceToFinal < closestSameColorDistance)
            {
                closestSameColorDistance = distanceToFinal;
                sameColorStairs = stairs;
            }
        }

        // Bước 2: Tìm cầu thang khác màu ở vị trí cao hơn cầu thang cùng màu
        if (sameColorStairs != null) // Nếu đã tìm thấy cầu thang cùng màu
        {
            StairsCheck differentColorStairs = null;
            float closestDifferentColorDistance = Mathf.Infinity;

            foreach (StairsCheck stairs in allStairs)
            {
                if (stairs.color == this.color) continue; // Bỏ qua cầu thang cùng màu
                if (stairs.transform.position.y <= sameColorStairs.transform.position.y) continue; // Chỉ tìm cầu thang khác màu cao hơn

                float distanceToSameColorStairs = Vector3.Distance(stairs.transform.position, sameColorStairs.transform.position);
                if (distanceToSameColorStairs < closestDifferentColorDistance)
                {
                    closestDifferentColorDistance = distanceToSameColorStairs;
                    differentColorStairs = stairs;
                }
            }

            // Nếu tìm thấy cầu thang khác màu, lấy nó làm target
            if (differentColorStairs != null)
            {
                targetStairs = differentColorStairs;
                return;
            }
        }

        // Bước 3: Nếu không tìm thấy cầu thang cùng màu hoặc cầu thang khác màu từ cầu thang cùng màu, tìm cầu thang khác màu gần NPC nhất
        targetStairs = null;
        float closestDistance = Mathf.Infinity;

        foreach (StairsCheck stairs in allStairs)
        {
            if (stairs.color == this.color) continue; // Bỏ qua cầu thang cùng màu

            float distanceToNPC = Vector3.Distance(transform.position, stairs.transform.position);
            if (distanceToNPC < closestDistance)
            {
                closestDistance = distanceToNPC;
                targetStairs = stairs;
            }
        }

        if (targetStairs == null)
        {
            currentState = NPCState.Searching;
        }
        else
        {
        }
    }

    // Di chuyển đến cầu thang
    public void MoveToStairs()
    {
        if (targetStairs == null || targetStairs.color == this.color)
        {
            FindNearestStairs();
            // Debug.Log(agent.velocity.sqrMagnitude);
        }

        if (targetStairs != null && targetStairs.color != this.color)
        {
            agent.SetDestination(targetStairs.transform.position);
            if (Vector3.Distance(transform.position, targetStairs.transform.position) < 2f)
            {
                if (listBrickHiden.Count <= 0)
                {
                    RandomizeMaxBricks();
                    currentState = NPCState.Collecting;
                    targetStairs = null;
                }
                else
                {
                    targetStairs = null;
                    currentState = NPCState.Building;
                }
            }
        }
        else
        {
            FindNearestStairs();
        }
    }
    public void StopAllActions()
    {

            agent.isStopped = true;
            agent.ResetPath();

            stateMachinge.ChangeSate(enemyNhayState);  
        agent.enabled = false;
        gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        ClearBrick();
        agent.speed = 0;
    }
    public void ResumeActions()
    {
        agent.enabled = true;
        agent.isStopped = false;
            agent.speed = 5; // Hoặc tốc độ bạn muốn
        
            stateMachinge.ChangeSate(enemyIdleState); 

        Play = true; 
    }

    // Randomize số lượng gạch cần thu thập
    public void RandomizeMaxBricks()
    {
        maxBricksToCollect = Random.Range(7, 13);
    }
}
