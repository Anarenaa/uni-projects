export default interface BlogCategory {
  id: number;
  title: string;
  slug: string;
  category_parent_id: number | null;
  category_parent_title: string | null;
  description: string | null;
}