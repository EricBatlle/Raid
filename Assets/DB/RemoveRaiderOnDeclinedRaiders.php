<?php
	//Include other php files
	include('DBConst.php');

	//Variables submitted
	$dayID = $_POST["dayID"];
	$raiderID = $_POST["raiderID"];

	//Create DB connection
	$conn = new mysqli($GLOBALS['serverName'], $GLOBALS['username'], $GLOBALS['password'], $GLOBALS['dbname']);

	//Check connection
	if($conn->connect_error)
	{
		echo WebResponse::$ERROR;
		die("connection failed ".$conn->connect_error);		
	}
	
	//Create SQL query and link it to the DB
	$sqlQuery = "DELETE FROM declined_raiders WHERE raiderID = '".$raiderID."' AND dayID = '".$dayID."'";
	$result = $conn->query($sqlQuery);

	//Manage Query response
	if($result == TRUE)
	{		
		echo WebResponse::$OK;
	}
	else
	{
		echo WebResponse::$ERROR;
	}

	$conn->close();
?>

