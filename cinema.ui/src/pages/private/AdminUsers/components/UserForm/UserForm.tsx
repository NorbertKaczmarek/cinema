import { FC } from 'react';

import { FormProvider, UseFormReturn } from 'react-hook-form';

import { Card } from 'Components/Card';
import { FormField } from 'Components/FormField';
import { Input } from 'Components/Input';
import { User } from 'Types/user';

interface Props {
  form: UseFormReturn<User>;
  isWithPassword?: boolean;
  isDisabled?: boolean;
}

export const UserForm: FC<Props> = ({ form, isDisabled = false, isWithPassword = false }) => {
  // @TODO - put
  return (
    <Card>
      <FormProvider {...form}>
        <form className="grid grid-cols-1 gap-6">
          {!isWithPassword && (
            <>
              <FormField name="firstName" label="Imię" control={form.control}>
                <Input isDisabled={isDisabled} />
              </FormField>
              <FormField name="lastName" label="Nazwisko" control={form.control}>
                <Input isDisabled={isDisabled} />
              </FormField>
            </>
          )}
          <FormField name="email" label="Adres e-mail" control={form.control}>
            <Input isDisabled={isDisabled} />
          </FormField>
          {isWithPassword && (
            <>
              <FormField name="oldPassword" label="Hasło" control={form.control}>
                <Input type="password" isDisabled={isDisabled} />
              </FormField>
              <FormField name="password" label="Nowe hasło" control={form.control}>
                <Input type="password" isDisabled={isDisabled} />
              </FormField>
              <FormField name="confirmPassword" label="Potwierdź hasło" control={form.control}>
                <Input type="password" isDisabled={isDisabled} />
              </FormField>
            </>
          )}
        </form>
      </FormProvider>
    </Card>
  );
};
