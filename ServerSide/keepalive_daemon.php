<?php
//Removes none active servers every 10 seconds.
echo "Starting Daemon" . PHP_EOL;
while(1)
{
	$db = new mysqli("localhost", "autem", "rPL5yaatPGxSw69h", "autem");
	$time = time() - 10;
	$sql = "DELETE FROM servers WHERE creation_date < '" . $time . "';";
	echo $sql . PHP_EOL;
	$db->query($sql);
	$db->close();
	sleep(10);
}
?>