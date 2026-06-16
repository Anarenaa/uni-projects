<script setup lang="ts">

const route = useRoute();
const id = route.params.id;

const { data: post } = await useFetch<any>(`/api/blog/admin/posts/${id}`);

</script>

<template>
  <div class="p-6 max-w-3xl mx-auto">
    <div class="flex items-center justify-between mb-6">
      <div class="flex items-center gap-4">
        <UButton icon="i-lucide-arrow-left" variant="ghost" color="neutral" @click="navigateTo('/BlogPosts')" />
        <h1 class="text-2xl font-bold text-gray-900">Перегляд статті #{{ id }}</h1>
      </div>
      <div>
        <UButton 
          icon="i-lucide-edit" 
          color="primary" 
          variant="subtle" 
          @click="navigateTo(`/BlogPosts/${id}/edit`)"
        >
          Редагувати
        </UButton>
      </div>
    </div>

    <div v-if="post" class="bg-white p-8 border border-gray-100 rounded-2xl shadow-sm space-y-6">
      
      <div class="flex flex-wrap items-center gap-3 text-xs font-semibold uppercase tracking-wider text-gray-400">
        <span>Категорія: <span class="text-gray-700">{{ post.category?.title || 'Без категорії' }}</span></span>
        <span>•</span>
        <span>Статус: 
          <span :class="post.is_published ? 'text-green-600' : 'text-amber-600'">
            {{ post.is_published ? 'Опубліковано' : 'Чернетка' }}
          </span>
        </span>
        <span v-if="post.user">•</span>
        <span v-if="post.user">Автор: <span class="text-gray-700">{{ post.user.name }}</span></span>
      </div>

      <h1 class="text-3xl font-extrabold text-gray-900 leading-tight">{{ post.title }}</h1>
      
      <div class="inline-block px-2.5 py-1 bg-gray-50 border border-gray-200 text-xs font-mono text-gray-600 rounded-md">
        slug: {{ post.slug }}
      </div>

      <hr class="border-gray-100" />

      <div v-if="post.excerpt" class="p-4 bg-gray-50 rounded-xl border border-gray-100 italic text-gray-600 leading-relaxed">
        {{ post.excerpt }}
      </div>

      <div class="text-gray-800 leading-relaxed whitespace-pre-line text-base">
        {{ post.content_raw || post.content_html }}
      </div>
      
      <div class="text-xs text-gray-400 pt-4 flex flex-col gap-1 border-t border-gray-50 font-mono">
        <span v-if="post.published_at">Дата публікації: {{ new Date(post.published_at).toLocaleString() }}</span>
        <span>Оновлення: {{ new Date(post.updated_at).toLocaleString() }}</span>
        <span>Створено в базі: {{ new Date(post.created_at).toLocaleString() }}</span>
      </div>

    </div>
  </div>
</template>