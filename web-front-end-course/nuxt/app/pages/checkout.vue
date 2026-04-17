<script setup lang="ts">
import { ref, computed } from "vue";
import { useRoute, useFetch } from "#imports";

useHead({
  title: "Checkout page",
});

const form = ref({
  card: "",
  exp: "",
  cvc: "",
  name: "",
  address: "",
  terms: false,
});

const loading = ref(false);
const error = ref("");

const subscriptionStore = useSubscriptionStore();
const route = useRoute();
const productId = computed(() => Number(route.query.productId));

const { data: pricing } = await useFetch<Product[]>("/api/pricing");

const product = computed(() => {
  if (subscriptionStore.selectedSubscription) {
    return subscriptionStore.selectedSubscription;
  }
  
  return pricing.value?.find((p) => p.id === productId.value);
});

const isCardExpired = (exp: string) => {
  const [m, y] = exp.split('/').map(Number);
  if (!m || !y) return true;

  const now = new Date();
  const currentYear = now.getFullYear() % 100; // 26 from 2026
  const currentMonth = now.getMonth() + 1;

  return y < currentYear || (y === currentYear && m < currentMonth);
};

async function submitForm() {
  const { card, exp, cvc, name, address, terms } = form.value;

  if (!card || !exp || !cvc || !name || !address || !terms) {
    error.value = "All fields are required and terms must be accepted";
    return;
  }

  if (!/^\d{16}$/.test(card.replace(/\s/g, ""))) {
    error.value = "Card number must contain 16 digits";
    return;
  }

  if (!/^(0[1-9]|1[0-2])\/[0-9]{2}$/.test(exp)) {
    error.value = "Invalid date format (MM/YY)";
    return;
  }
  
  if (isCardExpired(form.value.exp)) {
    error.value = "The card has expired or the date is incorrect";
    return;
  }

  if (!/^\d{3,4}$/.test(cvc)) {
    error.value = "CVC must be 3 or 4 digits";
    return;
  }
  loading.value = true;
  error.value = "";

  try {
    const res = await fetch("/api/subscription/create", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(form.value),
    });

    const result = await res.json();

    if (!res.ok) {
      throw new Error(
        result.statusMessage || result.message || "Payment failed",
      );
    }

    alert("Subscription started!");
    form.value = "";
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : "An error occurred";
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div class="subscription-page">
    <div class="text-center text-white bg-gray-800 py-4">Checkout</div>

    <div class="p-4">
      <NuxtLink to="/products" class="text-gray-500">
        <span class="text-xl">&laquo;</span>
        back
      </NuxtLink>
      <h3 class="capitalize font-semibold text-2xl mt-3">
        You're Almost In - Start Your 3-Day Free Trial Now!
      </h3>

      <p>
        Set uo your account to gain instant access! You won't be charged if you
        decide to cancel within 3 days
      </p>

      <div
        class="flex justify-around gap-8 my-12 items-baseline flex-wrap-reverse md:flex-nowrap"
      >
        <!-- Product Info -->
        <Product :product="product" :isButtonHidden="true" />

        <!-- Order & Payment -->
        <form
          class="max-w-full md:max-w-md lg:max-w-lg p-10 border border-gray-400 rounded-lg"
          @submit.prevent="submitForm"
        >
          <h4 class="text-lg mb-4 font-bold">Order Summary</h4>
          <div class="flex justify-between my-2">
            <span>Annual Plan</span>
            <span>${{ product.yearlyBilled.toFixed(2) }}</span>
          </div>
          <hr class="bg-gray-400 my-2" />
          <div class="flex justify-between">
            <span
              >Total Due
              <span class="text-xs"
                >(*not including sales tax where applicable)</span
              >
            </span>
            <span>${{ product.yearlyBilled.toFixed(2) }}</span>
          </div>
          <div class="flex justify-between font-bold my-2">
            <span>Due Today</span>
            <span>$0.00</span>
          </div>

          <div class="trial-note text-center bg-gray-100 py-4 my-6">
            Includes {{ product.daysFree }}-Day Free Trial
          </div>

          <h4 class="text-lg mb-4 font-bold">
            Billing Information
            <span class="text-gray-400 font-light">🛈</span>
          </h4>
          <div class="my-3">
            <h5 class="capitalize mb-1">Card details</h5>
            <div
              class="flex gap-3 bg-gray-50 py-2 px-4 border border-gray-400 rounded-md"
            >
              <div class="flex gap-1 items-center basis-2/4">
                <Icon name="lucide:credit-card" class="text-gray-400 text-xl" />
                <input
                  class="w-full min-w-0 p-1 outline-0"
                  v-model="form.card"
                  placeholder="Number"
                />
              </div>

              <input
                class="basis-1/4 w-full min-w-0 p-1 outline-0"
                v-model="form.exp"
                placeholder="MM/YY"
              />
              <input
                class="basis-1/4 w-full min-w-0 p-1 outline-0"
                v-model="form.cvc"
                placeholder="CVC"
              />
            </div>
          </div>
          <div class="my-3">
            <h5 class="capitalize mb-1">Address</h5>
            <div
              class="flex flex-col bg-gray-50 gap-2 p-4 border border-gray-400 rounded-md"
            >
              <label for="full-name-address"> Full Name </label>
              <input
                class="p-3 bg-white shadow rounded-sm"
                id="full-name-address"
                v-model="form.name"
              />
              <label for="address-field"> Address </label>
              <input
                class="p-3 bg-white shadow rounded-sm"
                id="address-field"
                v-model="form.address"
              />
            </div>
          </div>

          <div class="flex gap-2 items-baseline">
            <input
              id="terms"
              v-model="form.terms"
              type="checkbox"
              class="h-4 w-4 cursor-pointer"
            />
            <label for="terms">
              I consent to
              <a href="#" class="font-bold underline" target="_blank"
                >Terms of Use</a
              >
              and understand my {{ product.daysFree }}-day free trial will
              automatically convert to ${{
                formatCurrency(product.yearlyBilled)
              }}
              per year starting on {{ formatDateAfterDays(product.daysFree) }}.
              The yearly fee will be automatically charged each year going
              forward unless I cancel my account at least one (1) business day
              before the end of the current billing period, which can be done by
              calling (888) 463-3163.
            </label>
          </div>

          <button
            class="bg-gray-300 py-3 px-6 text-gray-600 rounded-md mt-4 cursor-pointer"
            type="submit"
          >
            {{ loading ? "Processing..." : "Try It Free" }}
          </button>

          <div v-if="error" class="text-red-500 bg-red-100 p-2 rounded mt-4">
            {{ error }}
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
