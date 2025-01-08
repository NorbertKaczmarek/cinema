import { format } from 'date-fns';
import { pl } from 'date-fns/locale';
import { Calendar, Clock, Star } from 'lucide-react';
import { useNavigate, useParams } from 'react-router-dom';

import { useUserCategoryById } from 'Api/queries/useUserCategoryById';
import { useUserMovieById } from 'Api/queries/useUserMovieById';
import { Badge } from 'Components/Badge';
import { Card } from 'Components/Card';
import { Skeleton } from 'Components/Skeleton';
import { capitalizeFirstLetter } from 'Utils/capitalize';
import { formatDateTime } from 'Utils/formatDateTime';
import { formatDuration } from 'Utils/formatDuration';
import { getYouTubeId } from 'Utils/getYoutubeId';

export const UserMoviePreview = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const { data: movie, isFetching: isFetchingMovie } = useUserMovieById(id);
  const { data: category, isFetching: isFetchingCategory } = useUserCategoryById(movie?.categoryId);

  const handleRedirect = (url: string) => navigate(`/order/${url}`);

  if (!movie || !category || isFetchingMovie || isFetchingCategory)
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
                {category && <Badge>{category.name}</Badge>}
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
                {(movie?.upcomingScreenings || []).map(screening => (
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
                        {`${format(new Date(screening.startDateTime), 'dd.MM', {
                          locale: pl,
                        })}, ${formatDateTime(screening.startDateTime)['time']}`}
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
