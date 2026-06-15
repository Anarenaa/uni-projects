<script setup lang="ts">
import type Post from "../../types/post";

useHead({
  title: "Управління статтями",
});

type LaravelPaginationResponse = {
  current_page: number;
  data: Post[];
  total: number;
  per_page: number;
};

const page = ref(1);
const perPage = ref(25);
const titleFilter = ref("");
const debouncedSearch = ref("");

watchDebounced(
  titleFilter,
  (newValue) => {
    debouncedSearch.value = newValue;
    page.value = 1;
  },
  { debounce: 500, maxWait: 1000 },
);

const { data, status, refresh } = useFetch<LaravelPaginationResponse>(
  "/api/blog/admin/posts",
  {
    key: "server-table-posts",
    query: {
      page: page,
      per_page: perPage,
      search: debouncedSearch,
    },
    watch: [page, perPage, debouncedSearch],
  },
);

const posts = computed(() => data.value?.data || []);
const totalResults = computed(() => data.value?.total || 0);
</script>

<template>
  <div class="p-4">
    <div class="flex gap-4 my-4">
      <UInput
        v-model="titleFilter"
        class="max-w-sm"
        placeholder="Пошук за заголовком..."
      />
    </div>

    <a
      href="/BlogPosts/create"
      class="block font-bold text-xl text-center bg-gray-100 py-4 my-1 border border-gray-300 hover:bg-gray-50 rounded-2xl"
    >
      Додати
    </a>

    <PostPostsTableComponent
      v-model:page="page"
      v-model:perPage="perPage"
      :posts="posts"
      :loading="status === 'pending'"
      :totalResults="totalResults"
      @post-deleted="refresh"
    />
  </div>
</template>
