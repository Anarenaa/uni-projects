<script setup lang="ts">
import { postSchema, type PostSchemaType } from '~/types/post.schema'
import type { FormSubmitEvent } from '@nuxt/ui'

const isSaving = ref(false);
const schema = postSchema;

const state = reactive<PostSchemaType>({
  title: "",
  slug: "",
  category_id: "1",
  content_raw: "",
});

const toast = useToast();

async function onSubmit(event: FormSubmitEvent<PostSchemaType>) {
  isSaving.value = true;
  try {
    const newPost = await $fetch<any>('/api/blog/admin/posts', {
      method: "POST",
      body: event.data,
    });

    toast.add({
      title: "Статтю створено!",
      description: `Новий запис "${newPost.item?.title || 'Без назви'}" успішно додано.`,
      color: "success",
      icon: "i-lucide-check-circle"
    });

    await navigateTo(`/BlogPosts/${newPost.item.id}`);
  } catch (error) {
    console.error(error);
    toast.add({
      title: "Помилка створення",
      description: "Не вдалося зберегти нову статтю. Перевірте введені дані.",
      color: "error",
      icon: "i-lucide-x-circle"
    });
  } finally {
    isSaving.value = false;
  }
}
</script>

<template>
  <div class="p-6 max-w-3xl mx-auto">
    <div class="flex items-center gap-4 mb-6">
      <UButton icon="i-lucide-arrow-left" variant="ghost" color="neutral" @click="navigateTo('/BlogPosts')" />
      <h1 class="text-2xl font-bold text-gray-900">Створення статті</h1>
    </div>

    <UForm :schema="schema" :state="state" @submit="onSubmit">
      <PostForm v-model="state" :is-saving="isSaving" />
    </UForm>
  </div>
</template>