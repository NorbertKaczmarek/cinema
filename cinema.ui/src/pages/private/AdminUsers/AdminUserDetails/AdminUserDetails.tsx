import { useEffect } from 'react';

import { useParams } from 'react-router-dom';

import { ActionButtons } from 'Components/ActionButtons';
import { Header } from 'Components/Header';
import { Spinner } from 'Components/Spinner';
import { useDictionaryState } from 'Hooks/useDictionaryState';
import { UserForm } from 'Pages/private/AdminUsers/components';
import { USER_DETAILS_DICT_STATE } from 'Pages/private/AdminUsers/constants';
import { useAuthStore } from 'Store/authStore';
import { User } from 'Types/user';

export const AdminUserDetails = () => {
  const { form, isEdit, isSpinning, handleEdit, handleCancel, handleUpdateElem, handleRedirect } =
    useDictionaryState<User>(USER_DETAILS_DICT_STATE);
  const { id } = useParams<{ id: string }>();

  const { userId } = useAuthStore();

  useEffect(() => {
    if (id !== userId) handleRedirect();
  }, [id, userId]);

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
