<?php

require_once('controller.php');
require_once('models/artistModel.php');

class ArtistController extends Controller
{
	protected function getHeader()
	{
		$page_header = parent::getHeader();
		$page_header .= 
			"<script type='text/javascript' src='http://requirejs.org/docs/release/2.2.0/minified/require.js' data-main='scripts/artist'></script>
			<link rel='stylesheet' type='text/css' href='styles/artist.css'>";

		return $page_header;
	}

	protected function getView() 
	{
        return 'views/artistView.php';
    }
	
	protected function getModel() 
	{
		$query = $this->getQueryStringParams();
		$artistId = $query['artist'];
		
		$spaghetti = executeQuery("SELECT * FROM Artist WHERE Id = ".$artistId." LIMIT 1");
		$model = new ArtistModel($spaghetti[0]);
			
		$models = array();	
		$spaghetti = executeQuery("SELECT * FROM Art WHERE Artist_Id = ".$artistId);
		foreach ($spaghetti as $noodles)
			$models[] = new ArtModel($noodles);
		
		$model->Art = $models;

		foreach ($model->Art as $art) 
		{
			$size = getimagesize($art->Thumbnail);
			if (!isset($model->MaxHeight) || $model->MaxHeight > $size[1])
				$model->MaxHeight = $size[1];
		}


		return $model;
	}
}

$controller = new ArtistController();
echo $controller->execute();

?>