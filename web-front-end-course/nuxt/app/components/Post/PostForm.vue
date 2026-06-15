<script setup lang="ts">
// Двонаправлене зв'язування для об'єкта форми
const form = defineModel<any>({ required: true });

defineProps<{
  isSaving: boolean
}>();

const { data: categories } = await useFetch<any>('/api/blog/admin/categories/list-all', {
  key: 'admin-categories-shared-list',
  transform: (response: any) => {
    const list = response?.data || response || [];
    return list.map((cat: any) => ({
      label: cat.id_title,
      value: String(cat.id)
    }));
  }
});
</script>

<template>
  <div class="bg-white p-6 border border-gray-100 rounded-2xl shadow-sm space-y-6">
    <div class="bg-gray-50 p-4 rounded-xl border border-gray-100">
      <label class="block text-xs font-semibold uppercase tracking-wider text-gray-500 mb-2">Категорія статті</label>
      <USelect v-model="form.category_id" :items="categories || []" placeholder="Оберіть категорію..." value-key="value" class="w-full bg-white">
        <template #item-label="{ item }">
          {{ item.label }}
        </template>
      </USelect>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Заголовок статті*</label>
        <UInput class="w-full" v-model="form.title" required />
      </div>
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1">Slug</label>
        <UInput class="w-full" v-model="form.slug" />
      </div>
    </div>

    <div v-if="form.is_published">
      <label class="block text-sm font-medium text-gray-700 mb-1">Дата публікації</label>
      <UInput class="w-full" v-model="form.published_at" placeholder="YYYY-MM-DD HH:MM:SS" />
    </div>

    <div>
      <label class="block text-sm font-medium text-gray-700 mb-1">Уривок (Excerpt)*</label>
      <UTextarea class="w-full" v-model="form.excerpt" :rows="2" />
    </div>

    <div>
      <label class="block text-sm font-medium text-gray-700 mb-1">Контент (Content Raw)*</label>
      <UTextarea class="w-full" v-model="form.content_raw" :rows="8" required />
    </div>

    <div class="flex items-center gap-2 py-2 border-t border-gray-50">
      <UCheckbox id="is_published" v-model="form.is_published" label="Опубліковано" class="cursor-pointer select-none" />
    </div>

    <div class="flex justify-end gap-3 pt-4 border-t border-gray-100">
      <UButton type="button" variant="outline" color="neutral" @click="navigateTo('/BlogPosts')">Скасувати</UButton>
      <UButton type="submit" color="primary" :loading="isSaving">Зберегти зміни</UButton>
    </div>
  </div>
</template>