import { useNavigate } from 'react-router-dom';

import { useAdminMovies } from 'Api/queries/useAdminMovies';

interface Props {
  route: string;
  movieId: string;
}

export const MovieCell = ({ route, movieId }: Props) => {
  const { data } = useAdminMovies({ page: 0, size: 0 });
  const navigate = useNavigate();

  const movie = (data?.content || []).find(({ id }) => id === movieId);

  return (
    <div className="cursor-pointer text-blue-500" onClick={() => navigate(route)}>
      {movie?.title ?? ''}
    </div>
  );
};
