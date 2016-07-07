<?php
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);

$db = new mysqli("localhost", "autem", "rPL5yaatPGxSw69h", "autem");
if ($db->connect_errno) {
	//Tell client that the query failed.
  die('{"result": false}');
}

$ip = $_SERVER['REMOTE_ADDR'];
if(!isset($ip))
{
	die("error");
}

$sql = "INSERT INTO servers (creation_date,ip,name,max_players,is_password,password,unique_id) VALUES('" . time() . "','" . $ip . "',";

if(isset($_GET['name']))
{
	$sql .= "'" . $db->real_escape_string($_GET['name']) . "',";
}
else
{
	$sql .= "'Autem Server',";
}
if(isset($_GET['max_players']))
{
	$sql .= "'" . $db->real_escape_string($_GET['max_players']) . "',";
}
else
{
	$sql .= "'4',";
}

if(isset($_GET['password']))
{
	$sql .= "'1','" . $db->real_escape_string($_GET['password']) . "',";
}
else
{
	$sql .= "'0','',";
}

$unique = md5($sql . time());
$sql .= "'" . $unique . "');";

//Insert into the list.
$db->query($sql);

//Return unique ID
echo $unique;
$db->close();
?>