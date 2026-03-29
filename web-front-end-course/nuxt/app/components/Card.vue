<script setup lang="ts">

interface Plan {
  id: string
  name: string
  daysFree: number
  monthlyPrice: number
  yearlyBilled: number
  originalPrice: number
  savings: number
  freeTeamMembers: number
  monthlyExtraTeamMembersPrice: number
  exports: number
  additionalExportsPrice: number
  freeSkipTraces: number
  additionalSkipTracesPrice: number
  importsPrice: number
  supportPrice: number
}

interface Props {
  plan: Plan
}
 
const props = defineProps<Props>()
const cardPlan = props.plan
</script>

<template>
    <div class="relative border border-gray-200 rounded-xl m-4 px-8 py-6 shadow-lg w-md
    before:absolute before:top-0 before:left-0 before:w-full before:h-2 before:bg-linear-to-r before:from-green-500 before:to-cyan-500 before:rounded-t-xl
    hover:outline">
        <h2 class="font-bold mb-4 text-2xl">{{cardPlan.name}} - Annual</h2>
        <span class="inline-block px-2 bg-gray-100 rounded-md font-semibold text-gray-700 mb-2 text-sm">
            {{cardPlan.daysFree}}-days free then:
        </span>
        <p class="text-gray-500 mb-2">
            <span class="text-4xl font-bold text-black">
                ${{cardPlan.monthlyPrice.toFixed(2)}}
            </span>
            /month
        </p>
        <p class="text-gray-600 text-sm mb-2">billed yearly at 
                <span class="line-through"> ${{cardPlan.originalPrice}} </span> 
                <span class="text-black font-medium"> ${{cardPlan.yearlyBilled}}</span>
        </p>
        <span class="text-green-600 font-medium text-sm bg-gray-100 px-2 py-1 rounded-md">
            ${{cardPlan.savings}} in saving
        </span>
        <button class="w-full bg-gradient-to-r from-yellow-400 to-orange-500 rounded-lg py-3 my-4 cursor-pointer
        font-medium text-base text-black capitalize
        active:outline">
            Try it free
        </button>

        <hr class="text-gray-200" />

        <ul class="my-4 flex flex-col gap-y-2 *font-medium *text-base">
            <li class="list-star relative pl-7">
                Primary user 
                {{cardPlan.freeTeamMembers === 0 
                ? 'only' 
                : `+ ${cardPlan.freeTeamMembers} free team members`}}
                <p class="text-gray-400 text-sm">
                  (extra team members for ${{cardPlan.monthlyExtraTeamMembersPrice}}/month)
                </p>
            </li>
            <li class="list-star relative pl-7">
                Save unlimited properties
            </li>
            <li class="list-star relative pl-7">
                <b>{{cardPlan.exports.toLocaleString()}}</b> exports
                <p class="text-gray-400 text-sm">(additional exports at ${{cardPlan.additionalExportsPrice}} each)</p>
            </li>
            <li class="list-star relative pl-7">
                <b>{{cardPlan.freeSkipTraces.toLocaleString()}}</b> free skip traces
                <p class="text-gray-400 text-sm">(additional skip tracing at ${{cardPlan.additionalSkipTracesPrice}} each)</p>
            </li>
            <li class="list-star relative pl-7">
                Imports ${{cardPlan.importsPrice}}
            </li>
            <li class="list-star relative pl-7">
                <b>
                    {{cardPlan.supportPrice == 0 
                    ? 'FREE' 
                    : '$' + cardPlan.supportPrice}} 
                </b>
                daily product trainings and support
            </li>
            <li class="list-star relative pl-7">
                Full suite of next-gen investing tools
            </li>
            <li class="list-star relative pl-7">
                Industry first AI powered comp tool
            </li>
            <li class="list-star relative pl-7">
                Includes dedicated support agent
            </li>
        </ul>
    </div>
</template>

<style scoped>
    .list-star::before {
        content: '✦';
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