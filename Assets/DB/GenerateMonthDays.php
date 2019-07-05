<?php
	//Include other php files
	include('DBConst.php');

	//Variables submitted
	$newMonth = $_POST["newMonth"];
	$newMonth_array = array();
	$newMonth_array = json_decode($newMonth,true);

	//Create DB connection
	$conn = new mysqli($GLOBALS['serverName'], $GLOBALS['username'], $GLOBALS['password'], $GLOBALS['dbname']);

	//Check connection
	if($conn->connect_error)
	{
		echo WebResponse::$ERROR;
		die("connection failed ".$conn->connect_error);		
	}
	
	//Create Query to insert all days recived
	$sqlQuery = "INSERT INTO day (numDay, numMonth, numYear, dayOfWeek, isRaideable) VALUES ";
	for($i = 0; $i < count($newMonth_array["array"])-1; $i++)
	{
		$sqlQuery.= "('".$newMonth_array["array"][$i]["numDay"]."', '".$newMonth_array["array"][$i]["numMonth"]."', '".$newMonth_array["array"][$i]["numYear"]."', '".$newMonth_array["array"][$i]["dayOfWeek"]."', '".$newMonth_array["array"][$i]["isRaideable"]."' ),";
	}
	$sqlQuery.= "('".$newMonth_array["array"][$i]["numDay"]."', '".$newMonth_array["array"][$i]["numMonth"]."', '".$newMonth_array["array"][$i]["numYear"]."', '".$newMonth_array["array"][$i]["dayOfWeek"]."', '".$newMonth_array["array"][$i]["isRaideable"]."' )";

	//Validate the query
	if($conn->query($sqlQuery) == TRUE)
	{		
		echo WebResponse::$OK;
	}
	else
	{
		echo $conn->error;
		echo WebResponse::$ERROR_0RESULTS;
	}
	
	//Close connection with DB
	$conn->close();
?>

