<?php

require_once('logger.php');
require_once('session.php');

$url = $_SERVER['REQUEST_URI'];	
$parts = parse_url($url);
parse_str($parts['query'], $queryString);
$moisture = $queryString['moisture'];

try
{
    $session = Session::Load();
    $session->Update($moisture);
}
catch (Exception $e) 
{
    Logger::Error('Caught exception: '.$e->getMessage());
}

?>