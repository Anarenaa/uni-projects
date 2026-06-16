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
      <UFormField label="Категорія статті" name="category_id" class="w-full" required>
        <USelect 
          v-model="form.category_id" 
          :items="categories || []" 
          placeholder="Оберіть категорію..." 
          class="w-full bg-white mt-2"
          required
        />
      </UFormField>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <UFormField label="Заголовок статті" name="title" required>
        <UInput class="w-full" v-model="form.title" placeholder="Введіть заголовок..." />
      </UFormField>

      <UFormField label="Slug" name="slug">
        <UInput class="w-full" v-model="form.slug" placeholder="автогенерація або вручну..." />
      </UFormField>
    </div>

    <div v-if="form.is_published">
      <UFormField label="Дата публікації" name="published_at">
        <UInput class="w-full" v-model="form.published_at" placeholder="YYYY-MM-DD HH:MM:SS" />
      </UFormField>
    </div>

    <UFormField label="Уривок (Excerpt)" name="excerpt">
      <UTextarea class="w-full" v-model="form.excerpt" :rows="2" placeholder="Короткий опис для прев'ю..." />
    </UFormField>

    <UFormField label="Контент (Content Raw)" name="content_raw" required>
      <UTextarea class="w-full" v-model="form.content_raw" :rows="8" placeholder="Пишіть текст статті тут..." />
    </UFormField>

    <div class="flex items-center gap-2 py-2 border-t border-gray-50">
      <UFormField name="is_published">
        <UCheckbox id="is_published" v-model="form.is_published" label="Опубліковано" class="cursor-pointer select-none" />
      </UFormField>
    </div>

    <div class="flex justify-end gap-3 pt-4 border-t border-gray-100">
      <UButton type="button" variant="outline" color="neutral" @click="navigateTo('/BlogPosts')">Скасувати</UButton>
      <UButton type="submit" color="primary" :loading="isSaving">Зберегти зміни</UButton>
    </div>

  </div>
</template>