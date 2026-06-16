<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import { categorySchema, type CategorySchemaType } from '~/types/category.schema'

const router = useRouter()
const toast = useToast()
const isSaving = ref(false)

const schema = categorySchema

const state = reactive<CategorySchemaType>({
  title: '',
  slug: '',
  parent_id: '1', 
  description: ''
})

const { data: categoriesList } = await useFetch<any>('/api/blog/admin/categories/list-all', {
  key: 'admin-categories-shared-list',
  transform: (response: any) => {
    const list = response?.data || response || [];
    return list.map((cat: any) => ({
      label: cat.id_title,
      value: String(cat.id)
    }));
  }
})

async function onSubmit(event: FormSubmitEvent<CategorySchemaType>) {
  isSaving.value = true
  try {
    const response = await $fetch<any>("/api/blog/admin/categories", {
      method: "POST",
      body: event.data,
    })

    toast.add({
      title: "Успішно створено!",
      description: `Категорію "${response.item?.title || response.title}" успішно додано.`,
      color: "success",
      icon: "i-lucide-check-circle"
    })

    await navigateTo("/BlogCategories")
  } catch (error) {
    console.error(error)
    toast.add({
      title: "Помилка створення",
      description: "Перевірте правильність заповнення полів.",
      color: "error",
      icon: "i-lucide-x-circle"
    })
  } finally {
    isSaving.value = false
  }
}
</script>

<template>
  <div class="p-6 max-w-2xl mx-auto space-y-6">
    <div class="flex items-center gap-4">
      <UButton icon="i-lucide-arrow-left" variant="ghost" color="neutral" @click="navigateTo('/BlogCategories')" />
      <h1 class="text-2xl font-bold text-gray-900">Нова категорія</h1>
    </div>

    <UForm :schema="schema" :state="state" class="bg-white p-6 border border-gray-100 rounded-2xl shadow-sm space-y-4" @submit="onSubmit">
      <UFormField label="Назва" name="title" required>
        <UInput v-model="state.title" class="w-full" placeholder="Введіть назву..." />
      </UFormField>

      <UFormField label="Slug" name="slug">
        <UInput v-model="state.slug" class="w-full" placeholder="автогенерація або вручну..." />
      </UFormField>

      <UFormField label="Батьківська категорія" name="parent_id" required>
        <USelect 
          v-model="state.parent_id" 
          class="w-full"
          :items="categoriesList"
        />
      </UFormField>

      <UFormField label="Опис" name="description">
        <UTextarea v-model="state.description" class="w-full" :rows="4" placeholder="Короткий опис категорії..." />
      </UFormField>

      <div class="flex justify-end pt-2">
        <UButton type="submit" color="primary" :loading="isSaving">
          Зберегти категорію
        </UButton>
      </div>
    </UForm>
  </div>
</template>