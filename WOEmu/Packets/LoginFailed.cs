using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public enum FailReason
    {
        WrongPassword,
        NonExistingUser,
    }

    public static class LoginFailed
    {
        public static void SendTo(Client c, FailReason f)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(0);

            //login fialed
            w.PushByte(0);

            switch (f)
            {
                case FailReason.WrongPassword:
                    w.PushShort((short)WrongPasswordMsg.Length);
                    w.PushBytes(Encoding.ASCII.GetBytes(WrongPasswordMsg));
                    break;

                case FailReason.NonExistingUser:
                    w.PushShort((short)NonExistingUserMsg.Length);
                    w.PushBytes(Encoding.ASCII.GetBytes(NonExistingUserMsg));
                    break;

                default:
                    throw new Exception("Unknown FailReason specified in LoginFailed.SendTo()!");
            }

            w.PushFloat(0.0f);
            w.PushFloat(0.0f);
            w.PushFloat(0.0f);
            w.PushFloat(0.0f);

            w.PushByte(0);

            w.PushLong(0);
            w.PushLong(0);

            w.PushByte((byte)BrokenModel.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(BrokenModel));

            w.PushByte(0);
            w.PushByte(0);

            w.PushShort(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        private static string WrongPasswordMsg = "You provided an incorrect password.";
        private static string NonExistingUserMsg = "The username you provided does not exist!";
        private static string BrokenModel = "model.broken";
    }
}
