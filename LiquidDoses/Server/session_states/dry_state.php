<?php

require_once("twitter_status.php");

class Dry_State extends Session_State
{
    /**
     * Posts a "Dry" status to Twitter when it initially transitions. 
     * @param Session $session 
     * @param int $moisture
     */
    public function Init($session, $moisture)
    {
        $twitterStatus = TwitterStatus::Load();
        $status = $twitterStatus->Send("dry");

        Logger::Debug("Moisture: ".$moisture." - ".$status);
    }

    /**
     * Changes the session to WATERED when moisture reach 75%.
     * @param Session $session 
     * @param int $moisture 
     */
    public function Update($session, $moisture)
    {
        if ($moisture >= 75)
        {
            $state = new Watered_State();
            $state->Init($session, $moisture);
            $session->ChangeState($state);
        }        
        else 
            Logger::Debug("Moisture: ".$moisture." - I'm thirsty...");
    }
}

?>