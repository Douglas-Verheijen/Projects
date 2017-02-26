<?php

require_once('session.php');
require_once('session_mode.php');
require_once('twitter_exchange.php');

class TwitterService 
{
    public function __construct()
    {
        $session = Session::Load();
        switch ($session->mode)
        {
            case Session_Mode::PROD:
                $this->exchange = new ExtendedTwitterExchange();
                break;

            case Session_Mode::TEST:
            default:
                $this->exchange = new DisabledTwitterExchange();
                break;
        }
    }

    /** 
     * Posts the given Status to Twitter. 
     * @param string $status
     * @return Returns JSON response from Twitter for status update.
     */
    public function PostStatus($status)
    {
        return $this->exchange->PostStatus($status);
    }

    /** 
     * Gets the recent Mentions from Twitter, up to the provided Id. 
     * @param int $last_tweet_id
     * @return Returns JSON response of Twitter Mentions.
     */
    public function GetMentions($last_tweet_id)
    {
        return $this->exchange->GetMentions($last_tweet_id);
    }
}

?>