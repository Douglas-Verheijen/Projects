<?php

require_once('twitter.php');
require_once("twitter_command.php");

class TwitterMention
{
    private static $twitter_mention_filename = 'twitter_mention_data';

    /** 
     * Gets the most recent mentions, finds any applicable hashtags, 
     * and perform whatever necessary command.
     * @param int $moisture
     */
    public function ReplyToMentions($moisture)
    {
        $latest_tweet_id = $this->tweet_id;

        $twitter = new TwitterService();
        $mentions = $twitter->GetMentions($latest_tweet_id);

        if (isset($mentions))
        {
            foreach ($mentions as $mention)
            {               
                $tweet_id = $mention->id;
                $sent_to = $mention->user->screen_name;

                if ($latest_tweet_id < $tweet_id)
                    $latest_tweet_id = $tweet_id;

                foreach ($mention->entities->hashtags as $hashtag)
                {
                    $action = $hashtag->text."_Command";
                    if (class_exists($action))
                    {
                        $data = new stdClass();
                        $data->sent_to = $sent_to;
                        $data->moisture = $moisture;

                        $command = new $action();
                        $command->Execute($data);
                    }
                }
            }
        }

        $this->tweet_id = $latest_tweet_id;
        $this->date = date("c");

        $data = serialize($this);
        file_put_contents(static::$twitter_mention_filename, $data);
    }

    /**
     * Loads the latest mention information. 
     * @return Returns loaded (or new if non-existant) instance of self.
     */
    public static function Load()
    {
        if (file_exists(static::$twitter_mention_filename))
        {
            $data = file_get_contents(static::$twitter_mention_filename);
            return unserialize($data);
        }

        $mention = new TwitterMention();
        $mention->tweet_id = 14927799;
        return $mention;
    }
}

?>