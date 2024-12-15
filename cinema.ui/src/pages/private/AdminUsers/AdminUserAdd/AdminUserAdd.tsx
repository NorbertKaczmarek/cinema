import { ActionButtons } from 'Components/ActionButtons';
import { Header } from 'Components/Header';
import { Spinner } from 'Components/Spinner';
import { useDictionaryState } from 'Hooks/useDictionaryState';
import { UserForm } from 'Pages/private/AdminUsers/components';
import { USER_DICT_STATE } from 'Pages/private/AdminUsers/constants';
import { User } from 'Types/user';

export const AdminUserAdd = () => {
  const { form, isSpinning, handleCreateElem, handleRedirect } =
    useDictionaryState<User>(USER_DICT_STATE);

  return (
    <Spinner isSpinning={isSpinning}>
      <div className="flex flex-col gap-6">
        <Header title="Nowy pracownik" onClick={handleRedirect} />
        <UserForm form={form} />
        <ActionButtons isEdit onCancel={handleRedirect} onSubmit={handleCreateElem} />
      </div>
    </Spinner>
  );
};
