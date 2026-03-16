const a = 1;
const b = 7;
console.log(a + b);
console.log(a - b);
console.log(a * b);
console.log(a / b);

//----------------------------

const firstName = "Anastasiia";
const lastName = "Mikulych";
const fullName = firstName + " " + lastName;
console.log(`Привіт, ${fullName}!`);

//----------------------------

let age = 16;
if(age >= 18){
    console.log("Доступ дозволено");
} else {
    console.log("Доступ заборонено");
}

//----------------------------

for(let i = 1; i<=10;i++){
    console.log(i);
}

//----------------------------

function square(number){
    return number * number;
}
console.log(square(5));

//----------------------------

const fruits = ['apple', 'banana', 'peach'];
fruits.push('pear');
fruits.forEach(fruit => {
    console.log(fruit);
});