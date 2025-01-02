import { useEffect, useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';

import { format } from 'date-fns';
import { pl } from 'date-fns/locale';
import { Popcorn } from 'lucide-react';
import { FormProvider, useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner';
import { z } from 'zod';

import { PATHS } from 'Api/paths';
import { useUserCategoryById } from 'Api/queries/useUserCategoryById';
import { useUserMovieById } from 'Api/queries/useUserMovieById';
import { useUserScreeningById } from 'Api/queries/useUserScreeningById';
import { useUserScreeningSeats } from 'Api/queries/useUserScreeningSeats';
import { Badge, BadgeVariant } from 'Components/Badge';
import { Button } from 'Components/Button';
import { Card } from 'Components/Card';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from 'Components/Dialog';
import { FormField } from 'Components/FormField';
import { Input } from 'Components/Input';
import { Skeleton } from 'Components/Skeleton';
import { queryHelpers } from 'Hooks/queryHelpers';
import { ROUTES } from 'Routing/routes';
import useOrderStore from 'Store/orderStore';
import { Order } from 'Types/order';
import { formatDuration } from 'Utils/formatDuration';
import { formatSeats } from 'Utils/formatSeats';

export const UserOrderSummary = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const form = useForm<Order>({
    resolver: zodResolver(
      z.object({
        email: z.string().nonempty('To pole jest wymagane').email('Niepoprawny adres e-mail'),
        phoneNumber: z.string().nonempty('To pole jest wymagane').length(9, 'Dokładnie 9 znaków'),
      })
    ),
  });
  const { screeningId, chosenSeats, clearOrder } = useOrderStore();
  const navigate = useNavigate();

  const { data: screening, isFetching: isFetchingScreening } = useUserScreeningById(screeningId);
  const { data: movie, isFetching: isFetchingMovie } = useUserMovieById(screening?.movieId);
  const { data: category, isFetching: isFetchingCategory } = useUserCategoryById(movie?.categoryId);
  const { data: seatAvailability, isFetching: isFetchingSeats } =
    useUserScreeningSeats(screeningId);

  const totalPrice = chosenSeats.length * 20;
  const { mutate: createDictElem, isLoading: isLoadingCreate } = queryHelpers.POST(
    PATHS.ORDERS.PUBLIC.CREATE_ORDER,
    {
      onSuccess: async () => {
        toast.success('Złożono zamówienie');
        setTimeout(() => {
          clearOrder();
          setIsModalOpen(false);
          navigate(ROUTES.public.HOME);
        }, 3000);
      },
      onError: () => {
        toast.error('Coś poszło nie tak!');
      },
    }
  );

  const onSubmitHandler = async () => {
    await form.handleSubmit(data =>
      createDictElem({ ...data, screeningId, seatIds: chosenSeats })
    )();
  };

  useEffect(() => {
    if (!screeningId || !chosenSeats.length) {
      navigate(ROUTES.public.HOME, { replace: true });
    }
  }, [screeningId, chosenSeats]);

  if (
    !movie ||
    !screening ||
    !category ||
    !seatAvailability ||
    isFetchingMovie ||
    isFetchingCategory ||
    isFetchingSeats ||
    isFetchingScreening
  )
    return (
      <div className="h-[35rem] py-8">
        <Skeleton rows={6} />
      </div>
    );

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
                {category && <Badge>{category.name}</Badge>}
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
                <Button
                  isLoading={isLoadingCreate}
                  isDisabled={isLoadingCreate}
                  onClick={onSubmitHandler}
                  className="w-full"
                >
                  Złóż zamówienie
                </Button>
              </FormProvider>
            </div>
          </div>
        </Card>
      </div>
      <Dialog
        open={isModalOpen}
        onOpenChange={() => {
          return;
        }}
      >
        <DialogContent isClosable={false} className="sm:max-w-[625px]">
          <DialogHeader>
            <DialogTitle className="text-2xl">Zamówienie złożone!</DialogTitle>
          </DialogHeader>
          <div className="flex flex-col gap-2">
            <p className="text-lg">Na Twoją skrzynkę pocztową wysłaliśmy kod weryfikacyjny.</p>
            <p className="text-lg">Nastąpi przekierowanie...</p>
            <Popcorn className="mx-auto mt-6 animate-bounce" />
          </div>
        </DialogContent>
      </Dialog>
    </div>
  );
};
