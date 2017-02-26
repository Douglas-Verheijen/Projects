<?php

if (file_exists('appSettings.local.json'))
    return json_decode(file_get_contents('appSettings.local.json'));
return json_decode(file_get_contents('appSettings.json'));

?>