<script setup lang="ts">
import { getPaginationRowModel } from '@tanstack/vue-table'
import { h, resolveComponent } from 'vue'
import type { TableColumn } from '@nuxt/ui'

useHead({
  title: 'Table'
})

const UButton = resolveComponent('UButton')

type Product = {
  id: number
  title: string
  description: string
  category: string
  price: number
  rating: number
  brand: string
  thumbnail: string
}

const { data, status } = useLazyFetch<{ products: Product[] }>('https://dummyjson.com/products', {
  key: 'table-products',
  server: false
})

const products = computed(() => data.value?.products || [])

const pagination = ref({
  pageIndex: 0,
  pageSize: 3
})

const table = useTemplateRef('table')

const globalFilter = ref('')

const selectedCount = computed(() => {
  const tableApi = table.value?.tableApi
  if (!tableApi) return 0
  
  return tableApi.getFilteredRowModel().rows.filter(row => row.getIsSelected()).length
})

const updatePageSize = (event: Event) => {
  const target = event.target as HTMLInputElement
  const newSize = parseInt(target.value)
  if (!isNaN(newSize) && newSize > 0) {
    pagination.value.pageSize = newSize
    pagination.value.pageIndex = 0
  }
}

const createSortableHeader = (label: string) => ({ column }) => {
  const isSorted = column.getIsSorted()

  return h(UButton, {
    color: 'neutral',
    variant: 'ghost',
    label,
    icon: isSorted
      ? isSorted === 'asc'
        ? 'i-lucide-arrow-up-narrow-wide'
        : 'i-lucide-arrow-down-wide-narrow'
      : 'i-lucide-arrow-up-down',
    class: '-mx-2.5',
    onClick: () => column.toggleSorting(column.getIsSorted() === 'asc')
  })
}

const columns: TableColumn<Product>[] = [
  {
    id: 'select',
    header: ({ table }) => h('input', { 
      type: 'checkbox', 
      class: 'w-4 h-4 cursor-pointer',
      checked: table.getIsAllPageRowsSelected(),
      indeterminate: table.getIsSomePageRowsSelected(),
      onChange: () => table.toggleAllPageRowsSelected(!table.getIsAllPageRowsSelected())
    }),
    cell: ({ row }) => h('input', { 
      type: 'checkbox', 
      class: 'w-4 h-4 cursor-pointer',
      checked: row.getIsSelected(),
      onChange: () => row.toggleSelected(!row.getIsSelected())
    })
  },
  {
    accessorKey: 'title',
    header: createSortableHeader('Title')
  },
  {
    accessorKey: 'description',
    header: createSortableHeader('Description'),
    cell: ({ row }) => h('p', { class: 'max-w-xs truncate' }, row.getValue('description'))
  },
  {
    accessorKey: 'price',
    header: createSortableHeader('Price'),
    cell: ({ row }) => `$${row.getValue('price')}`
  },
  {
    accessorKey: 'rating',
    header: createSortableHeader('Rating'),
    cell: ({ row }) => h('span', 
      { class: Number(row.getValue('rating')) < 4.5 
        ? 'text-red-500' 
        : 'text-green-500' 
      }, 
      row.getValue('rating')
    )
  },
  {
    accessorKey: 'brand',
    header: createSortableHeader('Brand')
  },
  {
    accessorKey: 'category',
    header: createSortableHeader('Category')
  },
  {
    accessorKey: 'thumbnail',
    header: 'Image',
    cell: ({ row }) => h('img', { src: row.getValue('thumbnail'), alt: row.getValue('title'), class: 'w-25 h-25 object-cover' })
  },
  {
    id: 'actions',
    header: '',
    cell: ({ row }) => h('button', {
      class: 'px-2 py-2 text-right cursor-pointer pr-4',
    }, '⋮')
  }
]
</script>

<template>
  <div class="p-4">
    <div class="flex gap-4 my-4">
      <p class="text-gray-400 mr-auto">
        <span>{{ selectedCount }}</span> selected
      </p>
      <UInput v-model="globalFilter" class="max-w-sm" placeholder="Filter..." />
    </div>
    
    <div class="border border-gray-100 rounded-lg">
      <div class="overflow-x-auto">
        <UTable
          ref="table"
          v-model:pagination="pagination"
          v-model:global-filter="globalFilter"
          :data="products"
          :columns="columns"
          :loading="status === 'pending' || status === 'idle'"
          :ui="{
            th: {
              base: 'px-3 py-2 font-normal text-gray-500',
              padding: 'px-4 py-2'
            },
            td: {
              base: 'border-b border-gray-100',
              padding: 'px-4 py-2'
            },
            tr: {
              base: 'hover:bg-gray-50'
            },
            thead: 'bg-gray-100 font-normal'
          }"
          class="w-full text-sm md:text-base"
          :pagination-options="{
            getPaginationRowModel: getPaginationRowModel()
          }"
        />
      </div>
      
      <div class="flex justify-between items-center text-gray-400 p-4 text-sm border-t border-gray-100">
        <div>
          Show
          <input 
            :value="pagination.pageSize" 
            @input="updatePageSize"
            class="w-7 mx-1 py-1 text-center text-black border border-black rounded-xs" 
            type="text"
          > of {{ products.length }} results
        </div>
        <UPagination
            :page="(table?.tableApi?.getState().pagination.pageIndex || 0) + 1"
            :items-per-page="table?.tableApi?.getState().pagination.pageSize"
            :total="table?.tableApi?.getFilteredRowModel().rows.length"
            @update:page="(p) => table?.tableApi?.setPageIndex(p - 1)"
          />
      </div>
    </div>
  </div>
</template>
