import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';

import { FormProvider, useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner';
import { z } from 'zod';

import { PATHS } from 'Api/paths';
import { Button } from 'Components/Button';
import { Card } from 'Components/Card';
import { FormField } from 'Components/FormField';
import { Input } from 'Components/Input';
import { queryHelpers } from 'Hooks/queryHelpers';
import { ROUTES } from 'Routing/routes';
import { useAuthStore } from 'Store/authStore';
import { decodeToken } from 'Utils/decode';

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

  const { token, role, setToken } = useAuthStore();

  const navigate = useNavigate();
  const handleNavigate = () => navigate(ROUTES.private.ORDER.TABLE);

  const { mutateAsync: login, isLoading } = queryHelpers.POST(PATHS.AUTH.PRIVATE.LOGIN, {
    onSuccess: async res => {
      const token = (res as { token: string }).token;
      const decoded = decodeToken(token);
      setToken(token, decoded.Role, decoded.Id);
      handleNavigate();
    },
    onError: () => {
      toast.error('Nieprawidłowy login lub hasło');
      form.reset();
    },
  });

  const handleSubmit = () => {
    form.handleSubmit(async data => login(data))();
  };

  useEffect(() => {
    if (token) handleNavigate();
  }, [token, role]);

  return (
    <div className="flex min-h-screen items-center justify-center bg-background p-4">
      <Card classNames={{ wrapper: 'w-full max-w-md', content: 'pb-0' }}>
        <FormProvider {...form}>
          <form className="space-y-4">
            <FormField name="email" label="Adres e-mail" control={form.control}>
              <Input />
            </FormField>
            <FormField name="password" label="Hasło" control={form.control}>
              <Input type="password" />
            </FormField>
            <Button onClick={handleSubmit} className="w-full" isLoading={isLoading}>
              Zaloguj
            </Button>
          </form>
        </FormProvider>
      </Card>
    </div>
  );
};
