<script setup lang="ts">

const toast = useToast();
const route = useRoute();
const id = route.params.id;

const form = ref<any>({});
const isSaving = ref(false);

const { data: postData } = await useFetch<any>(`/api/blog/admin/posts/${id}`);

if (postData.value) {
  form.value = { ...postData.value };
  form.value.category_id = form.value.category_id ? String(form.value.category_id) : null;
  form.value.is_published = Boolean(Number(form.value.is_published));
}

const updatePost = async () => {
  isSaving.value = true;
  try {
    const updatedPost = await $fetch<any>(`/api/blog/admin/posts/${id}`, {
      method: "PUT",
      body: form.value,
    });

    toast.add({
      title: "Успішно оновлено!",
      description: `Статтю "${updatedPost.item.title}" (ID: ${updatedPost.item.id}) успішно збережено.`,
      color: "success",
      icon: "i-lucide-check-circle"
    });

    navigateTo(`/BlogPosts/${updatedPost.item.id}`);
  } catch (error) {
    toast.add({
      title: "Помилка збереження",
      description: "Не вдалося оновити дані на сервері.",
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
    <div class="flex items-center justify-between mb-6">
      <div class="flex items-center gap-4">
        <UButton icon="i-lucide-arrow-left" variant="ghost" color="neutral" @click="navigateTo('/BlogPosts')" />
        <h1 class="text-2xl font-bold text-gray-900">Стаття #{{ id }}</h1>
      </div>
    </div>

    <form v-if="form" @submit.prevent="updatePost">
      <PostForm v-model="form" :is-saving="isSaving" />
    </form>
  </div>
</template>