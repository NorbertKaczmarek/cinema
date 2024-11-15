import { useAdminCategories } from 'Api/queries/useAdminCategories';
import { ActionButtons } from 'Components/ActionButtons';
import { Header } from 'Components/Header';
import { Spinner } from 'Components/Spinner';
import { useDictionaryState } from 'Hooks/useDictionaryState';
import { MOVIE_DICT_STATE } from 'Pages/private/constants';
import { Movie } from 'Types/movie';

import { MovieForm } from '../components';

export const AdminMovieAdd = () => {
  const { form, isSpinning, handleCreateElem, handleRedirect } =
    useDictionaryState<Movie>(MOVIE_DICT_STATE);

  const { isFetching } = useAdminCategories({ page: 0, size: 5 });

  return (
    <Spinner isSpinning={isSpinning || isFetching}>
      <div className="flex flex-col gap-6">
        <Header title="Nowy film" onClick={handleRedirect} />
        <MovieForm form={form} />
        <ActionButtons isEdit onCancel={handleRedirect} onSubmit={handleCreateElem} />
      </div>
    </Spinner>
  );
};
