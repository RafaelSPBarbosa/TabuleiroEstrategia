<?php
//$ip = $HTTP_SERVER_VARS["REMOTE_ADDR"]; // Get players IP

$db = new mysqli("localhost", "autem", "rPL5yaatPGxSw69h", "autem");
if ($db->connect_errno) {
	//Tell client that the query failed.
  die('{"result": false}');
}

$sql = "SELECT `matchmaking_id` , `name`, `max_players`, `current_players`, `is_full`, `is_password`, `never_true`, `in_progress` , `ip` FROM `servers` WHERE never_true='0' ";

if(isset($_GET['name']))
{
	$sql .= "AND name='" . strtolower($db->real_escape_string($_GET['name'])) . "' ";
}
if(isset($_GET['max_players']))
{
	$sql .= "AND max_players='" . $db->real_escape_string($_GET['max_players']) . "' ";
}
if(isset($_GET['is_full']))
{
	$sql .= "AND is_full='" . $db->real_escape_string($_GET['is_full']) . "' ";
}
else
{
	$sql .= "AND is_full='0' ";
}

if(isset($_GET['is_password']))
{
	$sql .= "AND is_password='" . $db->real_escape_string($_GET['is_password']) . "' ";
}
else
{
	$sql .= "AND is_password='0' ";
}


if(isset($_GET['page_number']))
{
	$num = 10 * $_GET['page_number'];
	$sql .= "LIMIT $num , 10";
}
else
{
	//Limits to first page.
	$sql .= "LIMIT 0 , 10;";
}

$result = $db->query($sql);
$json = "";
$num = 0;
if ($result->num_rows > 0) {
	//Set up JSON to support success.
	$json = '{"result": true,"servers": [';
    while($row = $result->fetch_assoc()) {
       //Generate JSON list to return.
	   //$json .= '"' . $row['name'] . '":{"max_players":"' . $row['max_players'] . '", "current_players" : "' . $row['current_players'] . '", "is_full" : "' . $row['is_full'] . ;
	   $json .= json_encode($row) . ',';
    }
	//End Json.
	$json = rtrim($json, ",");
	$json .= ']}';
} else {
   //generate no server message.
     die('{"result": false}');
}
echo $json;
$db->close();
?>