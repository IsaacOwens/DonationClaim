<?php

include 'MySQLCredentials.php';
 
//read the post from PayPal system and add 'cmd' 
$req = 'cmd=_notify-validate'; 

foreach ($_POST as $key => $value) { 
    $value = urlencode(stripslashes($value)); 
    $req .= "&$key=$value"; 
} 

//post back to PayPal system to validate 
$header = "POST /cgi-bin/webscr HTTP/1.1\r\n"; 
$header .= "Content-Type: application/x-www-form-urlencoded\r\n"; 
$header .= "Host: www.paypal.com\r\n"; //Change this to www.sandbox.paypal.com\r\n for testing
$header .= "Connection: close\r\n"; 
$header .= "Content-Length: " . strlen($req) . "\r\n\r\n"; 
$fp = fsockopen ('ssl://www.paypal.com', 443, $errno, $errstr, 30);  //Change this to ssl://www.sandbox.paypal.com for testing
// 

//error connecting to paypal 
if (!$fp) { 
    // 
} 
     
//successful connection     
if ($fp) { 
    fputs ($fp, $header . $req); 
     
    while (!feof($fp)) { 
        $res = fgets ($fp, 1024); 
        $res = trim($res); //NEW & IMPORTANT 
                 
        if (strcmp($res, "VERIFIED") == 0) { 
            $transaction_id = $_POST['txn_id'];
            $payerid = $_POST['payer_id'];
            $firstname = $_POST['first_name'];
            $lastname = $_POST['last_name'];
            $payeremail = $_POST['payer_email'];
            $paymentdate = $_POST['payment_date'];
            $paymentstatus = $_POST['payment_status'];
            $mdate= date('Y-m-d h:i:s',strtotime($paymentdate));
            $otherstuff = json_encode($_POST);
			$item_name = $_POST['item_name'];
 
            $conn = new mysqli($dbhost,$dbusername,$dbpassword);
			if ($conn->connect_error) {
				trigger_error('Database connection failed: '  . $conn->connect_error, E_USER_ERROR);
			}
 
            // insert in our IPN record table
            $query = "INSERT INTO rustserver.ibn_table
            (itransaction_id,ipayerid,iname,iemail,itransaction_date, ipaymentstatus,ieverything_else,item_name)
            VALUES
            ('$transaction_id','$payerid','$firstname $lastname','$payeremail','$mdate','$paymentstatus','$otherstuff','$item_name')";
			$result = $conn->query($query);

			$conn->close();
        } 
     
        if (strcmp ($res, "INVALID") == 0) { 
            //insert into DB in a table for bad payments for you to process later 
        } 
    } 

    fclose($fp); 
} 



?>