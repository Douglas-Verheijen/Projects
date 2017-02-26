<?php

require_once("twitter_status.php");

class Watered_State extends Session_State
{
    /**
     * Posts a "Watered" status to Twitter when it initially transitions. 
     * @param Session $session 
     * @param int $moisture 
     */
    public function Init($session, $moisture)
    {
        $twitterStatus = TwitterStatus::Load();
        $status = $twitterStatus->Send("watered");

        Logger::Debug("Moisture: ".$moisture." - ".$status);
    }
    
    /** 
     * Changes the session to WARNING when moisture reach 25%.   
     * @param Session $session 
     * @param int $moisture 
     */
    public function Update($session, $moisture)
    {
        if ($moisture <= 25)
        {
            $state = new Warning_State();
            $state->Init($session, $moisture);
            $session->ChangeState($state);
        }
        else
            Logger::Debug("Moisture: ".$moisture." - I'm having a good day.");
    }
}

?>