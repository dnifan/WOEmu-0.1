using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Objects;
using System.Text;
using WOEmu.Packets;

namespace WOEmu.PacketHandlers
{
    public static class Movement
    {
        private class MovementBlock
        {
            public float x, y, z, rot;
            public byte a, b;

            public float realRotation;
        }

        public static float Delta(float a, float b)
        {
            float res = a - b;
            if (res < 0)
                return -res;
            else
                return res;
        }

        public static void Read(Client c, byte[] Packet)
        {
            PacketReader reader = new PacketReader(Packet);

            reader.PopByte(); //ID

            List<MovementBlock> predictedMovements = new List<MovementBlock>();

            //we don't want movement prediction.. I guess..
            //so in fact there is still 30 params behind this.

            float oldX = c.player.Position.X;
            float oldY = c.player.Position.Y;
            float oldRot = c.player.Rotation;

            for (int i = 0; i < 6; i++)
            {
                MovementBlock blk = new MovementBlock();

                blk.x = reader.PopFloat();
                blk.y = reader.PopFloat();
                blk.z = reader.PopFloat();
                blk.rot = reader.PopFloat();

                blk.a = reader.PopByte();
                blk.b = reader.PopByte();

                blk.realRotation = (((float)blk.rot * 360.0f) / 256.0f);
                //make sure the client saves the new coordinates.
                c.player.Position.Set(blk.x, blk.y, blk.z);
                predictedMovements.Add(blk);
            }

            //predictedMovements[0] is current position, so we don't need that.
            //predictedMovements[1], let's pick that.
            
            float x = predictedMovements[5].x;
            float y = predictedMovements[5].y;
            float z = predictedMovements[5].z;
            float realRotation = (((float)predictedMovements[5].realRotation * 360.0f) / 256.0f);

            //make sure the client saves the new coordinates.
            c.player.Position.Set(x, y, z);
            c.player.Rotation = realRotation;

            sbyte relativeXOffset = (sbyte)(((x - oldX) * (c.player.Speed + 10)));
            sbyte relativeYOffset = (sbyte)(((y - oldY) * (c.player.Speed + 10)));
            sbyte relativeRotOffset = (sbyte)(realRotation - oldRot);

            UpdatePosition.Broadcast(c.player, (sbyte)relativeRotOffset, new Vector3(relativeXOffset, relativeYOffset, 0));
        }
    }
}



