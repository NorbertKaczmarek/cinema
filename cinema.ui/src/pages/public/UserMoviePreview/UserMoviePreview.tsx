import { format } from 'date-fns';
import { pl } from 'date-fns/locale';
import { Calendar, Clock, Star } from 'lucide-react';
import { useNavigate, useParams } from 'react-router-dom';

import { useAdminCategories } from 'Api/queries/useAdminCategories';
import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { Badge } from 'Components/Badge';
import { Card } from 'Components/Card';
import { Skeleton } from 'Components/Skeleton';
import { capitalizeFirstLetter } from 'Utils/capitalize';
import { formatDuration } from 'Utils/formatDuration';
import { getYouTubeId } from 'Utils/getYoutubeId';

const SCREENINGS = [
  {
    id: '08dd22c6-7606-4dd7-8547-dad7c1398f0e',
    startDateTime: '2024-12-22T21:00:00+00:00',
    endDateTime: '2024-12-22T23:11:00+00:00',
    movieId: '26340785-eec8-4619-ab6a-029a3080b270',
  },
  {
    id: '8ab39155-e008-4dbb-b947-9ffe9cc5106a',
    startDateTime: '2024-12-14T15:58:00+00:00',
    endDateTime: '2024-12-14T18:10:00+00:00',
    movieId: '3b3c3d08-432f-43a5-93d4-be899325cf38',
  },
  {
    id: '98bc826f-7413-49de-8a12-8da143de8e58',
    startDateTime: '2024-12-14T15:03:00+00:00',
    endDateTime: '2024-12-14T17:21:00+00:00',
    movieId: '3d37a2cb-be61-44c3-a736-622d7efe4d9c',
  },
  {
    id: '3c8ca145-9ff1-44cf-ab2a-560d2963dc59',
    startDateTime: '2024-12-07T11:56:00+00:00',
    endDateTime: '2024-12-07T14:08:00+00:00',
    movieId: '3b3c3d08-432f-43a5-93d4-be899325cf38',
  },
  {
    id: '46cb91c4-d2b1-4c7f-8b5b-d2a32f7a95f1',
    startDateTime: '2024-11-29T15:56:00+00:00',
    endDateTime: '2024-11-29T17:55:00+00:00',
    movieId: '39cb9985-0184-495b-891d-a96e07f89307',
  },
];

export const UserMoviePreview = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  // const { data: movie, isFetching: isFetchingMovie } = useUserMovieById(id);
  // const {data: category, isFetching: isFetchingCategory} = useUserCategoryById(movie?.categoryId)
  // @TODO - user
  const { data: movies, isFetching: isFetchingMovies } = useAdminMovies({ page: 0, size: 0 });
  const { data: categories, isFetching: isFetchingCategories } = useAdminCategories({
    page: 0,
    size: 0,
  });

  const movie = (movies?.content || []).find(({ id: movieId }) => movieId === id);
  const categoryName =
    (categories?.content || []).find(({ id }) => id === movie?.categoryId)?.name || '';

  const handleRedirect = (url: string) => navigate(`/order/${url}`);

  // if (!movie || !category ||  isFetchingMovie || isFetchingCategory)
  if (!movie || isFetchingMovies || isFetchingCategories)
    return (
      <div className="h-[35rem] py-8">
        <Skeleton rows={6} />
      </div>
    );

  const youtubeId = getYouTubeId(movie.trailerUrl);

  return (
    <div className="min-h-screen bg-background text-foreground">
      <div
        className="h-[50vh] w-full bg-cover bg-center"
        style={{ backgroundImage: `url(${movie.backgroundUrl})` }}
      />
      <div className="container mx-auto -mt-32 px-4 py-8">
        <Card classNames={{ wrapper: 'bg-background/80 backdrop-blur-sm', content: 'p-6' }}>
          <div className="flex flex-col gap-8 md:flex-row">
            <img
              src={movie.posterUrl}
              alt={movie.title}
              className="w-full rounded-lg shadow-lg md:w-1/3"
            />
            <div className="flex-1">
              <h1 className="mb-2 text-4xl font-bold">{movie.title}</h1>
              <div className="mb-4 flex items-center gap-4">
                {categoryName && <Badge>{categoryName}</Badge>}
                <div className="flex items-center">
                  <Star className="mr-1 h-5 w-5 text-yellow-400" />
                  <span>{movie.rating.toFixed(1)}</span>
                </div>
                <div className="flex items-center">
                  <Clock className="mr-1 h-5 w-5" />
                  <span>{formatDuration(movie.durationMinutes)}</span>
                </div>
              </div>
              <p className="mb-4 text-lg">{movie.description}</p>
              <p className="mb-2">
                <strong>Reżyser:</strong> {movie.director}
              </p>
              <p className="mb-4">
                <strong>W rolach głównych:</strong> {movie.cast}
              </p>
              <h2 className="mb-2 text-2xl font-semibold">Nadchodzące seanse</h2>
              <div className="mb-4 grid grid-cols-1 gap-4 md:grid-cols-3">
                {SCREENINGS.slice(0, 3).map(screening => (
                  <Card
                    key={screening.id}
                    classNames={{ wrapper: 'cursor-pointer', content: 'flex pb-0 gap-2' }}
                    onWrapperClick={() => handleRedirect(screening.id)}
                  >
                    <Calendar className="h-5 w-5" />
                    <div className="flex flex-col">
                      <span>
                        {capitalizeFirstLetter(
                          format(new Date(screening.startDateTime), 'EEEE', {
                            locale: pl,
                          })
                        )}
                      </span>
                      <span>
                        {format(new Date(screening.startDateTime), 'dd.MM, HH:mm', {
                          locale: pl,
                        })}
                      </span>
                    </div>
                  </Card>
                ))}
              </div>
            </div>
          </div>
        </Card>
        {youtubeId && (
          <div className="mt-8">
            <h2 className="mb-4 text-2xl font-semibold">Zwiastun</h2>
            <div className="relative w-full" style={{ paddingBottom: '56.25%' }}>
              <iframe
                src={`https://www.youtube.com/embed/${youtubeId}`}
                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                allowFullScreen
                className="absolute left-0 top-0 h-full w-full rounded-lg"
              ></iframe>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
