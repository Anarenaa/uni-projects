<script setup lang="ts">
interface Props {
  product: Product;
  isButtonHidden?: boolean;
}

const props = defineProps<Props>();
const product = props.product;
const isButtonHidden = props.isButtonHidden ?? false;
</script>

<template>
  <div
    class="relative border border-gray-200 rounded-xl px-8 py-6 shadow-lg w-md before:absolute before:top-0 before:left-0 before:w-full before:h-2 before:bg-linear-to-r before:from-green-500 before:to-cyan-500 before:rounded-t-xl hover:outline"
  >
    <h2 class="font-bold mb-4 text-2xl">{{ product.name }} - Annual</h2>
    <span
      class="inline-block px-2 bg-gray-100 rounded-md font-semibold text-gray-700 mb-2 text-sm"
    >
      {{ product.daysFree }}-days free then:
    </span>
    <p class="text-gray-500 mb-2">
      <span class="text-4xl font-bold text-black">
        ${{ formatCurrency(product.monthlyPrice) }}
      </span>
      /month
    </p>
    <p class="text-gray-600 text-sm mb-2">
      billed yearly at
      <span class="line-through"> ${{ formatCurrency(product.originalPrice) }} </span>
      <span class="text-black font-medium"> ${{ formatCurrency(product.yearlyBilled) }}</span>
    </p>
    <span
      class="text-green-600 font-medium text-sm bg-gray-100 px-2 py-1 rounded-md"
    >
      ${{ product.savings }} in saving
    </span>

    <NuxtLink
      :to="{ path: '/checkout', query: { productId: product.id } }"
      v-if="!isButtonHidden"
      class="block w-full text-center bg-linear-to-r from-yellow-400 to-orange-500 rounded-lg py-3 my-4 cursor-pointer font-medium text-base text-black capitalize active:outline"
    >
      Try it free
    </NuxtLink>

    <hr class="text-gray-200 my-4" />

    <ul class="my-4 flex flex-col gap-y-2 *font-medium *text-base">
      <li class="list-star relative pl-7">
        Primary user
        {{
          product.freeTeamMembers === 0
            ? "only"
            : `+ ${product.freeTeamMembers} free team members`
        }}
        <p class="text-gray-400 text-sm">
          (extra team members for ${{
            product.monthlyExtraTeamMembersPrice
          }}/month)
        </p>
      </li>
      <li class="list-star relative pl-7">Save unlimited properties</li>
      <li class="list-star relative pl-7">
        <b>{{ product.exports.toLocaleString() }}</b> exports
        <p class="text-gray-400 text-sm">
          (additional exports at ${{ product.additionalExportsPrice }} each)
        </p>
      </li>
      <li class="list-star relative pl-7">
        <b>{{ product.freeSkipTraces.toLocaleString() }}</b> free skip traces
        <p class="text-gray-400 text-sm">
          (additional skip tracing at ${{ product.additionalSkipTracesPrice }}
          each)
        </p>
      </li>
      <li class="list-star relative pl-7">
        Imports ${{ product.importsPrice }}
      </li>
      <li class="list-star relative pl-7">
        <b>
          {{ product.supportPrice == 0 ? "FREE" : "$" + product.supportPrice }}
        </b>
        daily product trainings and support
      </li>
      <li class="list-star relative pl-7">
        Full suite of next-gen investing tools
      </li>
      <li class="list-star relative pl-7">
        Industry first AI powered comp tool
      </li>
      <li class="list-star relative pl-7">Includes dedicated support agent</li>
    </ul>
  </div>
</template>

<style scoped>
.list-star::before {
  content: "✦";
  position: absolute;
  left: 0;
  width: 1.25rem;
  height: 1.25rem;
  margin-top: 0.25rem;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(to right, #a3e635, #22c55e);
  background-clip: text;
  color: transparent;
  font-size: 1.5rem;
}
</style>
