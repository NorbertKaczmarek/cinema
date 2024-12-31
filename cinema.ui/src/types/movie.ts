import { Screening } from 'Types/screening';

export type Movie = {
  id: string;
  title: string;
  posterUrl: string;
  director: string;
  cast: string;
  description: string;
  categoryId: string;
  rating: number;
  durationMinutes: number;
  trailerUrl: string;
  backgroundUrl: string;
  upcomingScreenings?: Screening[];
};
