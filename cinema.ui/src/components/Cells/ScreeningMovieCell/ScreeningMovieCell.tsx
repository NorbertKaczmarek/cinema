import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { useAdminScreenings } from 'Api/queries/useAdminScreenings';

interface Props {
  screeningId: string;
}

export const ScreeningMovieCell = ({ screeningId }: Props) => {
  const { data: screenings } = useAdminScreenings({ page: 0, size: 0 });
  const { data: movies } = useAdminMovies({ page: 0, size: 0 });

  const movieId = (screenings?.content || []).find(({ id }) => id === screeningId)?.movieId;
  const title = (movies?.content || []).find(({ id }) => id === movieId)?.title;

  return <span>{title ?? ''}</span>;
};
