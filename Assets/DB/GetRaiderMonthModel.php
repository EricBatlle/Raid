<?php
	//Include other php files
	include('DBConst.php');

	//Variables submitted
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
	$sqlQuery ="SELECT * FROM raider_day WHERE raiderID = '".$raiderID."'";	
	$result = $conn->query($sqlQuery);
	
	$json_array = array();
	$day_array = array();
	$daysID_array = array();
	$raid_day_array = array();

	//Manage Query response
	if($result->num_rows > 0)
	{		
		//Read information from raider_day table and store raiderID's related to the days = month
		while($row = $result->fetch_assoc()) 
		{			
			$raid_day_array[] = $row;
			array_push($daysID_array,$row["dayID"]);
		}	
		//With that information, get all day info
		$daysID_array_string = "'" .implode("','", $daysID_array  ) . "'"; 
		$sqlQuery ="SELECT * FROM day WHERE dayID IN ($daysID_array_string)";	
		$result = $conn->query($sqlQuery);
		//Manage Query response
		if($result->num_rows > 0)
		{
			while($row = $result->fetch_assoc()) 
			{			
				$day_array[] = $row;
			}	
			//And then join the info
			for($i = 0; $i < sizeof($day_array); $i++)
			{
				$day_array[$i]["isRotative"] = $raid_day_array[$i]["isRotative"];
				$day_array[$i]["assistanceSelected"] = $raid_day_array[$i]["assistanceSelected"];
				$day_array[$i]["lateTime"] = $raid_day_array[$i]["lateTime"];
			}		
			echo json_encode($day_array);
		}
		else
		{
			echo WebResponse::$ERROR_0RESULTS;
		}
	}
	else
	{
		echo WebResponse::$ERROR_0RESULTS;
	}

	//Close connection with DB
	$conn->close();	
?>

