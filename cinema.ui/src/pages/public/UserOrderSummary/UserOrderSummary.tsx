import { zodResolver } from '@hookform/resolvers/zod';

import { format } from 'date-fns';
import { pl } from 'date-fns/locale';
import { FormProvider, useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { z } from 'zod';

import { useAdminCategories } from 'Api/queries/useAdminCategories';
import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { useAdminScreenings } from 'Api/queries/useAdminScreenings';
import { useAdminScreeningSeats } from 'Api/queries/useAdminScreeningSeats';
import { Badge, BadgeVariant } from 'Components/Badge';
import { Button } from 'Components/Button';
import { Card } from 'Components/Card';
import { FormField } from 'Components/FormField';
import { Input } from 'Components/Input';
import { Skeleton } from 'Components/Skeleton';
import useOrderStore from 'Store/orderStore';
import { Order } from 'Types/order';
import { formatDuration } from 'Utils/formatDuration';
import { formatSeats } from 'Utils/formatSeats';

export const UserOrderSummary = () => {
  const form = useForm<Order>({
    resolver: zodResolver(
      z.object({
        email: z.string().nonempty('To pole jest wymagane').email('Niepoprawny adres e-mail'),
        phoneNumber: z.string().nonempty('To pole jest wymagane').length(9, 'Dokładnie 9 znaków'),
      })
    ),
  });
  const { chosenSeats, clearOrder } = useOrderStore();
  const screeningId = '08dd22c6-7606-4dd7-8547-dad7c1398f0e';

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

  const totalPrice = chosenSeats.length * 20;

  const onSubmitHandler = async () => {
    try {
      // @TODO
      // form.handleSubmit(data => createDictElem(data))();
      form.handleSubmit(data => console.log(data))();
    } catch (e) {
      const message = (e as { title: string }).title ?? 'Coś poszło nie tak.';
      toast.error(message);
    }
  };

  if (
    !movie ||
    !screening
    // !seatAvailability ||
    // isFetchingMovies ||
    // isFetchingCategories ||
    // isFetchingSeats ||
    // isFetchingScreenings
  )
    return (
      <div className="h-[35rem] py-8">
        <Skeleton rows={6} />
      </div>
    );

  // @TODO - if (!screeningId || seats.length redirect)
  // @TODO - mutation, onSuccess clear order

  return (
    <div
      className="mt-4 min-h-[calc(100vh_-_104px)] bg-background bg-cover bg-center p-8 text-foreground"
      style={{ backgroundImage: `url(${movie.backgroundUrl})` }}
    >
      <div className="mx-auto">
        <Card
          classNames={{
            wrapper: 'w-full max-w-5xl mx-auto bg-background/80 backdrop-blur-sm',
            content: 'pb-0',
          }}
        >
          <div className="flex gap-6">
            <img src={movie.posterUrl} alt={movie.title} className="w-1/4 rounded-lg shadow-lg" />
            <div className="flex-1">
              <h1 className="mb-2 text-3xl font-bold">{movie.title}</h1>
              <div className="mb-2 flex items-center gap-2">
                {categoryName && <Badge>{categoryName}</Badge>}
                <Badge variant={BadgeVariant.Info}>
                  {format(new Date(screening.startDateTime), 'dd.MM, HH:mm', {
                    locale: pl,
                  })}
                </Badge>
              </div>
              <p className="mb-2">
                <strong>Czas trwania:</strong> {formatDuration(movie.durationMinutes)}
              </p>
              <p className="mb-2">
                <strong>Wybrane miejsca:</strong>{' '}
                {formatSeats(chosenSeats, seatAvailability?.seats || [])}
              </p>
              <p className="mb-2">Razem: {totalPrice} PLN</p>

              <FormProvider {...form}>
                <form className="my-6 grid grid-cols-1 gap-6">
                  <FormField name="email" label="Adres e-mail" control={form.control}>
                    <Input />
                  </FormField>
                  <FormField name="phoneNumber" label="Numer telefonu" control={form.control}>
                    <Input />
                  </FormField>
                </form>
                <Button onClick={onSubmitHandler} className="w-full">
                  Złóż zamówienie
                </Button>
              </FormProvider>
            </div>
          </div>
        </Card>
      </div>
    </div>
  );
};
