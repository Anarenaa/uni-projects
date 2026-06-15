export default interface Post {
  id: number;
  title: string;
  slug: string;

  is_published: boolean;
  date_published: string | null; 
  
  user_id: number;
  author_name?: string;

  category_id: number;
  category_title?: string;
}