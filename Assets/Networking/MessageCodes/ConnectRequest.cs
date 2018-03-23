﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ByteStream = UdpKit.UdpStream;


namespace MessageCode {
    public class ConnectRequest {
        public static void Process(ulong sender, params object[] args) {
            Debug.Log("On Rec ConnectRequest.Process");
            Core.net.OnConnectRequest(sender);
        }
    }

    public class ConnectRequestResponse {
        public static void Process(ulong sender, params object[] args) {
            Debug.Log("On Rec Connect Req Response");
            Core.net.OnConnectRequestResponse(sender, (int)args[0], (int)args[1]);
        }

        //
        public static void Serialize(ulong receiver, ByteStream stream, params object[] args) {

            int arg0 = (int)args[0]; //the connectionIndex of the player who sent you this message (the host)
            int arg1 = (int)args[1]; //the connectionIndex of you assigned by the host

            SerializerUtils.WriteInt(stream, arg0, 0, 255);
            SerializerUtils.WriteInt(stream, arg1, 0, 255);
        }

        public static void Deserialize(ulong sender, int msgCode, ByteStream stream) {

            int arg0 = SerializerUtils.ReadInt(stream, 0, 255);
            int arg1 = SerializerUtils.ReadInt(stream, 0, 255);

            //no need for a null check, can't have a deserializer without a processor.
            //I mean, you can, but it wouldn't do anything with the data you just received
            Core.net.MessageProcessors[msgCode](sender, arg0, arg1);
        }

        public static int Peek(params object[] args) {
            int s = 0;
            s += SerializerUtils.RequiredBitsInt(0, 255);
            s += SerializerUtils.RequiredBitsInt(0, 255);
            return s;
        }
    }
}