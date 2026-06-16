<script setup lang="ts">
const route = useRoute();
const id = route.params.id;

const { data: category } = await useFetch<any>(`/api/blog/admin/categories/${id}`);
</script>

<template>
  <div class="p-6 max-w-2xl mx-auto">
    <div class="flex items-center justify-between mb-6">
      <div class="flex items-center gap-4">
        <UButton 
          icon="i-lucide-arrow-left" 
          variant="ghost" 
          color="neutral" 
          @click="navigateTo('/BlogCategories')" 
        />
        <h1 class="text-2xl font-bold text-gray-900">Категорія #{{ id }}</h1>
      </div>
      
      <div>
        <UButton 
          icon="i-lucide-edit" 
          color="primary" 
          variant="subtle" 
          @click="navigateTo(`/BlogCategories/${id}/edit`)"
        >
          Редагувати
        </UButton>
      </div>
    </div>

    <div v-if="category" class="bg-white p-6 border border-gray-100 rounded-2xl shadow-sm space-y-4">
      <div>
        <h3 class="text-xs font-semibold text-gray-400 uppercase tracking-wider">Назва</h3>
        <p class="text-xl font-bold text-gray-900">{{ category.data.title }}</p>
      </div>

      <div>
        <h3 class="text-xs font-semibold text-gray-400 uppercase tracking-wider">Slug (ЧПУ)</h3>
        <p class="font-mono text-sm text-gray-600 bg-gray-50 px-2 py-1 inline-block rounded">
          {{ category.data.slug }}
        </p>
      </div>

      <div>
        <h3 class="text-xs font-semibold text-gray-400 uppercase tracking-wider">Батьківська categoria</h3>
        <p :class="category.data.category_parent_title ? 'text-gray-700 font-medium' : 'text-gray-400 italic'">
          {{ category.data.category_parent_title || "Головна (коренева) категорія" }}
        </p>
      </div>

      <div>
        <h3 class="text-xs font-semibold text-gray-400 uppercase tracking-wider">Опис</h3>
        <p :class="category.data.description ? 'text-gray-600' : 'text-gray-400 italic'" class="leading-relaxed whitespace-pre-line">
          {{ category.data.description || "Опис відсутній" }}
        </p>
      </div>
    </div>
  </div>
</template>