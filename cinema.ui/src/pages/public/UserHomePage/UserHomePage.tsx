import { useMemo, useState } from 'react';

import { addDays, format, parseISO } from 'date-fns';
import { pl } from 'date-fns/locale';
import { Clock, Star } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

import { useUserCategories } from 'Api/queries/useUserCategories';
import { useUserMovies } from 'Api/queries/useUserMovies';
import { useUserScreenings } from 'Api/queries/useUserScreenings';
import { Badge } from 'Components/Badge';
import { Button, ButtonVariant } from 'Components/Button';
import { Card } from 'Components/Card';
import { MovieCarousel } from 'Components/MovieCarousel';
import { Skeleton } from 'Components/Skeleton';
import { Tabs, TabsContent, TabsList, TabsTrigger } from 'Components/Tabs';
import { Category } from 'Types/category';
import { Movie } from 'Types/movie';
import { Screening } from 'Types/screening';
import { capitalizeFirstLetter } from 'Utils/capitalize';
import { formatDuration } from 'Utils/formatDuration';

type GroupedScreenings = {
  [date: string]: {
    [movieId: string]: Screening[];
  };
};

export const UserHomePage = () => {
  const [selectedDate, setSelectedDate] = useState(format(new Date(), 'yyyy-MM-dd'));

  const navigate = useNavigate();

  const { data: screenings, isFetching: isFetchingScreenings } = useUserScreenings({
    startDate: format(new Date(), 'yyyy-MM-dd'),
    endDate: format(addDays(new Date(), 7), 'yyyy-MM-dd'),
  });
  const { data: movies, isFetching: isFetchingMovies } = useUserMovies();
  const { data: categories, isFetching: isFetchingCategories } = useUserCategories({
    page: 0,
    size: 0,
  });

  const dates = Array.from({ length: 7 }, (_, i) => format(addDays(new Date(), i), 'yyyy-MM-dd'));

  const handleNavigate = (screeningId: string) => navigate(`/order/${screeningId}`);

  const groupedScreenings = useMemo(() => {
    if (!screenings) return {};

    return screenings.reduce((acc, screening) => {
      const date = screening.startDateTime.split('T')[0];
      if (!acc[date]) acc[date] = {};
      if (!acc[date][screening.movieId]) acc[date][screening.movieId] = [];
      acc[date][screening.movieId].push(screening);
      return acc;
    }, {} as GroupedScreenings);
  }, [screenings]);

  const moviesMap = (movies || []).reduce(
    (acc, movie) => ({ ...acc, [movie.id]: movie }),
    {} as {
      [id: string]: Movie;
    }
  );

  const categoriesMap = (categories?.content || []).reduce(
    (acc, category) => ({
      ...acc,
      [category.id]: category,
    }),
    {} as { [id: string]: Category }
  );

  if (isFetchingScreenings || isFetchingMovies || isFetchingCategories)
    return (
      <div className="h-[35rem] py-8">
        <Skeleton rows={6} />
      </div>
    );

  return (
    <div>
      <MovieCarousel />
      <div className="mx-auto mt-6 text-foreground">
        <Card
          classNames={{ wrapper: 'min-h-screen w-full max-w-[1100px] mx-auto', content: 'pb-0' }}
        >
          <h1 className="mb-4 text-3xl font-bold">Event Schedule</h1>
          <Tabs value={selectedDate} onValueChange={setSelectedDate}>
            <TabsList className="mb-4 grid w-full grid-cols-7">
              {dates.map(date => (
                <TabsTrigger key={date} value={date}>
                  {capitalizeFirstLetter(
                    format(new Date(date), 'MMM d', {
                      locale: pl,
                    })
                  )}
                </TabsTrigger>
              ))}
            </TabsList>
            {dates.map(date => (
              <TabsContent key={date} value={date}>
                {groupedScreenings[date] ? (
                  Object.entries(groupedScreenings[date]).map(([movieId, movieScreenings]) => {
                    const movie = moviesMap[movieId];
                    const category = categoriesMap[movie.categoryId];
                    return (
                      <Card key={movieId} classNames={{ wrapper: 'mb-4', content: 'pb-0' }}>
                        <div className="flex gap-4">
                          <img
                            src={movie.posterUrl}
                            alt={movie.title}
                            className="h-96 w-1/3 rounded-lg bg-bottom object-cover shadow-lg"
                          />
                          <div className="flex-1">
                            <h2
                              className="mb-4 cursor-pointer text-4xl font-bold transition-colors hover:text-info"
                              onClick={() => navigate(`/movie/${movie.id}`)}
                            >
                              {movie.title}
                            </h2>
                            <div className="mb-6 flex items-center gap-2">
                              {category && <Badge>{category.name}</Badge>}
                              <div className="flex items-center">
                                <Star className="mr-1 h-4 w-4 text-yellow-400" />
                                <span>{movie.rating.toFixed(1)}</span>
                              </div>
                              <div className="flex items-center">
                                <Clock className="mr-1 h-4 w-4" />
                                <span>{formatDuration(movie.durationMinutes)}</span>
                              </div>
                            </div>
                            <p className="text-md mb-6 line-clamp-3">{movie.description}</p>
                            <div className="mt-2 flex flex-wrap gap-6">
                              {movieScreenings
                                .sort(
                                  (a, b) =>
                                    parseISO(a.startDateTime).getTime() -
                                    parseISO(b.startDateTime).getTime()
                                )
                                .map(screening => (
                                  <Button
                                    onClick={() => handleNavigate(screening.id)}
                                    key={screening.id}
                                    variant={ButtonVariant.Primary}
                                  >
                                    {format(parseISO(screening.startDateTime), 'HH:mm')}
                                  </Button>
                                ))}
                            </div>
                          </div>
                        </div>
                      </Card>
                    );
                  })
                ) : (
                  <p>Brak seansów w tym dniu</p>
                )}
              </TabsContent>
            ))}
          </Tabs>
        </Card>
      </div>
    </div>
  );
};
