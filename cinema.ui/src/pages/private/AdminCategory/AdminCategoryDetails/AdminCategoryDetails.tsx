import { ActionButtons } from 'Components/ActionButtons';
import { Header } from 'Components/Header';
import { Spinner } from 'Components/Spinner';
import { useDictionaryState } from 'Hooks/useDictionaryState';
import { CategoryForm } from 'Pages/private/AdminCategory/components';
import { CATEGORY_DICT_STATE } from 'Pages/private/AdminCategory/constants';
import { Category } from 'Types/category';

export const AdminCategoryDetails = () => {
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
  } = useDictionaryState<Category>(CATEGORY_DICT_STATE);

  return (
    <Spinner isSpinning={isSpinning}>
      <div className="flex flex-col gap-6">
        <Header title={data?.name ?? 'Podgląd kategorii'} onClick={handleRedirect} />
        <CategoryForm form={form} isDisabled={!isEdit} />
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
