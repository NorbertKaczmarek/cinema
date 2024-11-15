import { FC } from 'react';

import { FormProvider, UseFormReturn } from 'react-hook-form';

import { useAdminCategories } from 'Api/queries/useAdminCategories';
import { Card } from 'Components/Card';
import { FormField } from 'Components/FormField';
import { Input } from 'Components/Input';
import { Select } from 'Components/Select';
import { Movie } from 'Types/movie';

interface Props {
  form: UseFormReturn<Movie>;
  isDisabled?: boolean;
}

export const MovieForm: FC<Props> = ({ form, isDisabled = false }) => {
  const { data } = useAdminCategories({ page: 0, size: 5 });

  const categoryOptions = (data || []).map(({ name, id }) => ({ label: name, value: id }));

  return (
    <Card>
      <FormProvider {...form}>
        <form className="grid grid-cols-1 gap-6">
          <FormField name="title" label="Tytuł" control={form.control}>
            <Input isDisabled={isDisabled} />
          </FormField>
          <FormField name="categoryId" label="Kategoria" control={form.control}>
            <Select options={categoryOptions} disabled={isDisabled} />
          </FormField>
          <FormField name="description" label="Opis" control={form.control}>
            <Input isDisabled={isDisabled} />
          </FormField>
          <FormField name="posterUrl" label="Link do plakatu" control={form.control}>
            <Input isDisabled={isDisabled} />
          </FormField>
          <FormField name="director" label="Reżyser" control={form.control}>
            <Input isDisabled={isDisabled} />
          </FormField>
          <FormField name="cast" label="Obsada" control={form.control}>
            <Input isDisabled={isDisabled} />
          </FormField>
          <FormField name="rating" label="Oceny" control={form.control}>
            <Input type="number" min="0" step="0.01" isDisabled={isDisabled} />
          </FormField>
          <FormField name="duration" label="Długość" control={form.control}>
            <Input type="number" min="0" step="1" isDisabled={isDisabled} />
          </FormField>
        </form>
      </FormProvider>
    </Card>
  );
};
