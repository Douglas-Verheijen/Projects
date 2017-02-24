<?php

foreach (glob("commands/*.php") as $filename)
    require_once($filename);

interface ICommand
{
    public function execute();
}

?>