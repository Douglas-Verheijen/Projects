<?php

require_once('session_state.php');
require_once('session_mode.php');
date_default_timezone_set('America/Edmonton');

class Session
{
    private static $session_filename = 'session_data';
    private static $instance;

    public function __construct()
    {
        $this->mode = Session_Mode::TEST;
        $this->state = new Watered_State();
    }

    /** 
     * Updates the current session. 
     * @param int $moisture
     */
    public function Update($moisture)
    {
        $this->state->PreUpdate($this, $moisture);
        $this->state->Update($this, $moisture);
        $this->state->PostUpdate($this, $moisture);
    }

    /** 
     * Modifies the state of the session. 
     * @param Session_State $state
     */
    public function ChangeState($state)
    {
        $this->state = $state;
        $this->Save();
    }

    /** 
     * Modifies the mode of the session. 
     * @param Session_Mode $mode
     */
    public function ChangeMode($mode)
    {
        $this->mode = $mode;
        $this->Save();
    }

    /** 
     * Saves the current session.
     */
    public function Save()
    {
        $data = serialize($this);
        file_put_contents(static::$session_filename, $data);
    }

    /** 
     * Loads the existing session.
     * @return Returns singleton instance of self.
     */
    public static function Load()
    {
        if (null === static::$instance) 
        {
            if (file_exists(static::$session_filename))
            {
                $data = file_get_contents(static::$session_filename);
                static::$instance = unserialize($data);
            }
            else
                static::$instance = new Session();
        }

        return static::$instance;
    }

    /** 
     * Creates and persists a new session.
     */
    public static function Reset()
    {
        $session = new Session();
        $session->Save();
    }
}

?>