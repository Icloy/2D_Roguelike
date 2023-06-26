<?php
$hostname = 'localhost';
$username = 'root';
$password = '0000';
$database = 'savedatas';

$name = isset($_POST["name"]) ? $_POST["name"] : null;

try {
    $dbh = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);
} catch(PDOException $e) {
    echo '<h1>An error has occurred.</h1><pre>', $e->getMessage(),'</pre>';
}
 
$sth = $dbh->prepare('SELECT * FROM save WHERE name=:name');
$sth->bindParam(':name', $name, PDO::PARAM_STR);
$sth->execute();

$result = $sth->fetchAll(PDO::FETCH_ASSOC);
 
if (count($result) > 0) {
    echo json_encode($result);
} else {
    echo "null";
}
?>