<?php

require_once('command.php');
require_once('dbquery.php');

abstract class Controller
{
	protected function getHeader()
	{
		return 
			"<script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js'></script>
			<script type='text/javascript' src='scripts/featherlight.min.js'></script>
			<link rel='stylesheet' type='text/css' href='styles/featherlight.min.css'>
			<link rel='stylesheet' type='text/css' href='styles/generic.css'>
			<link rel='stylesheet' type='text/css' href='styles/site.css'>";
	}

	abstract protected function getView();
	
	abstract protected function getModel();

	protected function getQueryStringParams() 
	{
		$url = $_SERVER['REQUEST_URI'];	
		$parts = parse_url($url);
		parse_str($parts['query'], $queryString);
		return $queryString;
	}
	
	public function execute() 
	{
		if(isset($_POST['action']) && !empty($_POST['action'])) 
		{
			$action = $_POST['action'];

			if ($action == 'getModel')
				return $this->getModel();
			else if ($action == 'post')
				return $this->post();

			$action = $action."_Command";
   			if (class_exists($action))
			{
				$command = new $action();
				$command->execute();
				return;
			}
		}
		
		$page_header = $this->getHeader();
		$page_content = $this->getView();
		$model = $this->getModel();

		$this->display($page_header, $page_content, $model);
	}

	private function display($page_header, $page_content, $model)
	{
		require_once('views/_layout.php');
	}
} 

?>