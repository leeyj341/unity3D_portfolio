﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerState State;
    private CharacterController Controller;
    private AnimationController AnimController;
    private float h = 0.0f;
    private float v = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        State = GameManager.Instance.PS;
        Controller = GetComponent<CharacterController>();
        AnimController = GetComponent<AnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameTime.Equals(Constants.GameTime)) return;

        KeyInput();         // 키입력 
        Move();             // 움직임

        Die();
    }

    private void KeyInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (v != 0)
        {
            State.Speed = 10.0f;
        }
        else
        {
            State.Speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //일반 공격
            if (AnimController.CurAni.Equals(ANIM_SORT.SHOOT)) return;

            AnimController.ChangeAniSort(ANIM_SORT.ATTACK);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // 총 쏘기
            // 총이 없으면 안 쏘게
            //if (!GameManager.Instance.Inven.GetSubWeapon()) return;
            if (AnimController.CurAni.Equals(ANIM_SORT.ATTACK)) return;

            AnimController.ChangeAniSort(ANIM_SORT.SHOOT);
        }
    }

    private void Move()
    {
        transform.Rotate(transform.up * State.RotSpeed * h * Time.deltaTime);
        Controller.Move(transform.forward * State.Speed * v * Time.deltaTime);    
    }

    private void Die()
    {
        if (State.Hp <= 0.0f)
        {
            gameObject.SetActive(false);
        }
    }

    
}
