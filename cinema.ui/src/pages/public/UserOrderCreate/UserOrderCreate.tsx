import { useState } from 'react';

import { format } from 'date-fns';
import { pl } from 'date-fns/locale';
import { useNavigate, useParams } from 'react-router-dom';

import { useAdminCategories } from 'Api/queries/useAdminCategories';
import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { useAdminScreenings } from 'Api/queries/useAdminScreenings';
import { useAdminScreeningSeats } from 'Api/queries/useAdminScreeningSeats';
import { Badge } from 'Components/Badge';
import { Button } from 'Components/Button';
import { Card } from 'Components/Card';
import { Skeleton } from 'Components/Skeleton';
import { ROUTES } from 'Routing/routes';
import useOrderStore from 'Store/orderStore';
import { Seat } from 'Types/screening';
import { capitalizeFirstLetter } from 'Utils/capitalize';
import { formatSeats } from 'Utils/formatSeats';

export const UserOrderCreate = () => {
  const [selectedSeats, setSelectedSeats] = useState<string[]>([]);
  const { id: screeningId } = useParams<{ id: string }>();
  const { setOrderData } = useOrderStore();

  const navigate = useNavigate();

  //@TODO - USER, proper get by id calls
  // const {data: screening, isFetching: isFetchingScreening} = useUserScreeningById(screeningId);
  // const { data: movie, isFetching: isFetchingMovie } = useUserMovieById(screening?.movieId);
  // const {data: category, isFetching: isFetchingCategory} = useUserCategoryById(movie?.categoryId);
  const { data: seatAvailability, isFetching: isFetchingSeats } =
    useAdminScreeningSeats(screeningId);
  // useUserScreeningSeats(screeningId)

  const { data: screenings, isFetching: isFetchingScreenings } = useAdminScreenings({
    page: 0,
    size: 0,
  });
  const { data: movies, isFetching: isFetchingMovies } = useAdminMovies({ page: 0, size: 0 });
  const { data: categories, isFetching: isFetchingCategories } = useAdminCategories({
    page: 0,
    size: 0,
  });
  const screening = (screenings?.content || []).find(({ id }) => id === screeningId);
  const movie = (movies?.content || []).find(({ id: movieId }) => movieId === screening?.movieId);
  const categoryName =
    (categories?.content || []).find(({ id }) => id === movie?.categoryId)?.name || '';

  const toggleSeatSelection = (seatId: string) => {
    setSelectedSeats(prev =>
      prev.includes(seatId) ? prev.filter(id => id !== seatId) : [...prev, seatId]
    );
  };

  const getSeatColor = (seat: Seat) => {
    if (seat.isTaken) return 'bg-red-500';
    if (selectedSeats.includes(seat.id)) return 'bg-orange-500';
    return 'bg-green-500';
  };

  const handleProceed = () => {
    if (!screeningId) return;
    setOrderData(screeningId, selectedSeats);
    navigate(ROUTES.public.ORDER.SUMMARY);
  };

  const totalPrice = selectedSeats.length * 20;

  // if (!screening || !movie || !category || !seatAvailability || isFetchingScreening || isFetchingMovie || isFetchingCategory || isFetchingSeats)
  if (
    !movie ||
    !screening ||
    !seatAvailability ||
    isFetchingMovies ||
    isFetchingCategories ||
    isFetchingSeats ||
    isFetchingScreenings
  )
    return (
      <div className="h-[35rem] py-8">
        <Skeleton rows={6} />
      </div>
    );

  const { seats } = seatAvailability;

  const formattedSeats = formatSeats(selectedSeats, seats);

  return (
    <div className="py-8 text-foreground">
      <Card classNames={{ wrapper: 'w-full mx-auto px-4 sm:px-6 lg:px-8 lg:pb-0' }}>
        <div className="flex flex-col gap-6 lg:flex-row">
          <img
            src={movie.posterUrl}
            alt={movie.title}
            className="mx-auto w-full rounded-lg shadow-lg sm:w-1/3 lg:w-1/4"
          />

          <div className="w-full lg:w-2/3">
            <div className="mb-6">
              <div className="mb-4 flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
                <h1 className="text-center text-2xl font-bold sm:text-left sm:text-3xl">
                  {movie.title}
                </h1>
                {categoryName && <Badge>{categoryName}</Badge>}
              </div>
              <p className="mb-2 text-center text-lg sm:text-left sm:text-xl">
                {capitalizeFirstLetter(
                  format(new Date(screening.startDateTime), 'EEEE dd.MM, HH:mm', {
                    locale: pl,
                  })
                )}
              </p>
            </div>

            <div>
              <div className="mx-auto my-8 h-24 w-64 skew-x-6 skew-y-2 bg-white shadow-[0_3px_10px_rgba(255,255,255,0.75)]" />
              <div className="grid grid-cols-5 gap-1 sm:grid-cols-10">
                {seats.map(seat => (
                  <Button
                    key={seat.id}
                    className={`mb-2 h-8 w-8 text-xs ${getSeatColor(seat)}`}
                    isDisabled={seat.isTaken}
                    onClick={() => toggleSeatSelection(seat.id)}
                  >
                    {seat.row}
                    {seat.number}
                  </Button>
                ))}
              </div>
            </div>
          </div>

          <div className="w-full border-t pt-6 lg:w-1/3 lg:border-l lg:border-t-0 lg:pl-6 lg:pt-0">
            <h2 className="mb-4 text-center text-xl font-bold sm:text-2xl lg:text-left">
              Podsumowanie
            </h2>
            <p className="mb-2 text-center lg:text-left">Wybrane miejsca: {formattedSeats}</p>
            <p className="mb-4 text-center lg:text-left">Razem: {totalPrice} PLN</p>
            <Button
              className="w-full"
              disabled={selectedSeats.length === 0}
              onClick={handleProceed}
            >
              Przejdź do płatności
            </Button>
          </div>
        </div>
      </Card>
    </div>
  );
};
