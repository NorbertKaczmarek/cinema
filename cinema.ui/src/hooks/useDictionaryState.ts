import { useEffect, useState } from 'react';

import { FieldValues, useForm, UseFormReturn } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import { toast } from 'sonner';

import { queryHelpers } from 'Hooks/queryHelpers';
import { useEdit } from 'Hooks/useEdit';
import { useIsMounted } from 'Hooks/useIsMounted';

interface UseDictionaryStateProps<T> {
  paths: {
    getData: (id: string) => string;
    createData: string;
    updateData: (id: string) => string;
    deleteData: (id: string) => string;
  };
  initialData: T;
  queryKey: (id?: string) => string[];
  listUrl: string;
  staleTime?: number;
}

export const useDictionaryState = <T extends FieldValues>({
  paths,
  initialData,
  queryKey,
  listUrl,
  staleTime = 0,
}: UseDictionaryStateProps<T>): UseDictionaryState<T> => {
  const [dictData, setDictData] = useState<T>(initialData);
  const isMounted = useIsMounted();
  const form = useForm<T>();
  const { isEdit, openEdit, closeEdit } = useEdit(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();

  const handleRedirect = () => navigate(listUrl);

  const { isFetching } = queryHelpers.GET(paths.getData, id, queryKey(id), {
    queryKey: queryKey(id),
    staleTime,
    onSuccess: data => {
      if (isMounted()) {
        setDictData(data as T);
      }
    },
  });

  const { mutate: createDictElem, isLoading: isLoadingCreate } = queryHelpers.POST(
    paths.createData,
    {
      onSuccess: () => {
        toast.success('Pomyślnie utworzono element');
        handleRedirect();
      },
    }
  );

  const { mutate: updateDictElem, isLoading: isLoadingUpdate } = queryHelpers.PUT(
    paths.updateData,
    id,
    {
      onSuccess: data => {
        toast.success('Pomyślnie zaaktualizowano element');
        closeEdit();
        setDictData(prevState => ({ ...prevState, ...(data as T) }));
      },
    }
  );

  const { mutate: deleteDictElem, isLoading: isLoadingDelete } = queryHelpers.DELETE(
    paths.deleteData,
    id,
    {
      onSuccess: () => {
        toast.success('Pomyślnie usunięto element');
        handleRedirect();
      },
    }
  );

  const handleCreateElem = () => {
    try {
      form.handleSubmit(data => createDictElem(data))();
    } catch (e) {
      const message = (e as { title: string }).title ?? 'Coś poszło nie tak.';
      toast.error(message);
    }
  };

  const handleUpdateElem = () => {
    try {
      form.handleSubmit(data => updateDictElem(data))();
    } catch {
      toast.error('Error xD');
    }
  };

  useEffect(() => {
    form.reset(dictData);
  }, [dictData, isEdit]);

  return {
    data: dictData,
    form,
    isEdit,
    isSpinning: isFetching || isLoadingCreate || isLoadingUpdate || isLoadingDelete,
    handleCancel: closeEdit,
    handleEdit: openEdit,
    handleCreateElem,
    handleUpdateElem,
    handleDeleteElem: deleteDictElem,
    handleRedirect,
  };
};

interface UseDictionaryState<T extends FieldValues> {
  data: T;
  form: UseFormReturn<T>;
  isEdit: boolean;
  isSpinning: boolean;
  handleCancel: () => void;
  handleEdit: () => void;
  handleCreateElem: () => void;
  handleUpdateElem: () => void;
  handleDeleteElem: () => void;
  handleRedirect: () => void;
}
