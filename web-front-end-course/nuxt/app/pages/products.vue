<script setup lang="ts">
useHead({
  title: "Products",
});
const { data: annualPricingData } = await useFetch("/api/pricing");
</script>

<template>
  <div class="flex justify-around gap-6 p-8" v-if="annualPricingData">
    <Product
      v-for="product in annualPricingData"
      :key="product.id"
      :product
      :isButtonHidden="false"
    />
  </div>
</template>
