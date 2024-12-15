import { ActionButtons } from 'Components/ActionButtons';
import { Header } from 'Components/Header';
import { Spinner } from 'Components/Spinner';
import { useDictionaryState } from 'Hooks/useDictionaryState';
import { UserForm } from 'Pages/private/AdminUsers/components';
import { USER_DICT_STATE } from 'Pages/private/AdminUsers/constants';
import { User } from 'Types/user';

export const AdminUserDetails = () => {
  const { form, isEdit, isSpinning, handleEdit, handleCancel, handleUpdateElem, handleRedirect } =
    useDictionaryState<User>(USER_DICT_STATE);

  // if my id !== id from params then redirect home. taken from auth

  return (
    <Spinner isSpinning={isSpinning}>
      <div className="flex flex-col gap-6">
        <Header title="Podgląd pracownika" onClick={handleRedirect} />
        <UserForm form={form} isDisabled={!isEdit} isWithPassword />
        <ActionButtons
          isEdit={isEdit}
          onCancel={handleCancel}
          onEdit={handleEdit}
          onSubmit={handleUpdateElem}
        />
      </div>
    </Spinner>
  );
};
