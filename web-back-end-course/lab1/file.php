<?php
$number = "12345";
$chunk = chunk_split($number, 1, ".");
$sum = 0;
foreach(explode(".", $chunk) as $n)
    $sum += (int)$n;
echo $sum;

$search_num = 2;
$count = substr_count($number, $search_num);
echo "\n";
echo $count;

$array_num = range(1,5);
$sum1 = 0;
$denominator = 5;
foreach($array_num as $n)
    if($n % $denominator === 0)
        $sum1 += $n;
echo "\n";
echo $sum1;

$array = array_fill(0, 9, null);
$array = array_map(function() {
    return rand(0, 6);
}, $array);
$array = array_unique($array);
print_r($array);
$min_i = array_search(min($array), $array);
$max_i = array_search(max($array), $array);
echo $min_i;
echo $max_i;
echo "\n";
$temp = $array[$min_i];
$array[$min_i] = $array[$max_i];
$array[$max_i] = $temp;
print_r($array);