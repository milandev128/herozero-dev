using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIResponse
{
    public string status { get; set; }
    public string message { get; set; }
}

public class LoginRequest
{
    public string name { get; set; }
    public string password { get; set; }
}

public class LoginResponse
{
    public string status { get; set; }
    public string message { get; set; }
    public string token { get; set; }
    public UserInfo user { get; set; }
}

public class RegisterRequest
{
    public string name { get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public int skin { get; set; }
}

public class GetUserInfoResponse
{
    public string status { get; set; }
    public string message { get; set; }
    public UserInfo user { get; set; }
}

public class UserInfo
{
    public string _id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public int skin { get; set; }
    public int level { get; set; }
    public int coins { get; set; }
    public int strength { get; set; }
    public int dodge { get; set; }
    public object[] items { get; set; }
    public int __v { get; set; }
}

public class ItemRequest
{
    public string name { get; set;}
    public int strength { get; set;}
    public int dodge { get; set;}
}