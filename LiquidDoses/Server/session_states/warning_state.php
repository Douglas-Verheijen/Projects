<?php

class Warning_State extends Session_State
{
    /**
     * Logs a "Warning" message when it initially transitions.
     * @param Session $session 
     * @param int $moisture 
     */
    public function Init($session, $moisture)
    {
        Logger::Debug("Moisture: ".$moisture." - I could probably use a drink");
    }
    
    /** 
     * Changes the session to DRY when moisture reach 25% again. Otherwise,
     * changes the session back to WATERED.
     * @param Session $session 
     * @param int $moisture 
     */
    public function Update($session, $moisture)
    {
        if ($moisture <= 25)
        {
            $state = new Dry_State();
            $state->Init($session, $moisture);
            $session->ChangeState($state);
        }
        else
        {
            Logger::Debug("Moisture: ".$moisture." - Nevermind, I'm not thirsty yet.");
            $state = new Watered_State();
            $session->ChangeState($state);
        }
    }
}

?>