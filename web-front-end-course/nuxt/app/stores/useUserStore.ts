import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', () => {
  // State (Ref/Reactive)
  const name = ref('Гість')
  const isLoggedIn = ref(false)
  const loginTime = ref<string | null>(null)

  // Getters (Computed)
  const welcomeMessage = computed(() => `Вітаємо, ${name.value}!`)

  // Actions (Functions)
  function login(userName: string) {
    name.value = userName
    isLoggedIn.value = true
    loginTime.value = new Date().toLocaleTimeString()
  }

  function logout() {
    name.value = 'Гість'
    isLoggedIn.value = false
    loginTime.value = null
  }

  return { 
    name, 
    isLoggedIn, 
    loginTime, 
    welcomeMessage, 
    login, 
    logout 
  }
})

const profileData = ref()
const isLoading = ref(false)

async function fetchProfile() {
  isLoading.value = true
  try {
    // Приклад запиту через вбудований $fetch
    const response = await $fetch('https://api.github.com/users/octocat')
    profileData.value = response
  } finally {
    isLoading.value = false
  }
}