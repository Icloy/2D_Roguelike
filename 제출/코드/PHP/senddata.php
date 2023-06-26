<?php
$hostname = 'localhost';
$username = 'root';
$password = '0000';
$database = 'savedatas';
$secretKey = "mySecretKey";
 
try 
{
    $dbh = new PDO('mysql:host='. $hostname .';dbname='. $database,$username, $password);
} 
catch(PDOException $e) 
{
    echo '<h1>An error has occurred.</h1><pre>', $e->getMessage(),'</pre>';
}
 
$hash = $_GET['hash'];
$name = $_GET['name'];
$date = $_GET['date'];
$coin = $_GET['coin'];
$curhp = $_GET['curhp'];
$maxhp = $_GET['maxhp'];
$dmg = $_GET['dmg'];

$realHash = hash('sha256', $name . $date . $coin . $curhp . $maxhp . $dmg . $secretKey);

$sth = $dbh->prepare('SELECT COUNT(*) FROM save WHERE name = :name');
$sth->bindParam(':name', $name, PDO::PARAM_STR);
$sth->execute();
$count = $sth->fetchColumn();

if ($count > 0) {
  $sth = $dbh->prepare('UPDATE save SET date = :date, coin = :coin, curhp = :curhp, maxhp = :maxhp, dmg = :dmg WHERE name = :name');
} else {
  $sth = $dbh->prepare('INSERT INTO save VALUES (:name,:date, :coin, :curhp, :maxhp, :dmg)');
}

try 
{
  $sth->bindParam(':name', $name, PDO::PARAM_STR);
  $sth->bindParam(':date', $date, PDO::PARAM_STR);
  $sth->bindParam(':coin', $coin, PDO::PARAM_INT);
  $sth->bindParam(':curhp', $curhp, PDO::PARAM_INT);
  $sth->bindParam(':maxhp', $maxhp, PDO::PARAM_INT);
  $sth->bindParam(':dmg', $dmg, PDO::PARAM_INT);
  $sth->execute();
}
catch(Exception $e) 
{
  echo '<h1>An error has occurred.</h1><pre>',$e->getMessage() ,'</pre>';
}
?>
