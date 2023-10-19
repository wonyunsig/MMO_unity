using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //public float _speed = 3.0f;
    private PlayerStat _stat;
    private Vector3 _destPos;

    void Start()
    {
        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

        //UI_Button button = Managers.UI.ShowPopupUI<UI_Button>();
        //Managers.UI.ClosePopupUI(button);
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill
    }

    [SerializeField]
    private PlayerState _state = PlayerState.Idle;

    PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die :
                    break;
                case PlayerState.Idle :
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Moving :
                    anim.CrossFade("RUN", 0.1f);
                    break;            
                case PlayerState.Skill :
                    anim.CrossFade("ATTACK", 0.1f);
                    break;
            }
        }
    }
    
    private void Update()
    {
        switch (State)
        {
            case PlayerState.Die :
                UpdateDie();
                break;
            case PlayerState.Idle :
                UpdateIdle();
                break;
            case PlayerState.Moving :
                UpdateMoving();
                break;            
            case PlayerState.Skill :
                UpdateSkill();
                break;
        }
    }

    private void UpdateSkill()
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
        Debug.Log("OnHitEvent");

        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            PlayerStat myStat = gameObject.GetComponent<PlayerStat>();

            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            Debug.Log("damage : " + damage);
            targetStat.Hp -= damage;
        }
        
        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
    }


    private void UpdateIdle()
    {
    }

   private void UpdateMoving()
   {
       if (_lockTarget != null)
       {
           float distance = (_destPos - transform.position).magnitude;
           if (distance <= 1)
           {
               State = PlayerState.Skill;
               return;
           }
       }
       
       Vector3 dir = _destPos - transform.position;
       if (dir.magnitude < 0.1f)
       {
           State = PlayerState.Idle;
           return;
       }
      
       float moveDist = Math.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
       //이동
       //transform.position += dir.normalized * moveDist;
       NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
       nma.Move(dir.normalized * moveDist);
       
       Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
       if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
       {
           if(Input.GetMouseButton(0) == false)
               State = PlayerState.Idle;
           return;
       }
       if (dir.magnitude > 0.01f)
       {
           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime); 
       }
   }

    private void UpdateDie()
    {
        Debug.Log("Player die!!!!!");
    }
    
    private int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    private GameObject _lockTarget;

    private bool _stopSkill = false;
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == PlayerState.Die)
            return;

        switch (State)
        {
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
            {
                if (evt == Define.MouseEvent.PointerUp)
                    _stopSkill = true;
            }
                break;
        }
    }
    private void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100, _mask);
        switch (evt)
        {
            case Define.MouseEvent.PointerDown :
                if (raycastHit)
                {
                    _destPos = hit.point;
                    State = PlayerState.Moving;
                    _stopSkill = false;

                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;
                }
                break;
            case Define.MouseEvent.Press  :
                if (_lockTarget != null & raycastHit)
                    _destPos = hit.point;
                break;
            case Define.MouseEvent.PointerUp :
                _stopSkill = true;
                break;
            
        }
    }
}