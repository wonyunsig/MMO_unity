using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class MonsterController : BaseController
{
    private Stat _stat;
    [SerializeField] private float _scanRange = 8;
    [SerializeField] private float _attackRange = 1;

    private void Start()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving; 
            return;
        }
    }

    protected override void UpdateMoving()
    {
        GameObject player = Managers.Game.GetPlayer();
        float distance2 = (player.transform.position - transform.position).magnitude;
        //플레이어가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                NavMeshAgent nma2 = gameObject.GetOrAddComponent<UnityEngine.AI.NavMeshAgent>();
                nma2.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }
        
        if (distance2 > _scanRange)
        {
            NavMeshAgent nma2 = gameObject.GetOrAddComponent<UnityEngine.AI.NavMeshAgent>();
            nma2.SetDestination(transform.position);
            State = Define.State.Idle; 
            return;
        }
        
        //목적지 도착하면 정지
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            return;
        }
        
        //이동
        UnityEngine.AI.NavMeshAgent nma = gameObject.GetOrAddComponent<UnityEngine.AI.NavMeshAgent>();
        nma.SetDestination(_destPos);
        nma.speed = _stat.MoveSpeed;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 50 * Time.deltaTime);
        
    }
    protected override void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);

            if (targetStat.Hp <= 0)
            {
                Managers.Game.Despawn(targetStat.gameObject);
            }
            if (targetStat.Hp > 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                    State = Define.State.Skill;
                else
                    State = Define.State.Moving;

            }
            else
            {
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
        }
    }
}
