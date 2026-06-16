<script setup lang="ts">
import type { TableColumn, DropdownMenuItem } from "@nuxt/ui";
import type BlogCategory from "../types/category";

const props = defineProps<{
  categories: BlogCategory[];
  loading: boolean;
  totalResults: number;
  page: number;
  perPage: number;
}>();

const emit = defineEmits<{
  (e: "update:page", value: number): void;
  (e: "update:perPage", value: number): void;
  (e: "category-deleted"): void;
}>();

const currentPage = computed({
  get: () => props.page,
  set: (value) => emit("update:page", value),
});

const updatePageSize = (event: Event) => {
  const target = event.target as HTMLInputElement;
  const newSize = parseInt(target.value);
  if (!isNaN(newSize) && newSize > 0) {
    emit("update:perPage", newSize);
    emit("update:page", 1);
  }
};

const toast = useToast();
const deleteCategory = async (id: number) => {
  if (!confirm("Видалити цю категорію? Пов'язані пости можуть втратити категорію.")) return;
  
  try {
    const response = await $fetch<any>(`/api/blog/admin/categories/${id}`, { 
      method: "DELETE" 
    });
    
    if (response.success) {
      toast.add({
        title: `Категорію (ID: ${id}) видалено`,
        color: "success",
        icon: "i-lucide-check-circle",
      });
      
      emit("category-deleted");
    }
  } catch (error) {
    console.error("Не вдалося видалити категорію:", error);
    toast.add({
      title: "Критична помилка",
      description: "Сервер відхилив запит на видалення.",
      color: "error",
      icon: "i-lucide-x-circle",
    });
  }
};

const columns: TableColumn<any>[] = [
  { accessorKey: "id", header: "#" },
  { accessorKey: "title", header: "Назва категорії" },
  { accessorKey: "slug", header: "Slug" },
  { accessorKey: "category_parent_title", header: "Батьківська категорія" },
  { id: "actions", header: "" },
];

function getDropdownActions(category: BlogCategory): DropdownMenuItem[][] {
  return [
    [
      {
        label: "Редагувати",
        icon: "i-lucide-edit",
        onSelect: () => {
          navigateTo(`/BlogCategories/${category.id}/edit`);
        },
      },
    ],
    [
      {
        label: "Видалити",
        icon: "i-lucide-trash",
        color: "error",
        onSelect: () => {
          deleteCategory(category.id);
        },
      },
    ],
  ];
}
</script>

<template>
  <div class="border border-gray-100 rounded-lg">
    <div class="overflow-x-auto">
      <UTable
        :data="categories"
        :columns="columns"
        :loading="loading"
        class="w-full text-sm md:text-base"
        :ui="{
          th: 'px-3 py-2 font-normal text-gray-500 px-4 py-2',
          td: 'border-b border-gray-100 px-4 py-2',
          tr: 'hover:bg-gray-50',
          thead: 'bg-gray-100 font-normal',
        }"
      >
        <template #title-cell="{ row }">
          <NuxtLink
            :to="`/BlogCategories/${row.original.id}`"
            class="text-blue-600 hover:underline font-medium"
          >
            {{ row.original.title }}
          </NuxtLink>
        </template>

        <template #slug-cell="{ row }">
          <span :class="row.original.slug ? 'text-gray-700' : 'text-gray-400 italic'">
            {{ row.original.slug || "Немає" }}
          </span>
        </template>

        <template #category_parent_title-cell="{ row }">
          <span :class="row.original.category_parent_title ? 'text-gray-700' : 'text-gray-400 italic'">
            {{ row.original.category_parent_title || "Головна категорія" }}
          </span>
        </template>

        <template #actions-cell="{ row }">
          <div class="text-right pr-4">
            <UDropdownMenu :items="getDropdownActions(row.original)">
              <UButton
                icon="i-lucide-ellipsis-vertical"
                color="neutral"
                variant="ghost"
                aria-label="Дії"
                class="cursor-pointer"
              />
            </UDropdownMenu>
          </div>
        </template>
      </UTable>
    </div>

    <div class="flex justify-between items-center text-gray-400 p-4 text-sm border-t border-gray-100">
      <div>
        Показувати по
        <input
          :value="perPage"
          @change="updatePageSize"
          class="w-10 mx-1 py-1 text-center text-black border border-gray-300 rounded-sm [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none"
          type="number"
        />
        із <span class="font-bold text-black">{{ totalResults }}</span> записів
      </div>

      <UPagination
        v-model:page="currentPage"
        :items-per-page="perPage"
        :total="totalResults"
      />
    </div>
  </div>
</template>