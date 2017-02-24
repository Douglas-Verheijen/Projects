<?php

require_once('controller.php');
require_once('models/indexModel.php');

class IndexController extends Controller
{
	protected function getHeader()
	{
		$page_header = parent::getHeader();
		$page_header .= 
			"<script type='text/javascript' src='http://requirejs.org/docs/release/2.2.0/minified/require.js' data-main='scripts/index'></script>
			<link rel='stylesheet' type='text/css' href='styles/index.css'>";

		return $page_header;
	}

	protected function getView() 
	{
		return 'views/indexView.php';
    }

	protected function getModel()
	{
		$models = array();
		$spaghetti = executeQuery("SELECT * FROM Artist");
		foreach ($spaghetti as $noodles)
			$models[] = new IndexModel($noodles);

		return $models;
	}
}

$controller = new IndexController();
echo $controller->execute();

?>