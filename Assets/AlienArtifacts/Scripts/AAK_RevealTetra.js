//-------------------------
//Alien Artifacts Kit (Lite)
//by MobileJuze Pty Ltd
//www.mobilejuze.com.au
//-------------------------

//This script responds to a mouse click and sends a message to the "main script" to change the objects

#pragma strict

function Start () 
	{
	}

function Update () 
	{
	}

function OnMouseDown()
    {
    //Send a message to the "main script" to change the objects to those with a specific tag
    GameObject.Find("CodeHolder").SendMessage ("ChangeModels", "Tetra");
    }