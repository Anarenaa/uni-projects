<?php

//1. Знайти добуток усіх чисел у масиві. Користувач вводить масив чисел. Знайти добуток усіх елементів. -->
$input = "5 2 3 abc 4";

$array = explode(" ", $input);

$result = 1;
$has_numbers = false;

for ($i = 0; $i < count($array); $i++) {
    if ($array[$i] != "") {
        if (is_numeric($array[$i])) {
            $result *= $array[$i];
            $has_numbers = true;
        }
    }
}
if (!$has_numbers) {
    $result = "Будь ласка, введіть коректні числа!";
}
echo $result;
echo "\n";

//2. Знайти всі досконалі числа в масиві. Створити масив із 15 чисел від 1 до 1000. Вивести всі досконалі числа (наприклад: 6, 28, 496).
$range_array = array();
for ($i = 0; $i < 15; $i++) {
    $range_array[] = rand(1, 1000);
}
$result_array = array();

foreach ($range_array as $number) {
    $sum = 0;
    for ($i = 1; $i <= $number / 2; $i++) {
        if ($number % $i == 0) {
            $sum += $i;
        }
    }
    if ($sum == $number && $number > 0) {
        array_push($result_array, $number);
    }
}
print_r($range_array);
if (empty($result_array)) {
    echo "Досконалих чисел в цьому масиві немає";
} else {
    echo "Досконалі числа: ";
    foreach ($result_array as $n) {
        echo $n . " ";
    }
}
echo "\n";

//3. Кількість нулів у масиві. Користувач вводить масив чисел. Порахувати, скільки з них дорівнюють нулю.
$input_array = "1 0 0 1 0";
$array = explode(" ", $input_array);
$result = 0;
for ($i = 0; $i < count($array); $i++) {
    if ($array[$i] != "" && $array[$i] === "0") {
        $result++;
    }
}
echo $input_array;
echo "\n";
echo "Кількість нулів в масиві: $result";
echo "\n";

//4. Сума квадратів непарних чисел у масиві. Згенерувати масив із 20 чисел від 1 до 50. Знайти суму квадратів лише непарних.
$range_array = array();
for ($i = 0; $i < 20; $i++) {
    $range_array[] = rand(1, 50);
}
$sum = 0;
for ($i = 0; $i < count($range_array); $i++) {
    if ($range_array[$i] % 2 == 1) {
        $sum += $range_array[$i] * $range_array[$i];
    }
}
print_r($range_array);
echo "Сума квадратів непарних чисел = $sum";
echo "\n";

//5. Обмін першого та останнього елементів масиву. Створити масив із 8 випадкових чисел. Поміняти місцями перший та останній елемент масиву.
$range_array = array();
for ($i = 0; $i < 8; $i++) {
    $range_array[] = rand();
}
print_r($range_array);
[$range_array[0], $range_array[count($range_array) - 1]] = [$range_array[count($range_array) - 1], $range_array[0]];
print_r($range_array);

//6. Знайти середнє арифметичне додатних чисел у масиві. Створити масив із 10 випадкових чисел від -50 до 50. Знайти середнє арифметичне додатних чисел.
$range_array = array();
for ($i = 0; $i < 10; $i++) {
    $range_array[] = rand(-50, 50);
}
$sum = 0;
$count_pos = 0;
for ($i = 0; $i < count($range_array); $i++) {
    if ($range_array[$i] > 0) {
        $sum += $range_array[$i];
        $count_pos++;
    }
}
print_r($range_array);
$average = $sum / $count_pos;
echo "Середнє арифметичне додатних = $average";
echo "\n";

//7. Перетворення ПІБ на email-формат. Користувач вводить рядок “Гарбузюк Олег”. Згенерувати email у форматі: harbuzyuk.oleh@example.com (усі літери маленькі).
$input = "Мікулич Анастасія Борисівна";
$translit = [
    'А' => 'A',
    'Б' => 'B',
    'В' => 'V',
    'Г' => 'H',
    'Ґ' => 'G',
    'Д' => 'D',
    'Е' => 'E',
    'Є' => 'Ye',
    'Ж' => 'Zh',
    'З' => 'Z',
    'И' => 'Y',
    'І' => 'I',
    'Ї' => 'Yi',
    'Й' => 'Y',
    'К' => 'K',
    'Л' => 'L',
    'М' => 'M',
    'Н' => 'N',
    'О' => 'O',
    'П' => 'P',
    'Р' => 'R',
    'С' => 'S',
    'Т' => 'T',
    'У' => 'U',
    'Ф' => 'F',
    'Х' => 'Kh',
    'Ц' => 'Ts',
    'Ч' => 'Ch',
    'Ш' => 'Sh',
    'Щ' => 'Shch',
    'Ю' => 'Yu',
    'Я' => 'Ia',
    'а' => 'a',
    'б' => 'b',
    'в' => 'v',
    'г' => 'h',
    'ґ' => 'g',
    'д' => 'd',
    'е' => 'e',
    'є' => 'ye',
    'ж' => 'zh',
    'з' => 'z',
    'и' => 'y',
    'і' => 'i',
    'ї' => 'yi',
    'й' => 'y',
    'к' => 'k',
    'л' => 'l',
    'м' => 'm',
    'н' => 'n',
    'о' => 'o',
    'п' => 'p',
    'р' => 'r',
    'с' => 's',
    'т' => 't',
    'у' => 'u',
    'ф' => 'f',
    'х' => 'kh',
    'ц' => 'ts',
    'ч' => 'ch',
    'ш' => 'sh',
    'щ' => 'shch',
    'ю' => 'yu',
    'я' => 'ia',
    'ь' => '',
    '\'' => ''
];
$name_parts = explode(" ", trim($input));
print_r($name_parts);
$lastname = $name_parts[0];
$firstname = $name_parts[1];

$lat_lastname = strtr($lastname, $translit);
$lat_firstname = strtr($firstname, $translit);

$email = strtolower($lat_lastname . "." . $lat_firstname) . "@example.com";
echo $email;
echo "\n";

//8. Перевірити чи рік — кратний 400. Користувач вводить рік. Перевірити, чи кратний він 400.
$input = "1956";
if (is_numeric($input) && $input > 0 && $input < 10000) {
    if ($input % 400 === 0) {
        echo "$input - кратний 400";
    } else {
        echo "$input - не кратний 400";
    }
} else {
    echo "Неправильний ввід";
}
echo "\n";

// 9. Добуток елементів з парними індексами та вивід непарних індексів. Створити массив заповнивши його випадковими числами від 0 до 100 (rand). Порахувати добуток елементів, які більше 0 та у яких парні індекси. Вивести результат на екран і вивести елементи, які більше нуля і у яких не парний індекс.
$range_array = array();
for ($i = 0; $i < 10; $i++) {
    $range_array[] = rand(0, 100);
}
print_r($range_array);
$dob = 1;
for ($i = 0; $i < count($range_array); $i++) {
    if ($range_array[$i] > 0) {
        if ($i % 2 === 0) {
            $dob *= $range_array[$i];
        } else {
            echo "$range_array[$i] ";
        }
    }
}
echo "\n$dob - добуток елементів, які більше 0 та у яких парні індекси";
echo "\n";

// 10. Перевірка на високосний рік. Перевірити чи високосний рік. Вам потрібно розробити програму, яка перевіряла б введене користувачем число (рік). Число може бути в діапазоні від 1 до 9999.
$input = "1956";
if (is_numeric($input) && $input > 0 && $input < 10000) {
    if (($input % 400 === 0) || ($input % 4 === 0 && $input % 100 !== 0)) {
        echo "$input — високосний рік.";
    } else {
        echo "$input — не високосний рік.";
    }
} else {
    echo "Неправильний ввід";
}
echo "\n";
