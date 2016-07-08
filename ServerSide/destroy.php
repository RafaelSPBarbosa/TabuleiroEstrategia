<?php
//Deletes a specific server from the list.
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
$sql = "DELETE FROM servers WHERE `unique_id`='" . $db->real_escape_string($_GET['unique_id']) . "' AND `ip`='" . $ip . "';";
echo $sql;
$db->query($sql);
$db->close();
?>