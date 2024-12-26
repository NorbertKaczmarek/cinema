import { FC } from 'react';

import { pl } from 'date-fns/locale';
import { FormProvider, UseFormReturn } from 'react-hook-form';

import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { Card } from 'Components/Card';
import { DateTimePicker } from 'Components/DatetimePicker';
import { FormField } from 'Components/FormField';
import { Select } from 'Components/Select';
import { Screening } from 'Types/screening';

interface Props {
  form: UseFormReturn<Screening>;
  isDisabled?: boolean;
}

export const ScreeningForm: FC<Props> = ({ form, isDisabled = false }) => {
  const { data } = useAdminMovies({ page: 0, size: 0 });

  const movieOptions = (data?.content || []).map(({ title, id }) => ({ label: title, value: id }));

  return (
    <Card>
      <FormProvider {...form}>
        <form className="grid grid-cols-1 gap-6">
          {/* @TODO - new datepicker */}
          <FormField name="startDateTime" label="Data rozpoczęcia seansu" control={form.control}>
            <DateTimePicker placeholder="Wybierz datę" locale={pl} disabled={isDisabled} />
          </FormField>
          <FormField name="movieId" label="Film" control={form.control}>
            <Select options={movieOptions} disabled={isDisabled} />
          </FormField>
        </form>
      </FormProvider>
    </Card>
  );
};
