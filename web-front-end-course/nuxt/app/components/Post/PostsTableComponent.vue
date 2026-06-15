<script setup lang="ts">
import type { TableColumn, DropdownMenuItem } from "@nuxt/ui";
import type Post from "../../types/post";

const props = defineProps<{
  posts: Post[];
  loading: boolean;
  totalResults: number;
  page: number;
  perPage: number;
}>();

const emit = defineEmits<{
  (e: "update:page", value: number): void;
  (e: "update:perPage", value: number): void;
  (e: "post-deleted"): void;
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
const deletePost = async (id: number) => {
  if (!confirm("Видалити цей пост?")) return;
  
  try {
    const response = await $fetch<any>(`/api/blog/admin/posts/${id}`, { 
      method: "DELETE" 
    });
    
    if (response.success) {
      toast.add({
        title: `Запис (ID: ${id}) видалено`,
        color: "success",
        icon: "i-lucide-check-circle",
      });
      
      emit("post-deleted");
    }
  } catch (error) {
    console.error("Не вдалося видалити пост:", error);
    toast.add({
      title: "Критична помилка",
      description: "Сервер відхилив запит на видалення.",
      color: "error",
      icon: "i-lucide-x-circle",
    });
  }
};

const columns: TableColumn<Post>[] = [
  { accessorKey: "id", header: "#" },
  { accessorKey: "author_name", header: "Автор" },
  { accessorKey: "category_title", header: "Категорія" },
  { accessorKey: "title", header: "Заголовок" },
  { accessorKey: "date_published", header: "Дата публікації" },
  { id: "actions", header: "" },
];

function getDropdownActions(post: Post): DropdownMenuItem[][] {
  return [
    [
      {
        label: "Редагувати",
        icon: "i-lucide-edit",
        onSelect: () => {
          navigateTo(`/BlogPosts/${post.id}/edit`);
        },
      },
    ],
    [
      {
        label: "Видалити",
        icon: "i-lucide-trash",
        color: "error",
        onSelect: () => {
          deletePost(post.id)
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
        :data="posts"
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
        <template #user-cell="{ row }">
          <span class="font-medium text-gray-700">
            {{ row.original.author_name || "Невідомий автор" }}
          </span>
        </template>

        <template #category-cell="{ row }">
          <span class="text-gray-600">
            {{ row.original.category_title || "Без категорії" }}
          </span>
        </template>

        <template #title-cell="{ row }">
          <NuxtLink
            :to="`/BlogPosts/${row.original.id}`"
            class="text-blue-600 hover:underline font-medium"
          >
            {{ row.original.title }}
          </NuxtLink>
        </template>

        <template #date_published_at-cell="{ row }">
          <span class="text-sm text-gray-500">
            {{
              row.original.date_published
            }}
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

    <div
      class="flex justify-between items-center text-gray-400 p-4 text-sm border-t border-gray-100"
    >
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
