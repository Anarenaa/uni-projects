import * as z from 'zod'

export const postSchema = z.object({
  title: z.string({ message: "Введіть заголовок статті" })
          .min(5, "Заголовок повинен містити мінімум 5 символів")
          .max(200, "Заголовок не може перевищувати 200 символів"),
  
  slug: z.string()
          .max(200, "Slug не може перевищувати 200 символів")
          .or(z.literal(''))
          .optional(),
  
  content_raw: z.string({ message: "Контент статті є обов'язковим" })
          .min(5, "Мінімальна довжина статті 5 символів")
          .max(10000, "Контент не може перевищувати 10000 символів"),
  
  category_id: z.string({ message: "Оберіть категорію для статті" })
})

export type PostSchemaType = z.output<typeof postSchema>