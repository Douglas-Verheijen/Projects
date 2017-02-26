<?php

class Logger 
{
    private static $log_filename = "transaction_log";

    /**
     * Logs a DEBUG message in the transaction logs.
     * @param string $log_entry
     */
    public static function Debug($log_entry)
    {
        $log_entry = sprintf("[%s] DEBUG: %s\n", date("c"), $log_entry);
        static::Log($log_entry);
    }

    /**
     * Logs a ERROR message in the transaction logs.
     * @param string $log_entry
     */
    public static function Error($log_entry)
    {
        $log_entry = sprintf("[%s] ERROR: %s\n", date("c"), $log_entry);
        static::Log($log_entry);
    }   

    /**
     * Clears transaction logs.
     */
    public static function Clear()
    {
        unlink(static::$log_filename);
        static::Debug('The logs have been cleared.');
    }
    
    private static function Log($log_entry)
    {
        $file = fopen(static::$log_filename, 'a');
        fwrite($file, $log_entry);
        fclose($file);
        echo $log_entry;
    }
}

?>
