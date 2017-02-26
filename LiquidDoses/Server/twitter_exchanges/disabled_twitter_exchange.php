<?php

class DisabledTwitterExchange implements TwitterExchange
{    
    /**
     * Performs no action when the exchange is disabled.
     * @param string $status
     * @return Returns nothing.
     */
    public function PostStatus($status)
    {
        // Do nothing.
        return null;
    }

    /**
     * Performs no action when the exchange is disabled.
     * @param string $last_tweet_id
     * @return Returns nothing.
     */
    public function GetMentions($last_tweet_id)
    {
        // Do nothing.
        return null;
    }
}

?>