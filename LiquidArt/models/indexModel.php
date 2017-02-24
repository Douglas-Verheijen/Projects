<?php

class IndexModel
{
	public function  __construct($row)
	{
		$this->Id = $row["Id"];
		$this->Name = $row["Name"];
		$this->Thumbnail = $row["Thumbnail"];
	}
}

?>