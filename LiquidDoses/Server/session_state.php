<?php

require_once('twitter_mention.php');
require_once('twitter_status.php');
foreach (glob("session_states/*.php") as $filename)
    require_once($filename);

abstract class Session_State
{
    public function __construct()
    {
        $this->date = date("c");
    }

    /**
     * Initiates the states when it is first created. Independent of 
     * the __construct which is called whenever the state is contructed/loaded.
     * @param Session $session
     * @param int $moisture
     */
    public abstract function Init($session, $moisture);

    /** 
     * Performs any necessary actions before the state is updated. 
     * @param Session $session
     * @param int $moisture
     */
    public function PreUpdate($session, $moisture)
    {        
        $twitterMention = TwitterMention::Load();
        $twitterMention->ReplyToMentions($moisture);
    }    

    /** 
     * Updates the current state. 
     * @param Session $session
     * @param int $moisture
     */
    public abstract function Update($session, $moisture);
    
    /**
     * Performs any necessary actions after the state is updated. 
     * @param Session $session
     * @param int $moisture
     */
    public function PostUpdate($session, $moisture)
    {
        $twitterStatus = TwitterStatus::Load();
        $twitterStatus->SendIfExpired();
    }
}

?>