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
                UpdateMoving();
                break;
            case PlayerState.Moving :
                UpdateIdle();
                break;            
            case PlayerState.Skill :
                UpdateSkill();
                break;
        }
    }

    private void UpdateSkill()
    {
        throw new NotImplementedException();
    }

    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");

        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }
        //Animator anim = GetComponent <Animator>();
        //anim.SetBool("attack", false);

        State = PlayerState.Idle;
    }


    private void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
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
           //애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _stat.MoveSpeed);
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
        if (State == PlayerState.Die)
            return;
        
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