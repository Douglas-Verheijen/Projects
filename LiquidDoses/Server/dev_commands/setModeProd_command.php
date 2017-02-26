<?php

require_once('session.php');
require_once('logger.php');

class SetModeProd_Command implements DevCommand
{
    /**
     * Manually changes the session mode to PROD. 
     */
    public function Execute()
    {
        $session = Session::Load();
        $session->ChangeMode(Session_Mode::PROD);
        Logger::Debug('Session mode manually changed to PROD mode.');
    }
}

?>