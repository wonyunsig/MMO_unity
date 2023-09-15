using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _speed = 3.0f;
    
    private Vector3 _destPos;
    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        //UI_Button button = Managers.UI.ShowPopupUI<UI_Button>();
        Managers.UI.ShowSceneUI<UI_Inven>();
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
        anim.SetFloat("speed", 0);
    }

    private void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.00001f)
        {
            _state = PlayerState.Idle;
            return;
        }
        
        //이동
        float moveDist = Math.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
        transform.position += dir.normalized * moveDist;
        if (dir.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir),
                10 * Time.deltaTime);
        }
        
        //애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _speed);
    }

    private void UpdateDie()
    {
        Debug.Log("Player die!!!!!");
    }

    private void OnMouseClicked(Define.MouseEvent obj)
    {
        if (_state == PlayerState.Die)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Wall")))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}