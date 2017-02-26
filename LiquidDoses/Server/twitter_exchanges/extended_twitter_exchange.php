<?php

require_once('base_twitter_exchange.php');

class ExtendedTwitterExchange extends TwitterAPIExchange implements TwitterExchange
{
    public function __construct()
    {
        $settings = $this->GetAuth();
        parent::__construct($settings);
    }

    /**
     * Posts the given Status to Twitter.
     * @param string $last_tweet_id
     * @return Returns JSON response from Twitter for status update.
     */
    public function PostStatus($status)
    {
        $url = 'https://api.twitter.com/1.1/statuses/update.json';
        $postfields = array('status' => $status);
        $requestMethod = 'POST';

        $response = parent::buildOauth($url, $requestMethod)
        	->setPostfields($postfields)
        	->performRequest();

        return json_decode($response);
    }    
    
    /**
     * Gets the recent Mentions from Twitter, up to the provided Id.
     * @param string $last_tweet_id
     * @return Returns JSON response of Twitter Mentions.
     */
    public function GetMentions($last_tweet_id)
    {        
        $url = 'https://api.twitter.com/1.1/statuses/mentions_timeline.json';
        $getfield = '?count=200&since_id='.$last_tweet_id;
        $requestMethod = 'GET';

        $response = parent::setGetfield($getfield)
            ->buildOauth($url, $requestMethod)
        	->performRequest();

        return json_decode($response);
    }

    private function GetAuth()
    {
        $config = (require '..\configuration.php');
        return $config['twitter_auth'];
    }
}

?>