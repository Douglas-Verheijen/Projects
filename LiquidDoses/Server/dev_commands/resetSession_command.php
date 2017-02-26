<?php

require_once('session.php');
require_once('logger.php');

class ResetSession_Command implements DevCommand
{
    /**
     * Resets the current session. 
     */
    public function Execute()
    {
        Session::Reset();
        Logger::Debug('Session manually reset.');
    }
}

?>