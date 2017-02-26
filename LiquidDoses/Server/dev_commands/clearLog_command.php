<?php

require_once('logger.php');

class ClearLog_Command implements DevCommand
{
    /**
     * Clears the transaction logs. 
     */
    public function Execute()
    {
        Logger::Clear();
    }
}

?>