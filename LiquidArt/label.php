<?php

require_once('controller.php');

class LabelController extends controller
{
    protected function getValue()
    {
        $queryString = $this->getQueryStringParams();
        
        $query = "SELECT A.Name 'Artist_Name',
                AA.Name 'Art_Name',
                A.PhoneNumber 'Artist_PhoneNumber'
            FROM Artist A 
                INNER JOIN Art AA ON A.Id = AA.Artist_Id
            WHERE AA.Id = ".$queryString['art']."
            LIMIT 1";
        $result = executeQuery($query);
        $artData = $result[0];
        
        $query = "SELECT Contents
            FROM Label
            WHERE Id = ".$queryString['label']."
            LIMIT 1";

        $result = executeQuery($query);
        $labelData = $result[0]["Contents"];
        
        $labelData = str_replace('{{Artist_Name}}', $artData['Artist_Name'], $labelData);
        $labelData = str_replace('{{Art_Name}}', $artData['Art_Name'], $labelData);
        $labelData = str_replace('{{Artist_PhoneNumber}}', $artData['Artist_PhoneNumber'], $labelData);
        
        return $labelData;
    }
    
    protected function getModel()
    {
        header('Content-Type: application/json');
        
        $queryString = $this->getQueryStringParams();
		$artId = $queryString['artId'];
        
        $query = "SELECT * 
            FROM Artist A 
                INNER JOIN Art AA ON A.Id == AA.Artist_Id
            WHERE AA.Id = ".$artId."
            LIMIT 1;";
            
        $result = executeQuery($query);
        
        return $result;
    }
}

$controller = new LabelController();
echo $controller->execute();

?>