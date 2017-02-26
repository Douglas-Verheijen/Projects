<?php

foreach (glob("dev_commands/*.php") as $filename)
    require_once($filename);

interface DevCommand 
{
     /**
     * Executes the dev command.
     */
    public function Execute();
}

?>