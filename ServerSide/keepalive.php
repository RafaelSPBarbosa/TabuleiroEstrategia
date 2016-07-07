<?php
//Stops the server from being deleted from the list. Gets pinged by the host every 10 seconds. Daemon checks to see if each uploaded host has been pinged recently.
//Should also let the host keep the information up to date.

//IP and unique_id are both used to authenticate the user when trying to keep a server open.
$ip = $_SERVER['REMOTE_ADDR'];

if(!isset($_GET['unique_id']))
{
	die();
}

if(!isset($ip))
{
	die();
}

$db = new mysqli("localhost", "autem", "rPL5yaatPGxSw69h", "autem");
if ($db->connect_errno) {
  die();
}
$sql = "UPDATE servers SET creation_date='" . time() . "' WHERE `unique_id`='" . $db->real_escape_string($_GET['unique_id']) . "' AND `ip`='" . $ip . "';";
if ($db->query($sql) === TRUE) {
    echo "1";
} else {
    echo "0";
}
$db->close();
?>