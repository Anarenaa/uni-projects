<script setup lang="ts">
const isSaving = ref(false);

const form = ref<any>({
  title: "",
  slug: "",
  category_id: null,
  excerpt: "",
  content_raw: "",
  is_published: false,
  published_at: "", 
});

const toast = useToast();
const createPost = async () => {
  isSaving.value = true;
  try {
    const newPost = await $fetch<any>('/api/blog/admin/posts', {
      method: "POST",
      body: form.value,
    });

    toast.add({
      title: "Статтю створено!",
      description: `Новий запис "${newPost.item.title || 'Без назви'}" успішно додано в базу даних.`,
      color: "success",
      icon: "i-lucide-check-circle"
    });

    await navigateTo(`/BlogPosts/${newPost.item.id}`);
  } catch (error) {
    
    toast.add({
      title: "Помилка створення",
      description: "Не вдалося зберегти нову статтю. Перевірте введені дані.",
      color: "error",
      icon: "i-lucide-x-circle"
    });
  } finally {
    isSaving.value = false;
  }
};
</script>

<template>
  <div class="p-6 max-w-3xl mx-auto">
    <div class="flex items-center gap-4 mb-6">
      <UButton icon="i-lucide-arrow-left" variant="ghost" color="neutral" @click="navigateTo('/BlogPosts')" />
      <h1 class="text-2xl font-bold text-gray-900">Створення статті</h1>
    </div>

    <form @submit.prevent="createPost">
      <PostForm v-model="form" :is-saving="isSaving" />
    </form>
  </div>
</template>