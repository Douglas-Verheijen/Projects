<?php

foreach (glob("twitter_commands/*.php") as $filename)
    require_once($filename);

interface TwitterCommand 
{     
    /**
     * Executes the Twitter command.
     * @param stdClass $data
     */
    public function Execute($data);
}

?>