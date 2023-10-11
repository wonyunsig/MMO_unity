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
    }

    private PlayerState _state = PlayerState.Idle;
    
    private void Update()
    {
        switch (_state)
        {
            case PlayerState.Die :
                UpdateDie();
                break;
            case PlayerState.Moving :
                UpdateMoving();
                break;
            case PlayerState.Idle :
                UpdateIdle();
                break;
        }
    }


   private void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _stat.MoveSpeed);
    }

   private void UpdateMoving()
   {
       Vector3 dir = _destPos - transform.position;
       if (dir.magnitude < 0.1f)
       {
           _state = PlayerState.Idle;
       }
       else
       {
           float moveDist = Math.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);


           //이동
           //transform.position += dir.normalized * moveDist;
           NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
           nma.Move(dir.normalized * moveDist);
           
           Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
           if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
           {
               _state = PlayerState.Idle;
               return;
           }
           if (dir.magnitude > 0.01f)
           {
               transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime); 
           } 
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
    
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
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
                    _state = PlayerState.Moving;

                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;
                }
                break;
            case Define.MouseEvent.Press  :
                if (_lockTarget != null)
                {
                    _destPos = _lockTarget.transform.position;
                }
                else
                {
                    if (raycastHit)
                        _destPos = hit.point;
                }
                break;
        }
    }
}