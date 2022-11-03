using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO3;
using UnityEngine;

public class SocketClient : MonoBehaviour {

  private void Start () {
    Debug.Log ("Socket connecting...");
    var manager = new SocketManager(new Uri("http://localhost:3000"));
    manager.Socket.On("connect", () => {
      Debug.Log("Socket connected: " + manager.Handshake.Sid);
    });
  }
}