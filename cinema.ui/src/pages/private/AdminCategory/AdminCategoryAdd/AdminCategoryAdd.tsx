import { ActionButtons } from 'Components/ActionButtons';
import { Header } from 'Components/Header';
import { Spinner } from 'Components/Spinner';
import { useDictionaryState } from 'Hooks/useDictionaryState';
import { CategoryForm } from 'Pages/private/AdminCategory/components';
import { CATEGORY_DICT_STATE } from 'Pages/private/AdminCategory/constants';
import { Category } from 'Types/category';

export const AdminCategoryAdd = () => {
  const { form, isSpinning, handleCreateElem, handleRedirect } =
    useDictionaryState<Category>(CATEGORY_DICT_STATE);

  return (
    <Spinner isSpinning={isSpinning}>
      <div className="flex flex-col gap-6">
        <Header title="Nowa kategoria" onClick={handleRedirect} />
        <CategoryForm form={form} />
        <ActionButtons isEdit onCancel={handleRedirect} onSubmit={handleCreateElem} />
      </div>
    </Spinner>
  );
};
