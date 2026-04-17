import { defineStore } from 'pinia'
import type { Product } from '../types/product'

export const useSubscriptionStore = defineStore('subscription', {
  state: () => ({
    selectedSubscription: null as Product | null,
  }),

  getters: {
    getSelectedSubscription: (state) => state.selectedSubscription,
    isSubscriptionSelected: (state) => state.selectedSubscription !== null,
  },

  actions: {
    setSelectedSubscription(subscription: Product) {
      this.selectedSubscription = subscription
    },

    clearSelectedSubscription() {
      this.selectedSubscription = null
    },
  },
})