﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{  
    protected NavMeshAgent agent = null;
    protected EnemyAniController aniController = null;

    protected Vector3 goal = Vector3.zero;
    protected Vector3 prevPos = Vector3.zero;

    protected MonsterStatus status = null;

    public MonsterStatus Status { get => status; set => status = value; }

    // Start is called before the first frame update
    protected void Start()
    {
        Init();
    }

    private void Init()                                                                             // 초기화
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = status.Speed;
        agent.acceleration = status.Speed;

        aniController = GetComponent<EnemyAniController>();
        aniController.SetAnimatorType(status.Type);
        aniController.UpdateAnimatorParameter(MONSTER_STATUS.PATROL);

        prevPos = transform.position;     
    }  

    protected bool InDistance(Vector3 myPos, Vector3 targetPos, float dis)                          // 지정된 거리 안에 타겟이 들어오면 true 리턴
    {
        if (Vector3.Distance(myPos, targetPos) <= dis)
            return true;
        else return false;
    }

    protected Vector3 RandomPosition(Vector3 myPos)                                                 // 순찰 범위 내 무작위 좌표 생성 
    {
        float posX = status.MyArea.Position.x;
        float posZ = status.MyArea.Position.z;
        float distance = status.MyArea.PatrolRange;

        float randomX = Random.Range(posX - distance, posX + distance);
        float randomZ = Random.Range(posZ - distance, posZ + distance);

        Vector3 v = new Vector3(randomX, 0.0f, randomZ);

        if (Physics.Linecast(myPos, v, out RaycastHit hitInfo) && hitInfo.transform.tag.Equals("Obstacle"))
        {
            Debug.DrawLine(myPos, v, Color.green, 100.0f);
            return prevPos;
        }

        else return v;
    }

    protected void ChangeCoroutine(IEnumerator coroutine)
    {
        StopAllCoroutines();
        StartCoroutine(coroutine);
    }

    protected bool FindPlayer(Vector3 myPos)
    {
        // 플레이어 쫒아가는 경우
        // - 범위에 있을 때
        // - 원거리 타격을 받았을 때

        // Idle, Patrol, Damaged
        if (InDistance(myPos, GameManager.Instance.PlayerTransfrom.position, status.RecognizedRange))
            return true;
        else return false;
    }

    protected bool IsPlayerInAttackRange(Vector3 myPos)
    {
        if (InDistance(myPos, GameManager.Instance.PlayerTransfrom.position, status.AttackRange))
            return true;
        else return false;
    }


    // -------------------------------------------------------------------------------------------------------------------
    // 재정의 coroutine 함수

    protected virtual IEnumerator Motion_Idle()
    {
        yield return null;
    }

    protected virtual IEnumerator Motion_Patrol()
    {
        yield return null;
    }

    protected virtual IEnumerator Motion_Chase()
    {
        yield return null;
    }

    protected virtual IEnumerator Motion_Attack()
    {
        yield return null;
    }
}
