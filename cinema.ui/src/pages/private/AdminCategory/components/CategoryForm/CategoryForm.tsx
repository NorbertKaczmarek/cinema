import { FC } from 'react';

import { FormProvider, UseFormReturn } from 'react-hook-form';

import { Card } from 'Components/Card';
import { FormField } from 'Components/FormField';
import { Input } from 'Components/Input';
import { Category } from 'Types/category';

interface Props {
  form: UseFormReturn<Category>;
  isDisabled?: boolean;
}

export const CategoryForm: FC<Props> = ({ form, isDisabled = false }) => {
  return (
    <Card>
      <FormProvider {...form}>
        <form className="grid grid-cols-1 gap-6">
          <FormField name="name" label="Nazwa kategorii" control={form.control}>
            <Input isDisabled={isDisabled} />
          </FormField>
        </form>
      </FormProvider>
    </Card>
  );
};
