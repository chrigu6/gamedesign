//-------------------------
//Alien Artifacts Kit (Lite)
//by MobileJuze Pty Ltd
//www.mobilejuze.com.au
//-------------------------

//This script makes the alien artifact rotate

#pragma strict

function Start () 
	{
	}

function Update () 
	{		
	}

function FixedUpdate ()
	{
	//Rotate this object slowly
	this.transform.Rotate(Vector3.up * 10 * Time.deltaTime);	
	}

