<?php
//Removes none active servers every 10 seconds.
echo "Starting Daemon" . PHP_EOL;
while(1)
{
	$db = new mysqli("localhost", "autem", "rPL5yaatPGxSw69h", "autem");
	$time = time() - 15;
	$sql = "DELETE FROM servers WHERE creation_date < '" . $time . "';";
	$db->query($sql);
	$db->close();
	sleep(10);
}
?>