<?php 

include '../dbquery.php';

$url = $_SERVER['REQUEST_URI'];	
$parts = parse_url($url);
parse_str($parts['query'], $queryString);

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

?>
<html>
    <body>
        <div>
            <?php echo $artData["Artist_Name"]; ?><br />
            <?php echo $artData["Art_Name"]; ?><br />
            <?php echo $artData["Artist_PhoneNumber"]; ?><br />
        </div>
    </body>
</html>