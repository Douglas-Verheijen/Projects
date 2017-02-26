<?php

require_once('twitter.php');

class TwitterStatus
{
    private static $twitter_status_filename = 'twitter_status_data';
    private static $min_time = '8:00 AM';
    private static $max_time = '8:00 PM';

    public function __construct()
    {
        $this->statusIndex = -1;
        $this->Init();
    }

    /** 
     * Finds the status based on the provided index, posts the status on 
     * Twitter. 
     * @param int/string $index
     * @return Returns the sent status.
     */
    public function Send($index)
    {
        $statuses = static::GetStatuses();
        $status = $statuses[$index];

        $twitter = new TwitterService();
        $twitter->PostStatus($status);
            
        $this->Init();

        return $status;
    }

    /** 
     * Determines whether the new status needs to sent, finds the status, 
     * posts the status on Twitter.
     * @return Returns the sent status.
     */
    public function SendIfExpired()
    {
        if ($this->sendDate <= date('Y-m-d H:i:s'))
            return $this->Send($this->statusIndex);
    }

    /**
     * Loads the previously created status information. 
     * @return Returns loaded (or new if non-existant) instance of self.
     */
    public static function Load()
    {
        if (file_exists(static::$twitter_status_filename))
        {
            $data = file_get_contents(static::$twitter_status_filename);
            return unserialize($data);
        }

        return new TwitterStatus();
    }

    private function Init()
    {
        $tomorrow = date('Y-m-d', strtotime(date("c"). ' + 1 days'));
        $min = strtotime($tomorrow.' '.static::$min_time);
        $max = strtotime($tomorrow.' '.static::$max_time);

        $this->sendDate = date('Y-m-d H:i:s', rand($min, $max));

        $current = $this->statusIndex;
        while ($current == $this->statusIndex)
            $this->statusIndex = rand(0, 1);

        $data = serialize($this);
        file_put_contents(static::$twitter_status_filename, $data);
    }   

    private static function GetStatuses()
    {
        return array(
            "dry" => "I'm feeling a little dry. @aestreal @csidephoto #sensitiveplants #mimosas",
            "watered" => "I'm getting watered. Yay! @aestreal @csidephoto #sensitiveplants #mimosas",
            "How's everyone doing today? Hopefully good :) Yuppie! @aestreal @csidephoto #sensitiveplants #mimosas",
            "Today is a lovely day, isn't it? Hurrah! @aestreal @csidephoto #sensitiveplants #mimosas"
        );
    }
}

?>