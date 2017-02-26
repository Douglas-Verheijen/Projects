<?php

foreach (glob("twitter_exchanges/*.php") as $filename)
    require_once($filename);

interface TwitterExchange
{    
    /**
     * Posts the given Status to Twitter.
     * @param string $last_tweet_id
     * @return Returns JSON response from Twitter for status update.
     */
    public function PostStatus($status);

    /**
     * Gets the recent Mentions from Twitter, up to the provided Id.
     * @param string $last_tweet_id
     * @return Returns JSON response of Twitter Mentions.
     */
    public function GetMentions($last_tweet_id);
}

?>