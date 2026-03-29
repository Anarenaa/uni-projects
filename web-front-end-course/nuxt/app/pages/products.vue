<script setup lang="ts">
    const annualPricingData = await $fetch('/api/pricing')
</script>

<template>
  <div class="flex justify-around" v-if="annualPricingData">
    <Card :plan="annualPricingData.plans[0]" />
    <Card :plan="annualPricingData.plans[1]" />
    <Card :plan="annualPricingData.plans[2]" />
  </div>
</template>