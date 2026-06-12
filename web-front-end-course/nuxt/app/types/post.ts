export default interface Post {
  id: number;
  title: string;
  slug: string;
  is_published: boolean;
  published_at: string | null;
  user?: { id: number; name: string };
  category?: { id: number; title: string };
};