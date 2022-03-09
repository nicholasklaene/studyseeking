export interface Post {
  post_id: string;
  category: string;
  created_at: number;
  description: string;
  title: string;
  tags: string[];
}

export interface CreatePost {
  title: string;
  category: string;
  description: string;
  tags: string[];
}