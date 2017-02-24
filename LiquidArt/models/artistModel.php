<?php

class ArtistModel
{
	public function  __construct($row)
	{
		$this->Name = $row["Name"];
		$this->Thumbnail = $row["Thumbnail"];
		$this->Biography = $row["Biography"];
		$this->Site = $row["Site"];
	}
}

class ArtModel
{
	public function __construct($row)
	{
		$this->Name = $row["Name"];
		$this->Date = $row["Date"];
		$this->Price = $row["Price"];
		$this->Medium = $row["Medium"];
		$this->Status = $row["Status"];
		$this->Height = $row["Height"];
		$this->Width = $row["Width"];
		$this->Thumbnail = $row["Thumbnail"];
		
		$this->Size = $this->Width * $this->Height;
	}
}

class VenueModel
{
	public function __construct($row)
	{
		$this->Name = $row["Name"];
		$this->Address = $row["Address"];
		$this->City = $row["City"];
		$this->Province = $row["Province"];
		$this->Country = $row["Country"];
		$this->PostalCode = $row["PostalCode"];
		$this->ContactName = $row["ContactName"];
		$this->ContactPhoneNumber = $row["ContactPhoneNumber"];
		$this->ContactEmail = $row["ContactEmail"];
		$this->Thumbnail = $row["Thumbnail"]; 
	}
}

?>