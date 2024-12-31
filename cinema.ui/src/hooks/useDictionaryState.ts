import { useEffect, useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';

import { FieldValues, useForm, UseFormReturn } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import { toast } from 'sonner';
import { ZodTypeAny } from 'zod';

import queryClient from 'Configs/queryClient';
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
  listQueryKey: string[];
  schema: ZodTypeAny;
}

export const useDictionaryState = <T extends FieldValues>({
  paths,
  initialData,
  queryKey,
  listUrl,
  staleTime = 0,
  listQueryKey,
  schema,
}: UseDictionaryStateProps<T>): UseDictionaryState<T> => {
  const [dictData, setDictData] = useState<T>(initialData);
  const isMounted = useIsMounted();
  const form = useForm<T>({ resolver: zodResolver(schema) });
  const { isEdit, openEdit, closeEdit } = useEdit(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();

  const handleRedirect = () => navigate(listUrl);

  const handleInvalidate = async () => {
    await queryClient.invalidateQueries(listQueryKey);
  };

  const { isFetching } = queryHelpers.GET(paths.getData, id, queryKey(id), {
    queryKey: queryKey(id),
    staleTime,
    onSuccess: data => {
      if (isMounted()) {
        setDictData(data as T);
      }
    },
  });

  //@todo - error handling
  const { mutate: createDictElem, isLoading: isLoadingCreate } = queryHelpers.POST(
    paths.createData,
    {
      onSuccess: async () => {
        toast.success('Pomyślnie utworzono element');
        await handleInvalidate();
        handleRedirect();
      },
      onError: e => {
        const message = (e?.response?.data as string) ?? 'Coś poszło nie tak.';
        toast.error(message);
      },
    }
  );

  const { mutate: updateDictElem, isLoading: isLoadingUpdate } = queryHelpers.PUT(
    paths.updateData,
    id,
    {
      onSuccess: async data => {
        toast.success('Pomyślnie zaaktualizowano element');
        closeEdit();
        setDictData(prevState => ({ ...prevState, ...(data as T) }));
        await handleInvalidate();
      },
      onError: e => {
        const message = (e?.response?.data as string) ?? 'Coś poszło nie tak.';
        toast.error(message);
      },
    }
  );

  const { mutate: deleteDictElem, isLoading: isLoadingDelete } = queryHelpers.DELETE(
    paths.deleteData,
    id,
    {
      onSuccess: async () => {
        toast.success('Pomyślnie usunięto element');
        await handleInvalidate();
        handleRedirect();
      },
    }
  );

  const handleCreateElem = () => {
    form.handleSubmit(data => createDictElem(data))();
  };

  const handleUpdateElem = () => {
    form.handleSubmit(data => updateDictElem(data))();
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
    handleDeleteElem: () => deleteDictElem(),
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
