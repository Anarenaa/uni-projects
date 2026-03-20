/*
Завдання 1

Пріоритет завантаження скриптів У вас є два підключених файли. Якщо small.js завантажується швидше за big.js, який з них виконається першим і чому?

<script src="big.js"></script>
<script src="small.js"></script>

Відповідь: першим завантажиться big.js файл, бо код читається зверху вниз
*/

/*
Завдання 2

Дайте відповідь, чому дорівнюватиме х у цьому прикладі: var a = 2; var x = 1 + (a *= 2);
Відповідь: x = 5

Чи з'явиться alert у наступному коді та поясніть чому?
if ("0") { alert('Привіт'); }
Відповідь: так, тому що 0 це string, що не є false. Було би false, якщо рядок був би порожнім ""
*/

let admin; 
let someName = "Василь";
admin = someName; //робить копію
console.log(admin);

//Завдання 3
let countiesData = [];

async function fetchData() {
    const response = await fetch("https://api.census.gov/data/2020/acs/acs5/profile?get=NAME&for=county:*")
    const data = await response.json();

    //беремо всі об'єкти, окрім першого - slice(1)
    countiesData = data.slice(1).map(item => ({
        name: item[0],
        state: item[1],
        county: item[2],
        fullCode: item[1] + item[2]
    }));
}

function getCountyCode(countyName) {
    const foundCounty = countiesData.find(c => c.name.toLowerCase() === countyName.toLowerCase().trim());
    return foundCounty ? foundCounty.fullCode : "Округ не знайдено";
}

async function searchCounty(){
    const countyInput = document.querySelector("#countyInput");
    const county = countyInput.value;
    if(county === ""){
        alert("Поле не може бути порожнім");
        return;
    }
    await fetchData();

    const code = getCountyCode(county);
    const resultEl = document.querySelector("#result");

    resultEl.innerHTML = "Код: " + code;
}

// Завдання 4

document.getElementById("submit-button").addEventListener('click', (event)=>{
    event.preventDefault();
    const firstName = document.getElementById("first-name").value.trim();
    const lastName = document.getElementById("last-name").value.trim();
    const email = document.getElementById("email").value.trim();

    event.preventDefault();

    if (!firstName || !lastName || !email) {
        alert("Заповніть порожні поля");
        return;
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(email)) {
        alert("Введіть коректну електронну пошту");
        return;
    }
    
    alert(`Вітаємо ${firstName} ${lastName}! Ваша електронна пошта: ${email}`)
})

// Завдання 5

const leftButton = document.querySelector("#left-one");
const rightButton = document.querySelector("#right-one");
const elidEl = document.getElementById("elid");

let pixels = 0;
function leftSlide(){
    pixels -= 100;
    elidEl.style.left = `${pixels}px`;
}
function rightSlide(){
    pixels += 100;
    elidEl.style.left = `${pixels}px`;
}