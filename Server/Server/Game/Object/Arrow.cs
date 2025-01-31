﻿using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class Arrow : Projectile
    {
        public GameObject Owner { get; set; }

        long _nextMoveTick = 0;

        public override void Update()
        {
            if (Data == null || Data.projectile == null || Owner == null || Room == null)
            {
                return;
            }

            if (_nextMoveTick >= Environment.TickCount64)
            {
                return;
            }

            long tick = (long)(1000 / Data.projectile.speed);
            _nextMoveTick = Environment.TickCount64 + tick;

            Vector2Int destination = GetFrontCellPosition();
            if (Room.Map.CanMove(destination))
            {
                CellPosition = destination;

                S_Move movePacket = new S_Move();
                movePacket.ObjectId = Id;
                movePacket.PositionInfo = PosInfo;
                Room.Broadcast(movePacket);

                Console.WriteLine("Move Arrow !!");
            }
            else
            {
                GameObject target = Room.Map.Find(destination);
                if (target != null)
                {
                    // TODO: 피격 판정.
                }

                // 소멸
                Room.LeaveGame(Id);
            }
        }
    }
}
