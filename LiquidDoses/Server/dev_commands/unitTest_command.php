<?php

require_once('session.php');
require_once('logger.php');

class UnitTest_Command implements DevCommand
{
    /**
     * Performs a Unit Test on the basic functionaliy. 
     */
    public function Execute()
    {
        $session = Session::Load();
        $current = clone $session;

        Logger::Debug('   ***Performing Unit Test***');

        $session->Reset();
        $session->ChangeMode(Session_Mode::TEST);
        
        $session->Update(75);
        $session->Update(75);
        $session->Update(25);
        $session->Update(25);
        $session->Update(25);
        $session->Update(75);
        $session->Update(75);

        Logger::Debug('   ***Unit Test complete***');

        $current->Save();
    }
}

?>