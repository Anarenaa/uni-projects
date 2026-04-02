<script setup lang="ts">
import type { TableColumn } from '@nuxt/ui'

useHead({
  title: 'Products Table'
})

type Product = {
  id: number
  title: string
  description: string
  category: string
  price: number
  discountPercentage: number
  rating: number
  stock: number
  tags: string[]
  brand: string
  sku: string
  weight: number
  dimensions: {
    width: number
    height: number
    depth: number
  }
  warrantyInformation: string
  shippingInformation: string
  availabilityStatus: string
  reviews: {
    rating: number
    comment: string
    date: string
    reviewerName: string
    reviewerEmail: string
  }[]
  returnPolicy: string
  minimumOrderQuantity: number
  meta: {
    createdAt: string
    updatedAt: string
    barcode: string
    qrCode: string
  }
  images: string[]
  thumbnail: string
}

const { data, status } = useLazyFetch<{ products: Product[] }>('https://dummyjson.com/products', {
  key: 'table-products',
  server: false
})

const products = computed(() => data.value?.products || [])

const pageSize = ref(5)
const currentPage = ref(1)
const selectedProducts = ref<number[]>([])

const paginatedProducts = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value
  const end = start + pageSize.value
  return products.value.slice(start, end)
})

const updatePageSize = (value: string) => {
  const newSize = parseInt(value)
  if (!isNaN(newSize) && newSize > 0) {
    pageSize.value = newSize
    currentPage.value = 1
  }
}

const toggleSelectAll = () => {
  if (selectedProducts.value.length === paginatedProducts.value.length) {
    selectedProducts.value = []
  } else {
    selectedProducts.value = paginatedProducts.value.map(p => p.id)
  }
}

const toggleSelect = (productId: number) => {
  const index = selectedProducts.value.indexOf(productId)
  if (index > -1) {
    selectedProducts.value.splice(index, 1)
  } else {
    selectedProducts.value.push(productId)
  }
}

const isAllSelected = computed(() => {
  return paginatedProducts.value.length > 0 && 
         selectedProducts.value.length === paginatedProducts.value.length
})

const isIndeterminate = computed(() => {
  return selectedProducts.value.length > 0 && 
         selectedProducts.value.length < paginatedProducts.value.length
})

const columns: TableColumn<Product>[] = [
  {
    id: 'select',
    header: () => h('input', { 
      type: 'checkbox', 
      class: 'w-4 h-4 cursor-pointer',
      checked: isAllSelected.value,
      indeterminate: isIndeterminate.value,
      onChange: toggleSelectAll
    }),
    cell: ({ row }) => h('input', { 
      type: 'checkbox', 
      class: 'w-4 h-4 cursor-pointer',
      checked: selectedProducts.value.includes(row.original.id),
      onChange: () => toggleSelect(row.original.id)
    })
  },
  {
    accessorKey: 'title',
    header: 'Title'
  },
  {
    accessorKey: 'description',
    header: 'Description',
    cell: ({ row }) => h('p', { class: 'max-w-xs truncate' }, row.getValue('description'))
  },
  {
    accessorKey: 'price',
    header: 'Price',
    cell: ({ row }) => `$${row.getValue('price')}`
  },
  {
    accessorKey: 'rating',
    header: 'Rating',
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
    header: 'Brand'
  },
  {
    accessorKey: 'category',
    header: 'Category'
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
        <span>{{ selectedProducts.length }}</span> selected
      </p>
      <button class="flex gap-2 items-center cursor-pointer">
        <div class="w-6 h-6">
          <svg xmlns="http://www.w3.org/2000/svg" width="current" height="current" viewBox="0 0 24 24">
            <path fill="currentColor" fill-rule="evenodd"
              d="M22.75 7a.75.75 0 0 1-.75.75H2a.75.75 0 0 1 0-1.5h20a.75.75 0 0 1 .75.75m-3 5a.75.75 0 0 1-.75.75H5a.75.75 0 0 1 0-1.5h14a.75.75 0 0 1 .75.75m-3 5a.75.75 0 0 1-.75.75H8a.75.75 0 0 1 0-1.5h8a.75.75 0 0 1 .75.75"
              clip-rule="evenodd" />
          </svg>
        </div>
        <span>Sort by</span>
      </button>
      <button class="flex gap-2 items-center cursor-pointer">
        ⋮
        <span>Actions</span>
      </button>
    </div>
    
    <div class="border border-gray-100 rounded-lg">
      <div class="overflow-x-auto">
        <UTable
          :data="paginatedProducts"
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
        />
      </div>
      
      <div class="flex justify-between items-center text-gray-400 p-4 text-sm border-t border-gray-100">
        <div>
          Show
          <input 
            v-model="pageSize" 
            @input="updatePageSize"
            class="w-7 mx-1 py-1 text-center text-black border border-black rounded-xs" 
            type="text"
          > of {{ products.length }} results
        </div>
        <div class="py-1 px-2.5 mx-2 bg-green-100 text-black outline outline-green-300 rounded-full">
          1
        </div>
      </div>
    </div>
  </div>
</template>
