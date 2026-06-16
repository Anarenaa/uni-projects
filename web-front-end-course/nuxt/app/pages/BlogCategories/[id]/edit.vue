<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import { categorySchema, type CategorySchemaType } from '~/types/category.schema'

const route = useRoute()
const id = route.params.id
const toast = useToast()
const isSaving = ref(false)

const schema = categorySchema

const state = reactive<CategorySchemaType>({
  title: '',
  slug: '',
  parent_id: '1',
  description: ''
})

const { data: categoryData } = await useFetch<any>(`/api/blog/admin/categories/${id}`)
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
const category = categoryData.value?.data;
if (category) {
  state.title = category.title || ''
  state.slug = category.slug || ''
  state.description = category.description || ''
  if (category.category_parent_id) {
    state.parent_id = String(category.category_parent_id)
  }
}

async function onSubmit(event: FormSubmitEvent<CategorySchemaType>) {
  isSaving.value = true;
  
  try {
    const payload = {
      ...event.data,
      parent_id: Number(event.data.parent_id) 
    }

    const response = await $fetch<any>(`/api/blog/admin/categories/${id}`, {
      method: "PUT",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json"
      },
      body: payload
    })

    toast.add({
      title: "Зміни збережено!",
      description: `Категорію "${response.item?.title || response.title}" успішно оновлено.`,
      color: "success",
      icon: "i-lucide-check-circle"
    })

    await navigateTo(`/BlogCategories/${id}`)
  } catch (error) {
    console.error(error)
    toast.add({
      title: "Помилка оновлення",
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
      <UButton icon="i-lucide-arrow-left" variant="ghost" color="neutral" @click="navigateTo(`/BlogCategories`)" />
      <h1 class="text-2xl font-bold text-gray-900">Редагування категорії #{{ id }}</h1>
    </div>

    <UForm :schema="schema" :state="state" class="bg-white p-6 border border-gray-100 rounded-2xl shadow-sm space-y-4" @submit="onSubmit">
      <UFormField label="Назва" name="title" required>
        <UInput v-model="state.title" class="w-full" />
      </UFormField>

      <UFormField label="Slug" name="slug">
        <UInput v-model="state.slug" class="w-full" />
      </UFormField>

      <UFormField label="Батьківська категорія" name="parent_id" required>
        <USelect 
          v-model="state.parent_id" 
          class="w-full"
          :items="categoriesList"
        />
      </UFormField>

      <UFormField label="Опис" name="description">
        <UTextarea v-model="state.description" class="w-full" :rows="4" />
      </UFormField>

      <div class="flex justify-end pt-2">
        <UButton type="submit" color="primary" :loading="isSaving">
          Зберегти зміни
        </UButton>
      </div>
    </UForm>
  </div>
</template>