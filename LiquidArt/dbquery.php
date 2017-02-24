<?php

function executeQuery($query)
{
	if ($query == null)
		die("Query has not been provided");

	$config = (require_once 'configuration.php');
	$dbConfig = $config["databaseConnection"];

	$database = $dbConfig["database"];
	$username = $dbConfig["username"];
	$password = $dbConfig["password"];
	$hostname = $dbConfig["hostname"];

	//connection to the database
	$link = mysqli_connect($hostname, $username, $password, $database) 
		or die("Unable to connect to MySQL");

	//execute the SQL query and return records
	$queryResult = mysqli_query($link, $query);

	if (!$queryResult) {
		printf("Error: %s\n", mysqli_error($link));
		exit();
	}

	$result = array();
	while ($row = mysqli_fetch_array($queryResult)) 
		$result[] = $row;

	//close the connection
	mysqli_close($link);
		
	return $result;
}

?>