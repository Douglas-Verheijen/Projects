<?php

require_once('session.php');
require_once('logger.php');

class SetStateDry_Command implements DevCommand
{
    /**
     * Manually changes the session state to DRY. 
     */
    public function Execute()
    {
        $session = Session::Load();
        $session->ChangeState(new Dry_State());
        Logger::Debug('Session state manually changed to "Dry" state.');
    }    
}

?>