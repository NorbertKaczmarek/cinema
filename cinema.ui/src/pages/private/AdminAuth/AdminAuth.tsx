import { zodResolver } from '@hookform/resolvers/zod';

import { FormProvider, useForm } from 'react-hook-form';
import { z } from 'zod';

import { Button } from 'Components/Button';
import { Card } from 'Components/Card';
import { FormField } from 'Components/FormField';
import { Input } from 'Components/Input';

const schema = z.object({
  email: z.string().email('Niepoprawny adres email').nonempty('To pole jest wymagane'),
  password: z.string().nonempty('To pole jest wymagane'),
});

export const AdminAuth = () => {
  const form = useForm({
    resolver: zodResolver(schema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  const handleSubmit = () => {
    form.handleSubmit(data => console.log(data))();
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-background p-4">
      <Card classNames={{ wrapper: 'w-full max-w-md' }}>
        <FormProvider {...form}>
          <form className="space-y-4">
            <FormField name="email" label="Adres e-mail" control={form.control}>
              <Input />
            </FormField>
            <FormField name="password" label="Hasło" control={form.control}>
              <Input type="password" />
            </FormField>
            <Button onClick={handleSubmit} className="w-full">
              Zaloguj
            </Button>
          </form>
        </FormProvider>
      </Card>
    </div>
  );
};
