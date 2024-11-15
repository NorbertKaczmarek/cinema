import { useState } from 'react';

type UseEdit = {
  isEdit: boolean;
  openEdit: () => void;
  closeEdit: () => void;
  toggleEdit: () => void;
};

export const useEdit = (initialState = false): UseEdit => {
  const [isEdit, setIsEdit] = useState(initialState);

  const openEdit = (): void => {
    setIsEdit(true);
  };

  const closeEdit = (): void => {
    setIsEdit(false);
  };
  const toggleEdit = (): void => {
    setIsEdit(value => !value);
  };

  return { isEdit, openEdit, closeEdit, toggleEdit };
};
