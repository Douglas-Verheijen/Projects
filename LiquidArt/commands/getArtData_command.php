<?php

class getArtData_Command implements ICommand
{
	public function execute()
	{
		$url = $_SERVER['REQUEST_URI'];	
		$parts = parse_url($url);
		parse_str($parts['query'], $queryString);
		$artistId = $queryString['artist'];
		
		$order = $_POST['order'];
		$sort = $_POST['sort'];
		
		$models = array();	
		$spaghetti = executeQuery("SELECT * FROM Art WHERE Artist_Id = ".$artistId." ORDER BY ".$order." ".$sort);
		foreach ($spaghetti as $noodles)
			$models[] = new ArtModel($noodles);
			
		$model = new \stdClass;
		$model->Art = $models;

		foreach ($model->Art as $art) 
		{
			$size = getimagesize($art->Thumbnail);
			if (!isset($model->MaxHeight) || $model->MaxHeight > $size[1])
				$model->MaxHeight = $size[1];
		}
		
        require_once('views/components/artistGalleryComponent.php');
	}
}

?>