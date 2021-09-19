using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                GetDirInput();
                break;

            case CreatureState.Moving:
                GetDirInput();
                break;
        }

        base.UpdateController();
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void GetDirInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            Dir = MoveDir.None;
        }
    }

    protected override void UpdateIdle()
    {
        // Moving State�� ������ ���ΰ��� ����.
        if (Dir != MoveDir.None)
        {
            State = CreatureState.Moving;
            return;
        }

        // Skill State�� ������ ���ΰ��� ����.
        if (Input.GetKey(KeyCode.Space))
        {
            State = CreatureState.Skill;
            //_coSkill = StartCoroutine("CoStartPunch");
            _coSkill = StartCoroutine("CoStartShootArrow");
        }
    }

    protected override void MoveToNextPosition()
    {
        if (Dir == MoveDir.None)
        {
            State = CreatureState.Idle;
            CheckUpdatedFlag();

            return;
        }

        Vector3Int destination = CellPosition;
        switch (Dir)
        {
            case MoveDir.Up:
                destination += Vector3Int.up;
                break;

            case MoveDir.Down:
                destination += Vector3Int.down;
                break;

            case MoveDir.Left:
                destination += Vector3Int.left;
                break;

            case MoveDir.Right:
                destination += Vector3Int.right;
                break;
        }

        if (Managers.Map.CanMove(destination))
        {
            if (Managers.Object.FindCreature(destination) == null)
            {
                CellPosition = destination;
            }
        }

        CheckUpdatedFlag();
    }

    void CheckUpdatedFlag()
    {
        if (_updated)
        {
            C_Move movePacket = new C_Move();
            movePacket.PositionInfo = PositionInfo;
            Managers.Network.Send(movePacket);

            _updated = false;
        }
    }
}