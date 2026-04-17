<script setup>
// Отримуємо екземпляр стору
const userStore = useUserStore();

// Якщо нам потрібна деструктуризація зі збереженням реактивності:
const { name, isLoggedIn, welcomeMessage } = storeToRefs(userStore);

const inputName = ref("");

const handleLogin = () => {
  if (inputName.value) {
    userStore.login(inputName.value);
    inputName.value = "";
  }
};
</script>

<template>
  <div class="flex justify-center items-center flex-col gap-4 m-10">
    <h1>{{ welcomeMessage }}</h1>

    <div v-if="!isLoggedIn">
      <input
        v-model="inputName"
        placeholder="Введіть ваше ім'я"
        class="mb-4 p-2 border border-gray-400 rounded-lg"
      />
      <button
        @click="handleLogin"
        class="cursor-pointer block bg-green-500 mx-auto py-4 px-6"
      >
        Увійти
      </button>
    </div>

    <div v-else>
      <p>Ви зайшли о: {{ userStore.loginTime }}</p>
      <button
        @click="userStore.logout"
        class="cursor-pointer block bg-red-500 mx-auto my-4 py-4 px-6"
      >
        Вийти
      </button>
    </div>
  </div>
</template>
