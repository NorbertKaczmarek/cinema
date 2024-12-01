import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { ActionButtons } from 'Components/ActionButtons';
import { Header } from 'Components/Header';
import { Spinner } from 'Components/Spinner';
import { useDictionaryState } from 'Hooks/useDictionaryState';
import { ScreeningForm } from 'Pages/private/AdminScreening/components/ScreeningForm';
import { SCREENING_DICT_STATE } from 'Pages/private/AdminScreening/constants';
import { Screening } from 'Types/screening';

export const AdminScreeningAdd = () => {
  const { form, isSpinning, handleCreateElem, handleRedirect } =
    useDictionaryState<Screening>(SCREENING_DICT_STATE);

  const { isFetching } = useAdminMovies({ page: 0, size: 0 });

  return (
    <Spinner isSpinning={isSpinning || isFetching}>
      <div className="flex flex-col gap-6">
        <Header title="Nowy seans" onClick={handleRedirect} />
        <ScreeningForm form={form} />
        <ActionButtons isEdit onCancel={handleRedirect} onSubmit={handleCreateElem} />
      </div>
    </Spinner>
  );
};
