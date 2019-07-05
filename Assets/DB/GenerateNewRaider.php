<?php
	//Include other php files
	include('DBConst.php');

	//Variables submitted
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
	//ToDo: VERIFY IF THE USERNAME ALREADY EXISTS!
	//Create Query to insert new raider recived
	$sqlQuery ="INSERT INTO raider (username, password, name, mainClass, mainSpec, offSpec, daysRotated) VALUES ";	
	$sqlQuery .= "('".$newRaider["username"]."', '".$newRaider["password"]."', '".$newRaider["name"]."', '".$newRaider["mainClass"]."', '".$newRaider["mainSpec"]."', '".$newRaider["offSpec"]."' , '".$newRaider["daysRotated"]."');"; 

	//Validate query	
	if($conn->query($sqlQuery) == TRUE)
	{					
		//Create Query to generate connections between raider and days		
		//ToDo: Maybe this is not the safest way to get raiderID...chek it out
		$sqlQuery = "INSERT INTO raider_day (dayID, raiderID) 
					SELECT dayID, raiderID
					FROM day, raider
					WHERE raiderID";		
		$sqlQuery .= "= (SELECT raiderID FROM raider ORDER BY raiderID DESC LIMIT 1)";

		//Validate the query	
		if($conn->query($sqlQuery) == TRUE)
		{			
			//Create Query to generate connections between raider and accepted_raiders day list	
			$sqlQuery = "INSERT INTO accepted_raiders (dayID, raiderID) 
					SELECT dayID, raiderID
					FROM day, raider
					WHERE raiderID";		
			$sqlQuery .= "= (SELECT raiderID FROM raider ORDER BY raiderID DESC LIMIT 1)";

			//Validate the query	
			if($conn->query($sqlQuery) == TRUE)
			{			
				echo WebResponse::$OK;
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
	}
	else
	{
		echo WebResponse::$ERROR_REGISTER_DUPLICATE_USERNAME;
	}

	//Close connection with DB
	$conn->close();	
?>

