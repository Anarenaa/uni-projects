import * as z from 'zod'

export const categorySchema = z.object({
  title: z.string({ message: "Назва є обов'язковою" })
          .min(5, "Назва повинна містити мінімум 5 символів")
          .max(200, "Назва не може перевищувати 200 символів"),
  
  slug: z.string()
          .max(200, "Slug не може перевищувати 200 символів")
          .or(z.literal(''))
          .optional(),
  
  parent_id: z.string({ message: "Оберіть батьківську категорію" }),
  
  description: z.string()
          .min(3, "Опис повинен містити мінімум 3 символи")
          .max(500, "Опис не може перевищувати 500 символів")
          .or(z.literal(''))
          .optional()
})

export type CategorySchemaType = z.output<typeof categorySchema>