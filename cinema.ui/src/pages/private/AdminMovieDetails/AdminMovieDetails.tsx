import { useAdminCategories } from 'Api/queries/useAdminCategories.ts';
import { ActionButtons } from 'Components/ActionButtons';
import { Header } from 'Components/Header';
import { Spinner } from 'Components/Spinner';
import { useDictionaryState } from 'Hooks/useDictionaryState.ts';
import { MovieForm } from 'Pages/private/components';
import { MOVIE_DICT_STATE } from 'Pages/private/constants.ts';
import { Movie } from 'Types/movie.ts';

export const AdminMovieDetails = () => {
  const {
    data,
    form,
    isEdit,
    isSpinning,
    handleCancel,
    handleEdit,
    handleDeleteElem,
    handleUpdateElem,
    handleRedirect,
  } = useDictionaryState<Movie>(MOVIE_DICT_STATE);

  const { isFetching } = useAdminCategories({ page: 0, size: 5 });

  return (
    <Spinner isSpinning={isSpinning || isFetching}>
      <div className="flex flex-col gap-6">
        <Header title={data?.title ?? 'Podgląd filmu'} onClick={handleRedirect} />
        <MovieForm form={form} isDisabled={!isEdit} />
        <ActionButtons
          isEdit={isEdit}
          onCancel={handleCancel}
          onDelete={handleDeleteElem}
          onEdit={handleEdit}
          onSubmit={handleUpdateElem}
        />
      </div>
    </Spinner>
  );
};
