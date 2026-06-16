<script setup lang="ts">
import type BlogCategory from "../../types/category";

useHead({
  title: "Управління категоріями",
});

type LaravelPaginationResponse = {
  data: BlogCategory[];
  links: {
    first: string;
    last: string;
    prev: string | null;
    next: string | null;
  };
  meta: {
    current_page: number;
    from: number;
    last_page: number;
    per_page: number;
    to: number;
    total: number;
  };
};

const page = ref(1);
const perPage = ref(5);
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
  "/api/blog/admin/categories",
  {
    key: "server-table-categories",
    query: {
      page: page,
      per_page: perPage,
      search: debouncedSearch,
    },
    watch: [page, perPage, debouncedSearch],
  },
);

const categories = computed(() => data.value?.data || []);
const totalResults = computed(() => data.value?.meta?.total || 0);
</script>

<template>
  <div class="p-4">
    <div class="flex gap-4 my-4">
      <UInput
        v-model="titleFilter"
        class="max-w-sm"
        placeholder="Пошук за назвою..."
      />
    </div>

    <NuxtLink
      to="/BlogCategories/create"
      class="block font-bold text-xl text-center bg-gray-100 py-4 my-1 border border-gray-300 hover:bg-gray-50 rounded-2xl"
    >
      Додати категорію
    </NuxtLink>

    <CategoryTableComponent
      v-model:page="page"
      v-model:perPage="perPage"
      :categories="categories"
      :loading="status === 'pending'"
      :totalResults="totalResults"
      @category-deleted="refresh"
    />
  </div>
</template>