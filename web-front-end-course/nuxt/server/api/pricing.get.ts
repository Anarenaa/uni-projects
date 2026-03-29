export default defineEventHandler(async (event) => {

  const annualPricingData = {
    plans: [
      {
        id: 'starter-annual',
        name: 'Starter',
        daysFree: 3,
        monthlyPrice: 83.25,
        yearlyBilled: 999,
        originalPrice: 1188,
        savings: 189,
        freeTeamMembers: 0,
        monthlyExtraTeamMembersPrice: 35,
        exports: 10000,
        additionalExportsPrice: 0.02,
        freeSkipTraces: 500,
        additionalSkipTracesPrice: 0.08,
        importsPrice: 0.01,
        supportPrice: 0
      },
      {
        id: 'team-annual',
        name: 'Team',
        daysFree: 3,
        monthlyPrice: 207.50,
        yearlyBilled: 2490,
        originalPrice: 2988,
        savings: 498,
        freeTeamMembers: 2,
        monthlyExtraTeamMembersPrice: 25,
        exports: 50000,
        additionalExportsPrice: 0.01,
        freeSkipTraces: 1000,
        additionalSkipTracesPrice: 0.08,
        importsPrice: 0.01,
        supportPrice: 0
      },
      {
        id: 'business-annual',
        name: 'Business',
        daysFree: 3,
        monthlyPrice: 457.50,
        yearlyBilled: 5490,
        originalPrice: 6588,
        savings: 1098,
        freeTeamMembers: 6,
        monthlyExtraTeamMembersPrice: 20,
        exports: 100000,
        additionalExportsPrice: 0.01,
        freeSkipTraces: 2000,
        additionalSkipTracesPrice: 0.08,
        importsPrice: 0.01,
        supportPrice: 0
      }
    ]
  }

  return annualPricingData
})
