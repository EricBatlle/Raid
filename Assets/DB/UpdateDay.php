<?php
	//Include other php files
	include('DBConst.php');

	//Variables submitted
	$newDayModel = $_POST["newDayModel"];
	$newDayModel = json_decode($newDayModel, true);
	$newRaider = $_POST["newRaider"];
	$newRaider = json_decode($newRaider, true);

	//Create DB connection
	$conn = new mysqli($GLOBALS['serverName'], $GLOBALS['username'], $GLOBALS['password'], $GLOBALS['dbname']);

	//Check connection
	if($conn->connect_error)
	{
		echo WebResponse::$ERROR;
		die("connection failed ".$conn->connect_error);		
	}

	//Create Query to insert new raider recived
	$sqlQuery = "UPDATE raider_day SET isRotative = '".$newDayModel["isRotative"]."', assistanceSelected = '".$newDayModel["assistanceSelected"]."', lateTime = '".$newDayModel["lateTime"]."'"; 
	$sqlQuery .= "WHERE dayID = '".$newDayModel["dayID"]."' AND raiderID = '".$newRaider["raiderID"]."';"; 

	//Validate query	
	if($conn->query($sqlQuery) == TRUE)
	{					
		echo WebResponse::$OK;		
	}
	else
	{
		//echo mysqli_error($conn);
		echo WebResponse::$ERROR;
	}

	//Close connection with DB
	$conn->close();	
?>

