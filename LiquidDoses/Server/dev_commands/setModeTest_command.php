<?php

require_once('session.php');
require_once('logger.php');

class SetModeTest_Command implements DevCommand
{
    /**
     * Manually changes the session mode to TEST. 
     */
    public function Execute()
    {
        $session = Session::Load();
        $session->ChangeMode(Session_Mode::TEST);
        Logger::Debug('Session mode manually changed to TEST mode.');
    }
}

?>