<?php

require_once('session.php');
require_once('logger.php');

class SetStateWatered_Command implements DevCommand
{
    /**
     * Manually changes the session state to WATERED. 
     */
    public function Execute()
    {
        $session = Session::Load();
        $session->ChangeState(new Watered_State());
        Logger::Debug('Session state manually changed to "Watered" state.');
    }
}

?>