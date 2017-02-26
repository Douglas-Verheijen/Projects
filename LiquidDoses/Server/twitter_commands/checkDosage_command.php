<?php

require_once('twitter.php');

class CheckDosage_Command implements TwitterCommand
{
    /** 
     * Response to the "Sent" user with the current moisture level. 
     * @param stdClass $data
     */
    public function Execute($data)
    {
        $sent_to = $data->sent_to;
        $moisture = $data->moisture;

        $status = "@".$sent_to." I'm currently ".$moisture."%. Thanks for asking! :) (".date("H:i:s").")";

        $twitter = new TwitterService();
        $twitter->PostStatus($status);
    }
}

?>