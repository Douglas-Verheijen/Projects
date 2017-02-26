<?php

require_once('dev_command.php');

$url = $_SERVER['REQUEST_URI'];	
$parts = parse_url($url);
parse_str($parts['query'], $queryString);
$action = $queryString['action'];

if (isset($action))
{
    $action .= "_Command";
    if (!class_exists($action))
        throw new Exception('Command Not Found: '.$action);

    $command = new $action();
    $command->Execute();
}
else
{ 
    $command = new UnitTest_Command();
    $command->Execute();
}

?>