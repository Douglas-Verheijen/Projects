<?php

require_once('session.php');
require_once('logger.php');

class SetStateWarning_Command implements DevCommand
{
    /**
     * Manually changes the session state to WARNING. 
     */
    public function Execute()
    {
        $session = Session::Load();
        $session->ChangeState(new Warning_State());
        Logger::Debug('Session state manually changed to "Warning" state.');
    }
}

?>