//-------------------------
//Alien Artifacts Kit (Lite)
//by MobileJuze Pty Ltd
//www.mobilejuze.com.au
//-------------------------

#pragma strict

//This is a simple script to handle changing objects based on mouse clicks

//Let's just get an inventory of what objects are in the scene based on their tag

//Barth
var theBarths : GameObject[];
theBarths = GameObject.FindGameObjectsWithTag("Barth");
//DiPyramid
var theDiP : GameObject[];
theDiP = GameObject.FindGameObjectsWithTag("DiPyramid");
//GeodesicA
var theGeo : GameObject[];
theGeo = GameObject.FindGameObjectsWithTag("GeodesicA");
//Tetra
var theTetra : GameObject[];
theTetra = GameObject.FindGameObjectsWithTag("Tetra");
//UWhat
var theWhat : GameObject[];
theWhat = GameObject.FindGameObjectsWithTag("UWhat");
//Infinity
var theInf : GameObject[];
theInf = GameObject.FindGameObjectsWithTag("Infinity");

//Let's find the particles which will display when objects are changed
var theParticles : GameObject;
theParticles = GameObject.Find("ParticleHolder");

function Start () 
	{
	//Hide all the objects in the "Example Variations" area
	this.HideAll();
	}

function Update () 
	{
	}

function HideAll()
	{
	//This hides all the objects with a particular tag
	//Note that the six "main" objects are NOT tagged
	//Only the objects in the "Example Variations" area are tagged
	for(var fooObj1 : GameObject in theBarths)
		fooObj1.SetActive(false);
	for(var fooObj2 : GameObject in theDiP)
		fooObj2.SetActive(false);
	for(var fooObj3 : GameObject in theGeo)
	 	fooObj3.SetActive(false);
	for(var fooObj4 : GameObject in theTetra)
	 	fooObj4.SetActive(false);
	for(var fooObj5 : GameObject in theWhat)
	 	fooObj5.SetActive(false);
	for(var fooObj6 : GameObject in theInf)
	 	fooObj6.SetActive(false);
	}

function ChangeModels(theType: String)
	{
	//This will change the objects that are shown in the "Example Variations" area
	
	//Show particle effect (burst)
	theParticles.GetComponent(ParticleSystem).Play();

	//Evaluate the message received from the object that was clicked
	//Hide all objects, then show ONLY the suitable objects of a certain tag
	switch(theType)
		{
		case "Barth":
			this.HideAll();
			//Show Barth examples, move it to the front appropriately (Z Axis = 0)
			for(var fooObj1 : GameObject in theBarths)
				{
				fooObj1.SetActive(true);
				fooObj1.transform.position.z = 0;
				}
		break;
		case "Tetra":
			this.HideAll();
			//Show Tetra examples
			for(var fooObj1 : GameObject in theTetra)
				{
				fooObj1.SetActive(true);
				fooObj1.transform.position.z = 0;
				}
		break;
		case "DiPyramid":
			this.HideAll();
			//Show DiPyramid examples
			for(var fooObj1 : GameObject in theDiP)
				{
				fooObj1.SetActive(true);
				fooObj1.transform.position.z = 0;
				}
		break;
		case "UWhat":
			this.HideAll();
			//Show UWhat examples
			for(var fooObj1 : GameObject in theWhat	)
				{
				fooObj1.SetActive(true);
				fooObj1.transform.position.z = 0;
				}
		break;
		case "GeodesicA":
			this.HideAll();
			//Show GeoDesicA examples
			for(var fooObj1 : GameObject in theGeo	)
				{
				fooObj1.SetActive(true);
				fooObj1.transform.position.z = 0;
				}
		break;
		case "Infinity":
			this.HideAll();
			//Show Infinity examples
			for(var fooObj1 : GameObject in theInf	)
				{
				fooObj1.SetActive(true);
				fooObj1.transform.position.z = 0;
				}
		break;
		}

	}